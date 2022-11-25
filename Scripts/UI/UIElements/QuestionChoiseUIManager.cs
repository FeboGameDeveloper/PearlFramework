using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace Pearl.UI
{
    [Serializable]
    public struct ButtonInfo
    {
        public TrackingUnityEvent unityEvent;
        public string text;
        public bool isFirstFocus;
        public bool useInput;
        [ConditionalField("@useInput")]
        public string nameInput;
        [ConditionalField("@useInput")]
        public string map;
        [ConditionalField("@useInput")]
        public AudioClip clip;

        public ButtonInfo(string text, TrackingUnityEvent unityEvent, bool isFirstFocus = false, string nameInput = default, AudioClip clip = null, string map = default)
        {
            this.unityEvent = unityEvent;
            this.text = text;
            this.isFirstFocus = isFirstFocus;

            this.useInput = nameInput != default;
            this.nameInput = nameInput;
            this.map = map;
            this.clip = clip;
        }

        public ButtonInfo(string text, UnityAction unityAction, bool isFirstFocus = false, string nameInput = default, AudioClip clip = null, string map = default)
        {
            this.unityEvent = new TrackingUnityEvent();
            unityEvent?.AddNotPersistantListener(unityAction);
            this.text = text;
            this.isFirstFocus = isFirstFocus;

            this.useInput = nameInput != default;
            this.nameInput = nameInput;
            this.map = map;
            this.clip = clip;
        }

        public ButtonInfo(string text, UnityAction[] unityActions, bool isFirstFocus = false, string nameInput = default, AudioClip clip = null, string map = default)
        {
            this.unityEvent = new TrackingUnityEvent();

            foreach (var action in unityActions)
            {
                unityEvent?.AddNotPersistantListener(action);
            }
            this.text = text;
            this.isFirstFocus = isFirstFocus;

            this.useInput = nameInput != default;
            this.nameInput = nameInput;
            this.map = map;
            this.clip = clip;
        }

        public ButtonInfo(string text, bool isFirstFocus = false, string nameInput = default, AudioClip clip = null, string map = default)
        {
            this.unityEvent = new TrackingUnityEvent();
            this.text = text;
            this.isFirstFocus = isFirstFocus;

            this.useInput = nameInput != default;
            this.nameInput = nameInput;
            this.map = map;
            this.clip = clip;
        }
    }

    [Serializable]
    public struct QuestionInfo
    {
        public string title;
        public string text;
        public bool localizeTest;
        public ButtonInfo[] buttonsInfo;

        public QuestionInfo(string title, string text, bool localizeText, params ButtonInfo[] buttonsInfo)
        {
            this.title = title;
            this.text = text;
            this.buttonsInfo = buttonsInfo;
            this.localizeTest = localizeText;
        }
    }

    public enum FinishQustionEnum { Destroy, Disactive, Null }

    public class QuestionChoiseUIManager : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        private IntegerTansformDictionary dialogsIndexDictonary = null;
        [SerializeField]
        private TextManager titleContainer = null;
        [SerializeField]
        private TextManager descriptionContainer = null;
        #endregion

        #region Private Fields
        private Transform _buttonsContainer;
        private bool _isFirstFocusable = false;
        private FinishQustionEnum finishQustionEnum = FinishQustionEnum.Destroy;
        private string _focusGroup;
        #endregion

        #region Public Methods
        public void Initialize(in QuestionInfo questioInfo, in FinishQustionEnum finishQustionEnum, in string focusGroup)
        {
            Initialize(questioInfo.title, questioInfo.text, questioInfo.localizeTest, finishQustionEnum, focusGroup, questioInfo.buttonsInfo);
        }

        public void Initialize(in string titleText, in string descriptionText, in bool localizeText, in FinishQustionEnum finishQustionEnum, in string focusGroup, params ButtonInfo[] buttonInfos)
        {
            _focusGroup = focusGroup;
            this.finishQustionEnum = finishQustionEnum;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();

            var focusManagers = transform.GetComponentsInHierarchy<FocusLayerElement>();
            var layerFocus = FocusManager.CurrentLayerFocus + 1;
            foreach (var element in focusManagers)
            {
                element.SetLayerFocus(layerFocus, true);
            }

            _isFirstFocusable = false;

            if (buttonInfos == null || dialogsIndexDictonary == null)
            {
                return;
            }

            if (titleContainer != null)
            {
                titleContainer.SetText(titleText);
            }

            if (descriptionContainer != null)
            {
#if LOCALIZATION
                descriptionContainer.Localize = localizeText;
#endif
                descriptionContainer.SetText(descriptionText);
            }

            int length = buttonInfos.Length;

            if (!dialogsIndexDictonary.TryGetValue(length, out _buttonsContainer))
            {
                return;
            }

            foreach (var pair in dialogsIndexDictonary)
            {
                pair.Value.gameObject.SetActive(pair.Key == length);
            }

            for (int index = 0; index < buttonInfos.Length; index++)
            {
                if (index >= _buttonsContainer.childCount)
                {
                    continue;
                }

                Transform child = _buttonsContainer.GetChild(index);

                if (child == null)
                {
                    continue;
                }

                Button button = child.GetComponent<Button>();
                TextManager textManager = child.GetComponentInChildren<TextManager>();

                if (button == null || textManager == null)
                {
                    continue;
                }

                ButtonInfo buttonInfo = buttonInfos[index];

                var auxEvent = buttonInfo.unityEvent;
                auxEvent.AddInHeadNotPersistantListener(DisableObject);

                ButtonClickedEvent buttonClickEvent = new();

                var auxList = auxEvent.UnityActions;
                foreach (var unityEvent in auxList)
                {
                    buttonClickEvent.AddListener(unityEvent);
                }
                button.onClick = buttonClickEvent;
                textManager.SetText(buttonInfo.text);

                if (buttonInfo.useInput && child.TryAddOnlyOneComponent<InputUIElement>(out var inputUIElement))
                {
                    inputUIElement.Setting(buttonInfo.nameInput, StateButton.Up, buttonInfo.clip,
                        () => buttonClickEvent.Invoke());
                }


                if (buttonInfo.isFirstFocus || length == 1 || (!_isFirstFocusable && index == length - 1))
                {
                    FocusManager.SetFocus(button.gameObject, true);
                    _isFirstFocusable = true;
                }
            }
        }
        #endregion

        #region Private Methods
        private void DisableObject()
        {
            foreach (Transform child in _buttonsContainer)
            {
                if (child == null)
                {
                    continue;
                }

                if (child.TryGetComponent<Button>(out Button button))
                {
                    button.onClick.RemoveListener(DisableObject);
                }
            }

            if (finishQustionEnum == FinishQustionEnum.Destroy)
            {
                GameObjectExtend.DestroyExtend(gameObject);
            }
            else if (finishQustionEnum == FinishQustionEnum.Disactive)
            {
                gameObject.SetActive(false);
            }

            if (_focusGroup != null)
            {
                FocusManager.SetFocus(_focusGroup, true);
            }
        }
        #endregion
    }
}