using System.Collections.Generic;
using UnityEngine;

namespace Pearl.AI
{
    public abstract class Sensor : MonoBehaviour
    {
        [SerializeField]
        private float timeForUpdate = 0.1f;
        [SerializeField]
        protected Transform target = null;
        [SerializeField]
        protected float viewRadius = 10;
        [SerializeField]
        protected LayerMask targetMask = default;

        protected List<Transform> _targets = new List<Transform>();
        private float _auxTime = 0f;

        public Transform Target { get { return target; } }

        protected void Update()
        {
            _auxTime += Time.deltaTime;

            if (_auxTime >= timeForUpdate)
            {
                FindTargets();
                _auxTime = 0f;
            }
        }

        protected void FoundNewTarget(Transform newTarget)
        {
            if (_targets == null)
            {
                return;
            }

            _targets.Add(newTarget);
        }

        protected virtual void FindTargets()
        {
            if (_targets == null)
            {
                return;
            }

            _targets.Clear();
        }
    }

}