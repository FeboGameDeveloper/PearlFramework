#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;
using System.Collections.Generic;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class PearlEventsManagerTask : ActionTask
    {
        [RequiredField]
        public BBParameter<List<string>> triggerEvents;

        protected override void OnExecute()
        {
            if (triggerEvents != null && triggerEvents.value.IsNotNull(out List<string> auxTriggerEvents))
            {
                foreach (var trigger in auxTriggerEvents)
                {
                    PearlEventsManager.ClearTrigger(trigger);
                }
            }
            EndAction();
        }
    }
}

#endif