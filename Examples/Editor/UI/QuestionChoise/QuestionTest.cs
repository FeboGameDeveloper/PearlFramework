using Pearl.UI;
using UnityEngine;

namespace Pearl.Examples.UI
{
    public class QuestionTest : MonoBehaviour
    {
        public QuestionChoiseUIManager question;


        // Start is called before the first frame update
        public void Click()
        {
            ButtonInfo button1 = new("yes", Yes);
            ButtonInfo button2 = new("no", No);
            QuestionInfo questionInfo = new("Test", "Description", false, button1, button2);

            question.Initialize(questionInfo, FinishQustionEnum.Disactive, "page1");
        }

        // Update is called once per frame
        void Yes()
        {
            UnityEngine.Debug.Log("Yes");
            //FocusManager.SetFocus("page1", false, false);
        }

        void No()
        {
            UnityEngine.Debug.Log("No");
            //FocusManager.SetFocus("page1", false, false);
        }
    }
}
