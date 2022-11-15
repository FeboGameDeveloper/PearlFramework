#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class CheckGameFlowFSMTask : ActionTask
    {
        public BBParameter<bool> forceFinishBB;

        protected override void OnExecute()
        {
            bool isForce = forceFinishBB != null && forceFinishBB.value;
            GameManager.CheckTransitions(isForce);
            EndAction();
        }
    }
}

#endif