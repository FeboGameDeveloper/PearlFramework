#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Audio;

namespace Pearl
{
    [Category("Pearl")]
    public class ChangeSnapshotsTask : ActionTask
    {
        [RequiredField]
        public BBParameter<float> timeToReach = 0;
        [RequiredField]
        public BBParameter<SnapshotWeight[]> snapshotStruct = null;

        protected override void OnExecute()
        {
            if (timeToReach != null && snapshotStruct != null)
            {
                AudioManager.ChangeSnapshot(timeToReach.value, snapshotStruct.value);
            }
            EndAction();
        }
    }
}

#endif
