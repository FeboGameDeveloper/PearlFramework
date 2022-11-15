#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class FinishLoadSceneTask : ActionTask
    {
        protected override void OnExecute()
        {
            PearlEventsManager.CallEvent(ConstantStrings.FinishLoadScene, PearlEventType.Normal);
            EndAction();
        }
    }
}
#endif
