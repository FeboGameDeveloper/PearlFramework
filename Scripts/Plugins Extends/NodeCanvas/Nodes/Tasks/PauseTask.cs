#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Events;

namespace Pearl
{
    [Category("Pearl")]
    public class PauseTask : ActionTask
    {
        [RequiredField]
        public BBParameter<bool> pause = false;

        protected override void OnExecute()
        {
            if (pause != null)
            {
                LevelManager.CallPause(pause.value);
            }
            EndAction();
        }
    }
}

#endif