#if INK

using UnityEngine;

namespace Pearl.Examples.INK
{
    public class TextInterface : MonoBehaviour
    {
        public StoryIndex storyIndex;
        public TextManager textManager;


        void Start()
        {
            textManager.OnFinishWriteText.AddListener(FinishWriteText);
            DialogsManager.CreateDialog(storyIndex, "test", SetText, null, null);
            DialogsManager.ReadNextText("test");
        }

        public void SetText(TextStruct textStruct)
        {
            textManager.SetText(textStruct.title + ": " + textStruct.text);
        }

        private void FinishWriteText()
        {
            DialogsManager.ReadNextText("test");
        }

    }
}

#endif
