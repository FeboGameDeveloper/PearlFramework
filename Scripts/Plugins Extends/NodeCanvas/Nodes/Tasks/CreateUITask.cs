#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{

    [Category("Pearl")]
    public class CreateUITask : ActionTask
    {
        public BBParameter<bool> useDifferenceAspectRatioBB = false;

        [Conditional("useDifferenceAspectRatioBB", 1)]
        public BBParameter<Dictionary<float, GameObject>> aspectRatioDictionaryBB;
        [Conditional("useDifferenceAspectRatioBB", 0)]
        public BBParameter<GameObject> widgetBB;

        public BBParameter<CanvasTipology> canvasTipologyBB;
        public BBParameter<TypeSibilling> typeSibillingBB;

        [Conditional("typeSibillingBB", (int)TypeSibilling.SpecificIndex)]
        public BBParameter<int> positionChildBB;
        public BBParameter<bool> isDisabledBB;
        public BBParameter<string> nameElementBB;
        public BBParameter<string> modeBB;
        public BBParameter<GameObject> saveIstanceLocationBB;

        private readonly RangeDictionary<GameObject> _dict = new();

        protected override void OnExecute()
        {
            _dict.Clear();

            if (useDifferenceAspectRatioBB.IsExist(out var isVersion) && aspectRatioDictionaryBB.IsExist(out var aux) && isVersion)
            {
                foreach (var pair in aux)
                {
                    _dict.Add(pair.Key, pair.Value);
                }
            }
            else
            {
                if (widgetBB != null)
                {
                    _dict.Add(1, widgetBB.value);
                }
            }


            GameObject prefab = _dict.Find(ScreenExtend.AspectRatio);

            if (prefab && canvasTipologyBB != null && typeSibillingBB != null)
            {
                Transform container = CanvasManager.GetWidget(prefab.name, canvasTipologyBB.value);

                if (container == null)
                {
                    GameObjectExtend.CreateUIlement(prefab, out container, canvasTipology: canvasTipologyBB.value);
                    if (container != null && nameElementBB.IsExist(out var el) && !string.IsNullOrEmpty(el))
                    {
                        container.name = el;
                    }
                }

                if (container != null)
                {
                    if (saveIstanceLocationBB != null)
                    {
                        saveIstanceLocationBB.value = container.gameObject;
                    }

                    container.gameObject.SetActive(!isDisabledBB.value);

                    int pos = positionChildBB != null ? positionChildBB.value : 0;
                    container.SetSibilling(typeSibillingBB.value, pos);


                    if (modeBB != null && !string.IsNullOrWhiteSpace(modeBB.value))
                    {
                        var genericPage = container.GetComponent<GenericPage>();
                        if (genericPage)
                        {
                            genericPage.Mode = modeBB.value;
                        }
                    }
                }
            }
            EndAction();
        }
    }
}
#endif
