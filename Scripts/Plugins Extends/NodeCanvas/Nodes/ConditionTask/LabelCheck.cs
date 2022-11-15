#if NODE_CANVAS

using NodeCanvas.Framework;
using NodeCanvas.StateMachines;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class LabelCheck : ConditionTask<PearlFSMOwner>
    {
        public BBParameter<bool> useAutomaticString = false;

        [Conditional("useAutomaticString", 0)]
        public BBParameter<string> labelString;
        public BBParameter<bool> deleteLabelAfterCheck;

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override string info
        {
            get
            {
                if (useAutomaticString == null || !useAutomaticString.value)
                {
                    return "[" + labelString?.ToString() + "]";
                }
                return "[PreviousState]To[NextState]";
            }
        }

        protected override bool OnCheck()
        {
            base.OnCheck();

            string newLabel = useAutomaticString != null && useAutomaticString.value ? GetAutomaticString() : (labelString != null ? labelString.value : null);


            if (agent != null)
            {
                string currentLabel = agent.GetLabel();

                bool isCheck = currentLabel != null && newLabel != null && currentLabel.EqualsIgnoreCase(newLabel);

                if (((invert && !isCheck) || (!invert && isCheck)) && deleteLabelAfterCheck != null && deleteLabelAfterCheck.value)
                {
                    agent.ChangeLabel(string.Empty);
                }

                return isCheck;
            }
            return false;
        }

        private string GetAutomaticString()
        {
            if (agent == null)
            {
                return null;
            }

            FSMConnection[] connections = agent.GetCurrentState().GetTransitions();

            if (connections == null)
            {
                return null;
            }

            foreach (var connection in connections)
            {
                if (connection == null || connection.sourceNode == null || connection.targetNode == null)
                {
                    continue;
                }


                if (connection.condition == this)
                {
                    return connection.sourceNode.name + "To" + connection.targetNode.name;
                }
            }
            return null;
        }
    }
}

#endif