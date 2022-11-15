using UnityEngine;

namespace Pearl.Examples.SingletonExamples
{
    public class SingletonTest2 : MonoBehaviour
    {
        public void Start()
        {
            SingletonTest1.DebugLog();
        }
    }
}
