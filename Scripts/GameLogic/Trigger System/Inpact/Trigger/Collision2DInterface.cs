using UnityEngine;

namespace Pearl
{
    public class Collision2DInterface : TriggerAbstractInterface<Collision2D>
    {
        [SerializeField]
        private float minVelocity = 2f;
        [SerializeField]
        private Collider2D _myCollider = null;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Reset()
        {
            _myCollider = GetComponent<Collider2D>();

            base.Reset();
        }

        protected override bool IsTouching(Collision2D element)
        {
            if (_myCollider)
            {
                return _myCollider.IsTouching(element.collider);
            }

            return false;
        }

        protected void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision != null)
            {
                OnEnter(collision, collision.gameObject);
            }
        }

        protected void OnCollisionExit2D(Collision2D collision)
        {
            if (collision != null && collision.relativeVelocity.magnitude >= minVelocity)
            {
                OnExit(collision, collision.gameObject);
            }
        }
    }
}