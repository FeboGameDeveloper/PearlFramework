using Pearl.Editor;
using Pearl.Events;
using Pearl.Input;
using System;
using UnityEngine;

namespace Pearl.UI
{
    public enum InputDataType { Image, Text }

    [Serializable]
    public class ElementForInputData
    {
        public string inputEvent = string.Empty;
        public InputDataType inputDataType = InputDataType.Image;
        public string IDText;


        public ElementForInputData(string inputEvent, InputDataType inputDataType)
        {
            this.inputEvent = inputEvent;
            this.inputDataType = inputDataType;
        }

        public ElementForInputData(string inputEvent, InputDataType inputDataType, string IDText) : this(inputEvent, inputDataType)
        {
            this.IDText = IDText;
        }
    }


    public class ElementForInput : PearlBehaviour
    {
        #region Inspector
        [SerializeField]
        protected string inputEvent = string.Empty;
        [SerializeField]
        private InputDataType inputDataType = InputDataType.Image;
        [SerializeField, ConditionalField("@inputDataType == Image")]
        protected GameObject imageComponent = null;
        [SerializeField, ConditionalField("@inputDataType == Text")]
        protected TextManager textComponent = null;

        [SerializeField]
        protected bool useLabel = false;

        [SerializeField, ConditionalField("@useLabel")]
        protected TextManager labelContainer = null;
        [SerializeField, ConditionalField("@useLabel")]
        protected string IDText = null;
        #endregion

        #region Private field
        private ButtonImageForCommandScriptableObject[] buttonImageForInputScriptableObjects;
        private ButtonTextForCommandScriptableObject[] buttonTextForCommandScrptableObjecta;

        private const string buttonImageForInputSting = "ButtonImageForInput";
        private const string buttonTextForInputSting = "ButtonTextForInput";

        protected SpriteManager spriteManager;
        #endregion

        #region UnityCallback
        protected override void Awake()
        {
            base.Awake();

            if (inputDataType == InputDataType.Image && imageComponent)
            {
                buttonImageForInputScriptableObjects = AssetManager.LoadAsset<ButtonImageForCommandScriptableObject[]>(buttonImageForInputSting);
                spriteManager = new SpriteManager(imageComponent);
            }
            if (inputDataType == InputDataType.Text && textComponent)
            {
                buttonTextForCommandScrptableObjecta = AssetManager.LoadAsset<ButtonTextForCommandScriptableObject[]>(buttonTextForInputSting);
            }

            PearlEventsManager.AddAction<InputDeviceEnum, int>(ConstantStrings.ChangeInputDevice, SetType);
        }

        protected override void Start()
        {
            base.Start();

            SetType();
        }

        private void Reset()
        {
            imageComponent = gameObject;
            textComponent = GetComponent<TextManager>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PearlEventsManager.RemoveAction<InputDeviceEnum, int>(ConstantStrings.ChangeInputDevice, SetType);
        }
        #endregion

        #region Public Methods
        public void UpdateElement(string inputEvent)
        {
            this.inputEvent = inputEvent;
            SetType();
        }

        public void UpdateElement(string inputEvent, GameObject imageComponent)
        {
            this.imageComponent = imageComponent;
            this.inputDataType = InputDataType.Image;
            UpdateElement(inputEvent);
        }

        public void UpdateElement(string inputEvent, string IDText)
        {
            this.IDText = IDText;
            this.inputDataType = InputDataType.Text;
            UpdateElement(inputEvent);
        }
        #endregion

        #region Private Methods
        private void SetType(InputDeviceEnum currentInputDevice, int player)
        {
            SetType();
        }

        private void SetType()
        {
            if (labelContainer != null && !string.IsNullOrEmpty(IDText))
            {
#if LOCALIZATION
                string text = localizeLabel && tableName != null ? LocalizationManager.Translate(tableName, IDText) : IDText;
#else
                string text = IDText;
#endif
                labelContainer.SetText(text);
            }

            if (inputDataType == InputDataType.Image)
            {
                if (spriteManager != null && buttonImageForInputScriptableObjects != null)
                {
                    foreach (var scriptableObjects in buttonImageForInputScriptableObjects)
                    {
                        Sprite sprite = scriptableObjects.GetSprite(inputEvent);
                        if (sprite != null)
                        {
                            spriteManager.SetSprite(sprite);
                            break;
                        }
                    }
                }
            }
            else if (inputDataType == InputDataType.Text)
            {
                if (buttonTextForCommandScrptableObjecta != null)
                {
                    foreach (var scriptableObjects in buttonTextForCommandScrptableObjecta)
                    {
                        string text = scriptableObjects.GetText(inputEvent);
                        if (text != null)
                        {
                            textComponent.SetText(text);
                            break;
                        }
                    }
                }
            }
        }
        #endregion
    }
}

