#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class DestroyGameObjectTask : ActionTask
    {
        public BBParameter<GameObject> obj;

        protected override void OnExecute()
        {
            if (obj != null)
            {
                GameObjectExtend.DestroyExtend(obj.value);
            }
            EndAction();
        }
    }
}

#endif
