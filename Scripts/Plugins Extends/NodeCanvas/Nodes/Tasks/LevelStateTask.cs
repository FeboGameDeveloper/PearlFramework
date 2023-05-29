#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class LevelStateTask : ActionTask
    {
        public BBParameter<StateLevelEnum> stateLevelBB;

        protected override void OnExecute()
        {
            if (stateLevelBB.IsExist(out var stateLevel))
            {
                //LevelManager.StateLevel = stateLevel;
            }

            EndAction();
        }
    }
}

#endif
