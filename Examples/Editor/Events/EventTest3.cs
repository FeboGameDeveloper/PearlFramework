using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.EventExamples
{
    public class EventTest3 : MonoBehaviour
    {
        // Start is called before the first frame update
        protected void Awake()
        {
            PearlEventsManager.AddAction("testWait", OnTestWait);
        }

        protected void OnDisable()
        {
            PearlEventsManager.RemoveAction("testWait", OnTestWait);
        }

        private void OnTestWait()
        {
            Invoke("OnWaitTestWait", 5);
        }

        private void OnWaitTestWait()
        {
            WaitManager.Call(this);
        }
    }
}