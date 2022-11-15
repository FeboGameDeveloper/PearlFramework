#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class SetLabelFSMTask : ActionTask<PearlFSMOwner>
    {
        public BBParameter<string> newValue;

        public BBParameter<bool> gameManager;

        protected override void OnExecute()
        {
            if (newValue != null)
            {
                if (gameManager != null && gameManager.value)
                {
                    GameManager.CheckTransitionsAfterChangeLabel(newValue.value);
                }
                else
                {
                    agent.ChangeLabel(newValue.value);
                }
            }
            EndAction();
        }
    }
}

#endif