using UnityEngine;

namespace Pearl.AI
{
    public class Ear : Sensor
    {
        [SerializeField]
        private float minLevelSound = 10;

        protected override void FindTargets()
        {
            base.FindTargets();

            if (!target)
            {
                return;
            }

            Collider2D[] targetsInViewRadus = Physics2D.OverlapCircleAll(target.position, viewRadius, targetMask);

            foreach (Collider2D targetInViewRadius in targetsInViewRadus)
            {
                targetInViewRadius.TryGetComponents<IMakeSound>(out var targets);

                foreach (var targetSound in targets)
                {
                    if (targetSound != null && targetSound.CurrentLevelSound > minLevelSound)
                    {
                        FoundNewTarget(targetSound.Origin);
                    }
                }
            }
        }
    }
}
