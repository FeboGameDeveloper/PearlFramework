#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class AnimationsPageTask : ActionTask
    {
        [RequiredField]
        public BBParameter<string> animationName;
        [RequiredField]
        public BBParameter<GameObject> prefab;
        [RequiredField]
        public BBParameter<CanvasTipology> canvasTipology;

        protected override void OnExecute()
        {
            if (animationName != null && canvasTipology != null && prefab.IsExist(out var pagePrefab))
            {
                Transform pageObj = CanvasManager.GetWidget(pagePrefab.name, canvasTipology.value);

                if (pageObj != null && pageObj.TryGetComponent<GenericPage>(out GenericPage page))
                {
                    //page.StartAnimation(animationName.value);
                }


            }
            EndAction();
        }
    }
}

#endif