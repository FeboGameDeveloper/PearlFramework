#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Audio;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class DestroyAudioInPauseTask : ActionTask
    {
        protected override void OnExecute()
        {
            AudioManager.DestroyAudioInPause();

            EndAction();
        }
    }
}

#endif