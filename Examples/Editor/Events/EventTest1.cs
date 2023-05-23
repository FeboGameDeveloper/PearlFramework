using Pearl.Testing;
using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.EventExamples
{
    public class EventTest1 : MonoBehaviour
    {
        // Start is called before the first frame update
        protected void Awake()
        {
            PearlEventsManager.AddAction<string>("test", OnTest);
            PearlEventsManager.AddAction("testWait", OnTestWait);
        }

        protected void Start()
        {
            PearlEventsManager.AddAction<string>("testTrigger", OnTestTrigger);
        }

        protected void OnDisable()
        {
            PearlEventsManager.RemoveAction<string>("test", OnTest);
            PearlEventsManager.RemoveAction<string>("testTrigger", OnTestTrigger);
        }

        private void OnTest(string s)
        {
            LogManager.Log(s);
        }

        private void OnTestTrigger(string s)
        {
            LogManager.Log(s);
        }

        private void OnTestWait()
        {
            WaitManager.Call(this);
        }
    }
}