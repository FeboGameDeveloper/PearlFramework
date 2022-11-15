using UnityEngine;

namespace Pearl
{
    public class CollisionInterface : TriggerAbstractInterface<Collision>
    {
        [SerializeField]
        private float minVelocity = 2f;
        [SerializeField]
        private Collider _myCollider;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Reset()
        {
            _myCollider = GetComponent<Collider>();

            base.Reset();
        }

        protected override bool IsTouching(Collision element)
        {
            if (_myCollider != null)
            {
                return _myCollider.IsTouching(element.collider);
            }
            return false;
        }

        protected void OnCollisionEnter(Collision collision)
        {
            if (collision != null)
            {
                OnEnter(collision, collision.gameObject);
            }
        }

        protected void OnCollisionExit(Collision collision)
        {
            if (collision != null && collision.relativeVelocity.magnitude >= minVelocity)
            {
                OnExit(collision, collision.gameObject);
            }
        }
    }
}