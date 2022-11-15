#if NODE_CANVAS

using NodeCanvas.Framework;
using NodeCanvas.StateMachines;

namespace Pearl.NodeCanvas
{
    public class PearlFSMOwner : FSMOwner, IFSM
    {
        public void CheckTransitions(bool forceFinishState)
        {
            IState currentState = GetCurrentState();
            if (currentState != null)
            {
                if (forceFinishState)
                {
                    currentState.Finish(true);
                }
                currentState.CheckTransitions();
            }
        }

        public void CheckTransitionsAfterChangeLabel(string label)
        {
            CheckTransitionsAfterChangeLabel(label, false);
        }

        public void CheckForceTransitionsAfterChangeLabel(string label)
        {
            CheckTransitionsAfterChangeLabel(label, true);
        }

        public void StartFSM()
        {
            ChangeLabel(string.Empty);
            StartBehaviour(updateMode, null);
        }

        public void UpdateVariable<T>(string nameVar, T content)
        {
            UpdateVariable<T>(nameVar, content, BlackboardTypeEnum.Local);
        }

        public T GetVariable<T>(string nameVar)
        {
            return GetVariable<T>(nameVar, BlackboardTypeEnum.Local);
        }

        public T RemoveVariable<T>(string nameVar)
        {
            return RemoveVariable<T>(nameVar, BlackboardTypeEnum.Local);

        }

        public void UpdateVariable<T>(string nameVar, T content, BlackboardTypeEnum type)
        {
            IBlackboard board = GetBlackboard(type);
            board.UpdateVariable<T>(nameVar, content);
        }

        public T GetVariable<T>(string nameVar, BlackboardTypeEnum type)
        {
            if (nameVar == null)
            {
                return default;
            }
            IBlackboard board = GetBlackboard(type);

            if (board == null || !board.variables.ContainsKey(nameVar))
            {
                return default;
            }

            return board.GetVariableValue<T>(nameVar);
        }

        public T RemoveVariable<T>(string nameVar, BlackboardTypeEnum type)
        {
            if (nameVar == null)
            {
                return default;
            }
            IBlackboard board = GetBlackboard(type);
            T value = GetVariable<T>(nameVar);
            board.RemoveVariable(nameVar);
            return value;

        }

        public void ChangeLabel(string newLabel)
        {
            if (blackboard == null)
            {
                return;
            }

            blackboard.UpdateVariable<string>(ConstantStrings.label, newLabel);
        }

        public string GetLabel()
        {
            var labelContainer = blackboard.GetVariable<string>(ConstantStrings.label);
            string value = string.Empty;
            if (labelContainer != null)
            {
                value = labelContainer.value;
            }
            else
            {
                ChangeLabel(value);
            }
            return value;
        }

        private IBlackboard GetBlackboard(BlackboardTypeEnum type)
        {
            if (type == BlackboardTypeEnum.Local)
            {
                return blackboard;
            }
            else if (graph != null)
            {
                return graph.blackboard;
            }
            else
            {
                return null;
            }
        }

        private void CheckTransitionsAfterChangeLabel(string label, bool force)
        {
            ChangeLabel(label);
            CheckTransitions(force);
        }
    }
}


#endif
