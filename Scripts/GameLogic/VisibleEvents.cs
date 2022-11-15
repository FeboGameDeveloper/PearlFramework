using Pearl.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public class VisibleEvents : MonoBehaviour
    {
        [SerializeField]
        private bool initStart = false;

        [SerializeField]
        private Renderer _renderer = null;
        [SerializeField]
        private BoolEvent events = null;
        [SerializeField]
        private UnityEvent eventsOnlyActive = null;
        [SerializeField]
        private UnityEvent eventsOnlyDisactive = null;

        private bool _isVisible;


        // Start is called before the first frame update
        protected void Start()
        {
            if (_renderer == null)
            {
                return;
            }
            bool aux = _renderer.isVisible;

            if (initStart)
            {
                Invoke(aux);
            }
            else
            {
                _isVisible = aux;
            }
        }

        protected void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            if (_renderer != null)
            {
                bool aux = _renderer.isVisible;
                if (aux != _isVisible)
                {
                    Invoke(aux);
                }
            }
        }

        private void Invoke(bool isVisible)
        {
            _isVisible = isVisible;

            if (_isVisible)
            {
                eventsOnlyActive?.Invoke();
            }
            else
            {
                eventsOnlyDisactive?.Invoke();
            }

            events?.Invoke(_isVisible);
        }
    }
}
