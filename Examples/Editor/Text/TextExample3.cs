using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.TextExamples
{
    public class TextExample3 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            PearlEventsManager.AddAction("wait", Wait);
        }

        // Update is called once per frame
        void Wait()
        {
            WaitManager.Call(this);
        }
    }
}
