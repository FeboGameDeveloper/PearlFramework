#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ExecuteActionTask : ActionTask
    {
        public BBParameter<ComponentReference<ISetterActions>> AIControl;
        public BBParameter<string> action;
        public BBParameter<bool> useParameter;
        [Conditional("useParameter", 1)]
        public BBParameter<SingleParameterData> parameter;

        protected override void OnExecute()
        {
            if (AIControl == null || action == null)
            {
                return;
            }


            ISetterActions manager = AIControl.value.Component;
            string actionString = action.value;
            if (action != null)
            {
                if (useParameter != null && useParameter.value)
                {
                    manager.SetAction(actionString, parameter.value.Get());
                }
                else
                {
                    manager.SetAction(actionString);
                }
            }
            EndAction();
        }
    }
}
#endif

