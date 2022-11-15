#if NODE_CANVAS

using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ChangeVarElementInBlackboardTask<T> : ActionTask<FSMOwner>
    {
        public enum ChangeVarElementInBlackboardTaskEnum { Add, Remove }

        public BBParameter<ChangeVarElementInBlackboardTaskEnum> action;
        public BBParameter<string> nameVar;
        [Conditional("action", (int)ChangeVarElementInBlackboardTaskEnum.Add)]
        public BBParameter<T> defaultValue;

        protected override void OnExecute()
        {
            if (blackboard != null && nameVar != null && action != null)
            {
                if (action.value == ChangeVarElementInBlackboardTaskEnum.Add)
                {
                    T auxDfaultValue = defaultValue != null ? defaultValue.value : default;

                    blackboard.UpdateVariable(nameVar.value, auxDfaultValue);
                }
                else
                {
                    blackboard.RemoveVariable(nameVar.value);
                }
            }
            EndAction();
        }
    }
}

#endif
