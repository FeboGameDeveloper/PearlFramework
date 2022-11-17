using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.Text
{
    public class TextExample2 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            PearlEventsManager.AddAction("wait2", Wait);
        }

        // Update is called once per frame
        void Wait()
        {
            WaitManager.Call(this);
        }
    }
}
