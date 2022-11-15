#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class NearCheck : ConditionTask<Transform>
    {
        public BBParameter<PositionData> position;
        public BBParameter<float> minDistance;
        public BBParameter<bool> use2D;

        protected override bool OnCheck()
        {
            Vector3 destination = position != null ? position.value.Get() : default;
            Vector3 target = agent != null ? agent.position : default;

            float distance = use2D != null && use2D.value ? Vector2.Distance(destination, target) : Vector3.Distance(destination, target);
            return minDistance != null ? distance.MinorOrEquals(minDistance.value) : false;
        }
    }
}

#endif
