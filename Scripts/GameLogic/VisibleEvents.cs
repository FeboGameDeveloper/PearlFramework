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
        private bool checkNotVisibleAfterVisible = true;

        [SerializeField]
        private Renderer _renderer = null;

        [SerializeField]
        private UnityEvent eventsOnlyVisible = null;
        [SerializeField]
        private UnityEvent eventsOnlyInvisible = null;


        private bool _isVisible = false;
        private bool _itWasVisible = false;


        // Start is called before the first frame update
        protected void Start()
        {
            if (_renderer == null)
            {
                return;
            }

            if (initStart)
            {
                CheckVisible(true);
            }
        }

        protected void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            CheckVisible();
        }

        private void CheckVisible(bool ignoreTransition = false)
        {
            if (_renderer != null)
            {
                bool aux = _renderer.isVisible;
                if (ignoreTransition || aux != _isVisible)
                {
                    _isVisible = aux;

                    if (_isVisible)
                    {
                        _itWasVisible = true;
                    }

                    if (_isVisible)
                    {
                        eventsOnlyVisible?.Invoke();
                    }
                    else if (!checkNotVisibleAfterVisible || _itWasVisible)
                    {
                        eventsOnlyInvisible?.Invoke();
                    }
                }
            }
        }
    }
}
