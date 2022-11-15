using Pearl.Debug;
using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.EventExamples
{
    public class CallTest : MonoBehaviour
    {
        // Start is called before the first frame update
        protected void Awake()
        {
            PearlEventsManager.CallEvent("testTrigger", PearlEventType.Trigger, "testTrigger");
        }

        protected void Start()
        {
            PearlEventsManager.CallEvent("test", PearlEventType.Normal, "test");
            PearlEventsManager.CallEventWithReturn("testWait", OnFinishtestWait);
        }

        private void OnFinishtestWait()
        {
            LogManager.Log("Finish wait");
        }
    }
}