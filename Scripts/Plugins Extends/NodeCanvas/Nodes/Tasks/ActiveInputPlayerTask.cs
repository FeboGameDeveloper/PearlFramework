#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Input;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ActiveInputPlayerTask : ActionTask
    {
        public BBParameter<bool> active;
        public BBParameter<int> numPlayer = 0;

        protected override void OnExecute()
        {
            if (active != null && numPlayer != null)
            {
                InputManager.ActivePlayer(active.value, numPlayer.value);
            }
            EndAction();
        }
    }
}
#endif

