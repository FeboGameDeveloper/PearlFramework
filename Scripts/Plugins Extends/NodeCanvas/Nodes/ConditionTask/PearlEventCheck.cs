#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;
using System;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class PearlEventCheck : ConditionTask<PearlFSMOwner>
    {
        [RequiredField]
        public BBParameter<string> eventName;
        public BBParameter<bool> trigger;
        [RequiredField]
        public BBParameter<bool> useCallback;
        [Conditional("useCallback", 1)]
        public BBParameter<BlackboardTypeEnum> blackboardType = BlackboardTypeEnum.Graph;
        [Conditional("useCallback", 1)]
        public BBParameter<string> nameVarForCallback;

        private bool _triggerResult = false;

        protected override string info
        {
            get
            {
                string s = "[" + eventName.ToString() + "]";
                if (useCallback != null && useCallback.value)
                {
                    s += "\nNameCallback = " + nameVarForCallback.value;
                }
                return s;
            }
        }

        protected override void OnEnable()
        {
            PearlEventsManager.AddAction(eventName.value, OnFinish);
        }

        protected override void OnDisable()
        {
            PearlEventsManager.RemoveAction(eventName.value, OnFinish);
        }

        protected override bool OnCheck() { return _triggerResult; }

        private void OnFinish()
        {
            if (useCallback != null && useCallback.value && blackboard != null && nameVarForCallback != null)
            {
                if (blackboardType.value == BlackboardTypeEnum.Graph)
                {
                    blackboard.UpdateVariable<Action>(nameVarForCallback.value, Wait);
                }
                else
                {
                    agent.blackboard.UpdateVariable<Action>(nameVarForCallback.value, Wait);
                }
            }

            Finish();
        }

        private void Wait()
        {
            WaitManager.Call(this);
        }

        private void OnFinish(Action onFinishAction)
        {
            if (blackboard != null && nameVarForCallback != null)
            {
                if (blackboardType.value == BlackboardTypeEnum.Graph)
                {
                    blackboard.UpdateVariable<Action>(nameVarForCallback.value, onFinishAction);
                }
                else
                {
                    agent.blackboard.UpdateVariable<Action>(nameVarForCallback.value, onFinishAction);
                }

                Finish();
            }
        }

        private void Finish()
        {
            if (trigger != null && trigger.value)
            {
                _triggerResult = true;
            }
            else
            {
                YieldReturn(true);
            }
        }
    }
}

#endif