#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class BlockTimeTask : ActionTask
    {
        public BBParameter<bool> isBlockTime;

        protected override void OnExecute()
        {
            if (isBlockTime != null)
            {
                Time.timeScale = isBlockTime.value ? 0 : 1;
            }
            EndAction();
        }
    }
}

#endif
