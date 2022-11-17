using UnityEngine;

namespace Pearl.Examples.Debug
{
    public class DebugTest : MonoBehaviour
    {
        public TMPro.TMP_Text textContent;


        void Start()
        {
            var logger = UnityEngine.Debug.unityLogger;
        }

        // Update is called once per frame
        void Update()
        {
            UnityEngine.Debug.Log("CIAO");
        }
    }

}