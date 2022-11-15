#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class CallPearlEventTask : ActionTask
    {
        [RequiredField]
        public BBParameter<string> eventEnum;
        [RequiredField]
        public BBParameter<PearlEventType> eventType;
        [RequiredField]
        public BBParameter<bool> wantReturn;

        public BBParameter<bool> useParameter;
        [Conditional("useParameter", 1)]
        public BBParameter<SingleParameterData> parameter;

        protected override void OnExecute()
        {
            if (eventEnum != null && eventType != null)
            {
                object parameters = null;
                if (useParameter != null && useParameter.value && parameter != null)
                {
                    parameters = parameter.value.Get();
                }


                if (wantReturn != null && wantReturn.value)
                {
                    if (useParameter != null && useParameter.value)
                    {
                        PearlEventsManager.CallEventWithReturn(eventEnum.value, eventType.value, FinishAction, parameters);
                    }
                    else
                    {
                        PearlEventsManager.CallEventWithReturn(eventEnum.value, eventType.value, FinishAction);
                    }
                }
                else
                {
                    if (useParameter != null && useParameter.value)
                    {
                        PearlEventsManager.CallEvent(eventEnum.value, eventType.value, parameters);
                    }
                    else
                    {
                        PearlEventsManager.CallEvent(eventEnum.value, eventType.value);
                    }
                    EndAction();
                }
            }
            else
            {
                EndAction();
            }
        }

        private void FinishAction()
        {
            EndAction();
        }
    }
}

#endif