using Pearl.Audio;
using Pearl.UI;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public class GenerateQuestionarie : PearlBehaviour, ISingleton
    {
        #region Inspector
        [SerializeField]
        private GameObject questionariePrefab = null;
        #endregion

        #region GenerateQuestionarie
        public static void GenerateQuestionarieUI(QuestionInfo questionInfo, string storageGroup)
        {
            if (Singleton<GenerateQuestionarie>.GetIstance(out var result, CreateSingletonEnum.CreateStatic))
            {
                result.GenerateQuestionarieUIInternal(questionInfo, storageGroup);
            }
        }

        public static void GenerateChoiseUI(UnityAction yesEvent, UnityAction noEvent, in bool localizeText, in string focusGroup, in string description = "are you sure?")
        {
            ButtonInfo buttonInfo1 = new("no", noEvent, true, "Back", AudioUI.Get(UIAudioStateEnum.OnClick));
            ButtonInfo buttonInfo2 = new("yes", yesEvent, false);

            QuestionInfo questionInfo = new("Attention", description, localizeText, buttonInfo1, buttonInfo2);

            GenerateQuestionarieUI(questionInfo, focusGroup);
        }

        public static void GenerateChoiseUI(UnityAction yesEvent, in bool localizeText, in string focusGroup, in string description = "are you sure?")
        {
            static void noEvent() { }
            GenerateChoiseUI(yesEvent, noEvent, localizeText, focusGroup, description);
        }

        public static void GenerateQuestionarieUI(in string description, in bool localizeText, in string buttonText, in string focusGroup)
        {
            ButtonInfo buttonInfo = new(buttonText, true);
            GenerateQuestionarieUI(description, localizeText, focusGroup, buttonInfo);
        }

        public static void GenerateQuestionarieUI(in string description, in bool localizeText, in string focusGroup, params ButtonInfo[] buttonsInfo)
        {
            GenerateQuestionarieUI("Attention", description, localizeText, focusGroup, buttonsInfo);
        }

        public static void GenerateQuestionarieUI(in string title, in string description, in bool localizeText, in string focusGroup, params ButtonInfo[] buttonsInfo)
        {
            QuestionInfo questionInfo = new(title, description, localizeText, buttonsInfo);
            GenerateQuestionarieUI(questionInfo, focusGroup);
        }

        public static void GenerateQuestionarieUI(in string title, in string description, in bool localizeText, in string buttonText, string focusGroup)
        {
            ButtonInfo buttonInfo = new(buttonText, true);
            GenerateQuestionarieUI(title, description, localizeText, focusGroup, buttonInfo);
        }
        #endregion

        #region Private
        private void GenerateQuestionarieUIInternal(QuestionInfo questionInfo, in string focusGroup)
        {
            GameObjectExtend.CreateUIlement<QuestionChoiseUIManager>(questionariePrefab, out QuestionChoiseUIManager questionManager, canvasTipology: CanvasTipology.Debug);

            if (questionManager)
            {
                questionManager.Initialize(questionInfo, FinishQustionEnum.Destroy, focusGroup);
            }
        }
        #endregion
    }
}
