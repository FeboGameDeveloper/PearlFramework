#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class CreatePrefabTask : ActionTask
    {
        [RequiredField]
        public BBParameter<GameObject> prefab;

        protected override void OnExecute()
        {
            if (prefab != null)
            {
                GameObjectExtend.CreateGameObject(prefab.value, out _);
            }
            EndAction();
        }
    }
}

#endif
