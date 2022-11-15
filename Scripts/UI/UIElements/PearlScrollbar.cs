using Pearl.Audio;
using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class PearlScrollbar : Scrollbar, IPearlSelectable
    {
        public event Action OnSelected;
        public event Action OnDeselected;
        public event Action OnHighlighted;
        public event Action OnNotHighlighted;
        public event Action OnUp;
        public event Action OnPressed;

        [SerializeField]
        private bool useBackSound = false;
        [SerializeField]
        private bool useSound = true;

        private bool _isSelectable = false;
        private object _item;


        public bool UseBackSound { get { return useBackSound; } set { useBackSound = value; } }

        public bool UseSound { get { return useSound; } set { useSound = value; } }

        public bool IsSelectable { get { return _isSelectable; } }

        public object Item { set { _item = value; } }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnHighlighted?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            OnPressed?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            OnUp?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnNotHighlighted?.Invoke();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            _isSelectable = true;
            OnSelected?.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            _isSelectable = false;
            OnDeselected?.Invoke();
        }

        public override Selectable FindSelectableOnDown()
        {
            Selectable selectable = base.FindSelectableOnDown();

            if (selectable == null)
            {
                selectable = navigation.selectOnRight;
            }

            if (selectable != null && selectable.isActiveAndEnabled && selectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                if (Application.isPlaying && useSound)
                {
                    AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);
                }
                return selectable;
            }
            return null;
        }

        public override Selectable FindSelectableOnLeft()
        {
            Selectable selectable = base.FindSelectableOnLeft();

            if (selectable == null)
            {
                selectable = navigation.selectOnRight;
            }

            if (selectable != null && selectable.isActiveAndEnabled && selectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                if (Application.isPlaying && useSound)
                {
                    AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);
                }
                return selectable;
            }
            return null;
        }

        public override Selectable FindSelectableOnRight()
        {
            Selectable selectable = base.FindSelectableOnRight();

            if (selectable == null)
            {
                selectable = navigation.selectOnRight;
            }

            if (selectable != null && selectable.isActiveAndEnabled && selectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                if (Application.isPlaying && useSound)
                {
                    AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);
                }
                return selectable;
            }
            return null;
        }

        public override Selectable FindSelectableOnUp()
        {
            Selectable selectable = base.FindSelectableOnUp();

            if (selectable == null)
            {
                selectable = navigation.selectOnRight;
            }

            if (selectable != null && selectable.isActiveAndEnabled && selectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                if (Application.isPlaying && useSound)
                {
                    AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);
                }
                return selectable;
            }
            return null;
        }

        public void CallTriggerEvent(string ev)
        {
            PearlEventsManager.CallEvent(ev, PearlEventType.Normal);
        }
    }
}
