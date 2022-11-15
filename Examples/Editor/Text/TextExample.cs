using Pearl.Events;
using UnityEngine;

namespace Pearl.Examples.TextExamples
{
    public class TextExample : MonoBehaviour
    {
        public TextManager textManager;
        [TextArea]
        public string text;

        // Start is called before the first frame update
        void Start()
        {
            TextManager.SetVariableString("enemy", "Romualdo");
            textManager.SetLocalVariableString("pg", "Ettore");
            textManager.SetLocalVariableString("color", "blue");
            textManager.SetLocalVariableString("a", "color");
            TextManager.SetVariableString("b", "22");

            textManager.OnEvent += Method1;
            textManager.OnFinishWriteText.AddListener(Method2);
            textManager.OnStartWriteText.AddListener(Method3);

            PearlEventsManager.AddAction("test", GlobalMethod);
            PearlEventsManager.AddAction("wait", WaitMethod);

            Invoke(nameof(FillText), 1f);
        }

        void FillText()
        {
            textManager.SetText(text);
        }

        public void WaitMethod()
        {
            WaitManager.Call(this);
        }

        public void GlobalMethod()
        {
            Debug.LogManager.Log("Test");
        }

        public void Method1(string name, string value)
        {
            Debug.LogManager.Log(name + "  " + value);
            Invoke("Wait", 1);
        }

        public void Wait()
        {
            WaitManager.Call(this);
        }

        public void Method2()
        {
            Debug.LogManager.Log("Finish");
        }

        public void Method3()
        {
            Debug.LogManager.Log("Start");
        }

    }
}

