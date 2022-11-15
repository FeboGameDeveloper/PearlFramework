using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.UI
{
    public class UI_MagneticInfiniteScrollData<T, F> : MonoBehaviour, IContentFiller<T>
    {
        public event Action<F> OnValueChanged;
        public event Action OnFill;

        [SerializeField]
        private UI_MagneticInfiniteScroll scroll = null;
        [SerializeField]
        protected bool useFiller = false;
        [SerializeField]
        private GameObject prefab = null;
        [SerializeField]
        private int multiplyNumberElements = 1;

        protected Filler<T> filler;

        private List<T> _contentList;

        protected virtual void Awake()
        {
            if (scroll)
            {
                scroll.OnScrollCurrentElement += OnNewSelection;
            }
        }

        private void OnDestroy()
        {
            if (scroll)
            {
                scroll.OnScrollCurrentElement -= OnNewSelection;
            }
        }

        private void Reset()
        {
            scroll = GetComponent<UI_MagneticInfiniteScroll>();
        }

        private void Start()
        {
            UseFill();
        }

        private void UseFill()
        {
            if (useFiller && filler != null)
            {
                filler.Fill(this, UI_MagneticInfiniteScroll.waitForContentSet);
            }
        }

        public List<T> GetContent()
        {
            return _contentList;
        }

        public void FillAndSetContent(ref List<T> content, ref T current)
        {
            FillContent(content);
            PearlInvoke.WaitForMethod<T>(UI_MagneticInfiniteScroll.waitForContentSet, SetContent, current);
        }

        public void FillContent(List<T> content)
        {
            if (content == null || scroll == null)
            {
                return;
            }

            for (int i = 1; i < multiplyNumberElements; i++)
            {
                content.AddRange(content);
            }

            var transforms = new List<Transform>();

            foreach (var info in content)
            {
                GameObjectExtend.CreateUIlement(prefab, out GameObject obj, CanvasTipology.Null);

                var element = obj.GetComponent<IElement<T, F>>();
                if (element != null)
                {
                    element.SetContent(info);
                }
                transforms.Add(obj.transform);
            }


            _contentList = content;

            scroll.SetNewItems(ref transforms);
        }

        public void SetContent(T content)
        {
            if (scroll == null)
            {
                return;
            }

            var items = scroll.Items;

            if (items == null)
            {
                return;
            }

            int equalsIndex = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item == null)
                {
                    continue;
                }

                var element = item.GetComponent<IElement<T, F>>();
                if (element != null && element.Equals(content))
                {
                    equalsIndex = i;
                    scroll.SetContentInPivot(equalsIndex);
                    OnFill?.Invoke();
                    return;
                }
            }
        }

        public F GetCurrentID()
        {
            if (scroll != null)
            {
                var obj = scroll.GetCurrentItem();
                if (obj != null)
                {
                    var element = obj.GetComponent<IElement<T, F>>();
                    if (element != null)
                    {
                        return element.ID;
                    }
                }
            }

            Debug.LogManager.Log("error");
            return default;
        }

        private void OnNewSelection(GameObject newSelection)
        {
            var element = newSelection.GetComponent<IElement<T, F>>();
            if (element != null && OnValueChanged != null)
            {
                OnValueChanged.Invoke(element.ID);
            }
        }
    }

}