using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.WaitExamples
{
    public class Call : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            WaitManager.Wait(typeof(Waitxamples1), Wait);
            WaitManager.Wait(typeof(Waitxamples2), Wait);
            Invoke("Caller", 2f);
        }

        void Caller()
        {
            WaitManager.Call<float>(typeof(Call), 2f);
            WaitManager.Call<float>(typeof(Call), 4f);
        }

        void Wait()
        {
            UnityEngine.Debug.Log("Finish wait");
        }
    }
}

