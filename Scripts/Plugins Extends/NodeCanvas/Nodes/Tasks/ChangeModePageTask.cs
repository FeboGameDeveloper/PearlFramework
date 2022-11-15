#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ChangeModePageTask : ActionTask
    {
        [RequiredField]
        public BBParameter<GameObjectReference> container;
        [RequiredField]
        public BBParameter<string> mode;


        protected override void OnExecute()
        {
            var gameObject = container != null ? container.value.GameObject : null;


            if (mode != null && !string.IsNullOrWhiteSpace(mode.value))
            {
                var genericPage = gameObject.GetComponent<GenericPage>();
                if (genericPage)
                {
                    genericPage.Mode = mode.value;
                }
            }

            EndAction();
        }
    }
}

#endif

