using System.Collections.Generic;
using UnityEngine;

namespace Pearl.AI
{
    public class FieldOfView : Sensor
    {
        [SerializeField]
        [Range(0, 360)]
        private float viewAngle = 20;

        [SerializeField]
        private LayerMask obstacleMask = default;

        [SerializeField]
        private IChangeDirection directionManagerInspector = null;

        private Vector3 _currentDirection;

        public Vector3 CurrentDirection { get { return _currentDirection; } }
        public List<Transform> Targets { get { return _targets; } }

        public float ViewAngle { get { return viewAngle; } }
        public float ViewRadius { get { return viewRadius; } }

        private void Awake()
        {
            CreateDirectionManager();
        }

        private void Reset()
        {
            IChangeDirection aux = gameObject.GetComponent<IChangeDirection>();

            target = transform;

            CreateDirectionManager();
        }

        private void OnValidate()
        {
            CreateDirectionManager();
        }

        private void ChangeDirection(Vector3 direction)
        {
            _currentDirection = direction;
        }

        private void CreateDirectionManager()
        {
            if (directionManagerInspector is IChangeDirection aux)
            {
                aux.OnChangeDirection -= ChangeDirection;
                aux.OnChangeDirection += ChangeDirection;
            }
        }

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
                Vector2 targetVector = targetInViewRadius.bounds.center;
                Vector2 targetVectorUp = new Vector2(targetInViewRadius.bounds.center.x, targetInViewRadius.bounds.max.y);
                Vector2 targetVectorDown = new Vector2(targetInViewRadius.bounds.center.x, targetInViewRadius.bounds.min.y);


                if (IsFindTarget(targetVector) || IsFindTarget(targetVectorUp) || IsFindTarget(targetVectorDown))
                {
                    FoundNewTarget(targetInViewRadius.transform);
                }
            }
        }

        private bool IsFindTarget(Vector3 targetVector)
        {
            if (!target)
            {
                return false;
            }

            Vector3 dirToTarget = (targetVector - target.position).normalized;

            if (Vector3.Angle(_currentDirection, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(targetVector, target.position);
                if (!Physics2D.Raycast(target.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    return true;
                }
            }
            return false;
        }
    }

}