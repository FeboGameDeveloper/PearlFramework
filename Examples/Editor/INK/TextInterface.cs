#if INK

using UnityEngine;

namespace Pearl.Examples.INK
{
    public class TextInterface : MonoBehaviour
    {
        public StoryIndex storyIndex;
        public TextManager textManager;

        private int _id;


        void Start()
        {
            textManager.OnFinishWriteText.AddListener(FinishWriteText);
            DialogsManager.CreateDialog(storyIndex, out _id, SetText, null, null);
            DialogsManager.ReadNextText(_id);
        }

        public void SetText(TextStruct textStruct)
        {
            textManager.SetText(textStruct.title + ": " + textStruct.text);
        }

        private void FinishWriteText()
        {
            DialogsManager.ReadNextText(_id);
        }

    }
}

#endif
