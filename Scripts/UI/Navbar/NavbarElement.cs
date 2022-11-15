using Pearl.Input;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.UI.Button;

namespace Pearl.UI
{
    [Serializable]
    public class NavbarInfoElement : ElementForInputData
    {
        public ButtonClickedEvent unityEvent = null;

        public NavbarInfoElement(ButtonClickedEvent unityEvent, string inputEvent, InputDataType inputDataType) : base(inputEvent, inputDataType)
        {
            this.unityEvent = unityEvent;
        }

        public NavbarInfoElement(ButtonClickedEvent unityEvent, string inputEvent, InputDataType inputDataType, string IDText) : base(inputEvent, inputDataType, IDText)
        {
            this.unityEvent = unityEvent;
        }
    }

    [RequireComponent(typeof(PearlButton))]
    public class NavbarElement : ElementForInput
    {
        #region Inspector fields
        [SerializeField]
        private PearlButton pearlButton = null;
        #endregion

        #region Private Fields
        private bool _useInput;
        private bool _isRightLayer = true;
        #endregion

        #region Unity Callbacks
        protected override void Start()
        {
            base.Start();

            UpdateElement(inputEvent, IDText);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            RemoveImput();
        }

        protected override void OnEnableAfterStart()
        {
            base.OnEnableAfterStart();

            if (TryGetComponent<FocusLayerElement>(out var element))
            {
                _isRightLayer = element.IsRightLayer();
            }

            if (_isRightLayer)
            {
                AddInput();
            }
        }

        protected void Reset()
        {
            pearlButton = GetComponent<PearlButton>();
        }
        #endregion

        #region Public Methods
        public void UpdateElement(NavbarInfoElement infoNavbarElement)
        {
            if (pearlButton)
            {
                pearlButton.onClick = infoNavbarElement.unityEvent;
            }

            UpdateElement(infoNavbarElement.inputEvent, infoNavbarElement.IDText);
        }
        #endregion

        #region IFocusLayer
        public void SetFocusLayer(bool isRightLayer)
        {
            _isRightLayer = isRightLayer;
            if (_isRightLayer)
            {
                AddInput();
            }
            else
            {
                RemoveImput();
            }
        }
        #endregion

        #region Private Methods
        private void InvokeButton()
        {
            if (pearlButton)
            {
                PointerEventData data = new(EventSystem.current)
                {
                    button = PointerEventData.InputButton.Left
                };
                pearlButton.OnPointerUp(data);
                pearlButton.OnPointerClick(data);
            }
        }

        private void OnDown()
        {
            if (pearlButton)
            {
                PointerEventData data = new(EventSystem.current)
                {
                    button = PointerEventData.InputButton.Left
                };
                pearlButton.OnPointerDown(data);
            }
        }

        private void AddInput()
        {
            if (InputManager.Input && !_useInput)
            {
                _useInput = true;
                InputManager.Input.PerformedHandle(inputEvent, OnDown, InvokeButton, ActionEvent.Add);
            }
        }

        private void RemoveImput()
        {
            if (InputManager.Input && _useInput)
            {
                _useInput = false;
                InputManager.Input.PerformedHandle(inputEvent, OnDown, InvokeButton, ActionEvent.Remove);
            }
        }
        #endregion
    }
}
