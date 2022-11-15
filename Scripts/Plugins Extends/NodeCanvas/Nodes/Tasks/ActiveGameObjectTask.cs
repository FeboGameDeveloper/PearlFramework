#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ActiveGameObjectTask : ActionTask
    {
        [RequiredField]
        public BBParameter<GameObjectReference> container;
        [RequiredField]
        public BBParameter<EnableEnum> enableState;
        [Conditional("enableState", (int)EnableEnum.Disable)]
        public BBParameter<bool> destroy;


        protected override void OnExecute()
        {
            var active = enableState.value == EnableEnum.Enable ? true : false;
            var gameObject = container != null ? container.value.GameObject : null;

            if (gameObject != null)
            {
                gameObject.SetActive(active);
                if (!active && destroy.IsExist(out var value) && value)
                {
                    GameObjectExtend.DestroyGameObject(gameObject);
                }
            }

            EndAction();
        }
    }
}

#endif
