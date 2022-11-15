using UnityEngine;

namespace Pearl.Events
{
    public abstract class AddPearlEventsAbstract<T> : MonoBehaviour
    {
        [SerializeField]
        private string eventName = string.Empty;

        // Start is called before the first frame update
        private void Awake()
        {
            PearlEventsManager.AddAction<T>(eventName, Invoke);
        }

        private void OnDisable()
        {
            PearlEventsManager.RemoveAction<T>(eventName, Invoke);
        }

        protected abstract void Invoke(T parameter);
    }
}