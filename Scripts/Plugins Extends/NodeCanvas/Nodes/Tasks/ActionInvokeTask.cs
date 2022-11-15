#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ActionInvokeTask : ActionTask<PearlFSMOwner>
    {
        public BBParameter<bool> useString;

        [Conditional("useString", 0)]
        public BBParameter<Action> action;
        [Conditional("useString", 1)]
        public BBParameter<string> nameAction;
        [Conditional("useString", 1)]
        public BBParameter<BlackboardTypeEnum> blackboardType = BlackboardTypeEnum.Graph;

        protected override void OnExecute()
        {
            Action auxAction = null;
            if (useString != null && useString.value && blackboard != null && nameAction != null && blackboardType != null)
            {
                if (blackboardType.value == BlackboardTypeEnum.Graph)
                {
                    auxAction = blackboard.GetVariableValue<Action>(nameAction.value);
                }
                else
                {
                    auxAction = agent.blackboard.GetVariableValue<Action>(nameAction.value);
                }
            }
            else if (action != null)
            {
                auxAction = action.value;
            }

            auxAction?.Invoke();
            EndAction();
        }
    }
}

#endif
