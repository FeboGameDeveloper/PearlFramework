#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class PlatformCheck : ConditionTask
    {
        [RequiredField]
        public BBParameter<RuntimePlatform[]> platforms;

        protected override bool OnCheck()
        {
            if (platforms == null)
            {
                return false;
            }

            RuntimePlatform currentPlatform = Application.platform;


            bool result = false;
            foreach (var platform in platforms.value)
            {
                if (platform == currentPlatform)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}

#endif