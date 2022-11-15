#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class PlayerPrefsCheck : ConditionTask
    {
        public BBParameter<string> nameVarBB;

        protected override bool OnCheck()
        {
            if (nameVarBB == null)
            {
                return false;
            }

            return PlayerPrefs.HasKey(nameVarBB.value);
        }
    }
}

#endif