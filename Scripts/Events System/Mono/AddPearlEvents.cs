using UnityEngine;
using UnityEngine.Events;

namespace Pearl.Events
{
    public class AddPearlEvents : PearlBehaviour
    {
        [SerializeField]
        private string eventName = string.Empty;

        [SerializeField]
        private UnityEvent unityEvent = null;

        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            PearlEventsManager.AddAction(eventName, Invoke);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            PearlEventsManager.RemoveAction(eventName, Invoke);
        }

        private void Invoke()
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke();
            }
        }
    }
}
