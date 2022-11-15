#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class LocalizationCheck : ConditionTask<PearlFSMOwner>
    {
        [Conditional("isStoragedBB", 0)]
        public BBParameter<bool> isFinishedBB = default;
        [Conditional("isFinishedBB", 0)]
        public BBParameter<bool> isStoragedBB = default;

        protected override bool OnCheck()
        {
            if (isFinishedBB != null && isFinishedBB.value)
            {
                return LocalizationManager.IsDone();
            }
            else
            {
                return PlayerPrefs.HasKey("Language");
            }
        }
    }
}

#endif