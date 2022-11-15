using UnityEngine;

namespace Pearl.Events
{
    public class CallEvents : MonoBehaviour
    {
        [SerializeField]
        private string pearlEvent;
        [SerializeField]
        private PearlEventType eventType;

        public string Event { set { pearlEvent = value; } }

        public void Call()
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType);
        }

        public void Call(bool parameter)
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType, parameter);
        }

        public void Call(string parameter)
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType, parameter);
        }

        public void Call(GameObject parameter)
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType, parameter);
        }

        public void Call(int parameter)
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType, parameter);
        }

        public void Call(float parameter)
        {
            PearlEventsManager.CallEvent(pearlEvent, eventType, parameter);
        }
    }
}
