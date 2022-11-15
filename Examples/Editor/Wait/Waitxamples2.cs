using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.WaitExamples
{
    public class Waitxamples2 : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            WaitManager.Wait<float>(typeof(Call), Test);
        }

        // Update is called once per frame
        void Test(float a)
        {
            UnityEngine.Debug.Log(a);
            Invoke("Call", 2);
        }

        void Call()
        {
            WaitManager.Call(typeof(Waitxamples2));
        }
    }
}
