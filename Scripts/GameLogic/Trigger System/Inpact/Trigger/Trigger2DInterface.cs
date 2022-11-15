using UnityEngine;

namespace Pearl
{
    public class Trigger2DInterface : TriggerAbstractInterface<Collider2D>
    {
        [SerializeField]
        private Collider2D _myCollider = null;

        protected override void Awake()
        {
            base.Awake();

            if (_myCollider == null)
            {
                _myCollider = GetComponent<Collider2D>();
            }
        }

        protected override void Reset()
        {
            base.Reset();

            _myCollider = GetComponent<Collider2D>();
        }

        protected override bool IsTouching(Collider2D element)
        {
            if (_myCollider)
            {
                return _myCollider.IsTouching(element);
            }

            return false;
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider)
            {
                OnEnter(collider, collider.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider)
            {
                OnExit(collider, collider.gameObject);
            }
        }
    }
}
