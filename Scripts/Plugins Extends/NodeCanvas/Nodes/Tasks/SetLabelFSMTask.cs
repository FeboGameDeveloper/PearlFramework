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
        public BBParameter<bool> checkTransitions = false;

        protected override void OnExecute()
        {
            if (newValue != null)
            {
                if (gameManager != null && gameManager.value)
                {
                    if (checkTransitions != null && checkTransitions.value)
                    {
                        GameManager.ChangeLabel(newValue.value);
                    }
                    else
                    {
                        GameManager.CheckTransitionsAfterChangeLabel(newValue.value);
                    }
                }
                else
                {
                    if (checkTransitions != null && checkTransitions.value)
                    {
                        agent.CheckForceTransitionsAfterChangeLabel(newValue.value);
                    }
                    else
                    {
                        agent.ChangeLabel(newValue.value);
                    }
                }
            }
            EndAction();
        }
    }
}

#endif