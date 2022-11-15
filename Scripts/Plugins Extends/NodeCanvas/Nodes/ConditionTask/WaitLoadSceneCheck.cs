#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class WaitLoadSceneCheck : ConditionTask<PearlFSMOwner>
    {
        private bool _triggerResult = false;


        protected override void OnEnable()
        {
            PearlEventsManager.AddAction(ConstantStrings.FinishLoadScene, OnFinish);
        }

        protected override void OnDisable()
        {
            PearlEventsManager.RemoveAction(ConstantStrings.FinishLoadScene, OnFinish);
        }

        protected override bool OnCheck() { return _triggerResult; }

        private void OnFinish()
        {
            _triggerResult = true;
        }
    }
}

#endif
