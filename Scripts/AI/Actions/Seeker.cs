using System;
using UnityEngine;

namespace Pearl.AI
{
    public class Seeker : AbstractAction<Transform>
    {
        [SerializeField]
        private float mindDistanceForStop = 0.5f;
        [SerializeField]
        private Transform target = null;
        [SerializeField]
        private Transform destination = null;

        [SerializeField]
        private ISetDirectionMovement MovementManager = null;

        private ISetDirectionMovement _movementInterface;
        private bool _active = false;

        public event Action OnFinishSeek;


        private void Awake()
        {
            MovementManager.Cast<ISetDirectionMovement>(out _movementInterface);
        }

        private void Reset()
        {
            target = transform;
        }

        // Start is called before the first frame update
        private void Start()
        {
        }

        private void Update()
        {
            if (_active && target && destination)
            {
                Vector2 dir = Vector2Extend.Direction(target.position, destination.position);
                _movementInterface?.SetDirection(dir);

                float distance = Vector2Extend.DistancePow2(target.position, destination.position);
                if (distance <= mindDistanceForStop)
                {
                    Stop();
                }
            }
        }

        public override void Execute(Transform destination)
        {
            this.destination = destination;
            _active = true;
        }

        public override void Stop()
        {
            _movementInterface?.SetDirection(Vector3.zero);
            _active = false;
            OnFinishSeek?.Invoke();
        }
    }
}
