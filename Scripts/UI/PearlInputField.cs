using Pearl.Audio;
using Pearl.Input;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pearl.UI
{
    [RequireComponent(typeof(FocusLayerElement))]
    public class PearlInputField : TMP_InputField, IPearlSelectable, IFocusLayer
    {
        #region Events
        public event Action OnSelected;
        public event Action OnDeselected;
        public event Action OnHighlighted;
        public event Action OnNotHighlighted;
        public event Action OnUp;
        public event Action OnPressed;
        #endregion

        #region Public Field
        public ReaderNumericInput axisUtility = null;
        public string nextTextButton = "NextText";
        public bool clearOnDisactive = true;
        public bool isVector = false;
        public SemiAxis2DEnum nextAxis = SemiAxis2DEnum.Down;

        public bool useAutoSizeFont;
        public float minSizeFont = 2f;

        public bool useSound;
        public bool useSoundInPause;
        #endregion

        #region Private Fields
        private bool _isSelectable = false;
        private const float _minTime = 0.2f;
        private Selectable _nextSelectable;
        private Navigation.Mode _oldMode;
        private bool _isSelected;
        #endregion

        #region Property
        public bool IsSelectable { get { return _isSelectable; } }
        #endregion

        #region UnityCallbacks
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            axisUtility.MinTime = _minTime;

            onSelect.AddListener(Active);
            onDeselect.AddListener(Deactive);

            if (useAutoSizeFont)
            {
                lineType = LineType.MultiLineSubmit;
                lineLimit = 1;
                textComponent.enableAutoSizing = true;
                textComponent.fontSizeMin = minSizeFont;
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            var textAreaTransform = transform.GetChildInHierarchy("Text Area");
            var textTransform = transform.GetChildInHierarchy("Text");
            var placeholderTransform = transform.GetChildInHierarchy("Placeholder");

            if (textAreaTransform != null && textTransform != null && placeholderTransform != null)
            {
                textViewport = textAreaTransform.GetComponent<RectTransform>();
                textComponent = textTransform.GetComponent<TMP_Text>();
                placeholder = placeholderTransform.GetComponent<TMP_Text>();
            }
        }
#endif

        protected override void OnDisable()
        {
            base.OnDisable();

            if (clearOnDisactive)
            {
                text = "";
            }

            ResetNavigation();
        }

        private void Update()
        {
            if (_isSelected && axisUtility != null)
            {
                if (isVector)
                {
                    axisUtility.GetValue(NavigateUpdateVector, true);
                }
                else
                {
                    axisUtility.GetValue(NavigateUpdateFloat, true);
                }
            }
        }
        #endregion

        #region Public Methods
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            _isSelectable = true;
            OnSelected?.Invoke();
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            OnUp?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            OnPressed?.Invoke();
        }


        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            _isSelectable = false;
            OnDeselected?.Invoke();
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnHighlighted?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnNotHighlighted?.Invoke();
        }
        #endregion

        #region IFocusLayer
        public void SetFocusLayer(bool isRightLayer)
        {
            enabled = isRightLayer;
            interactable = isRightLayer;
        }
        #endregion

        #region Private Methods
        private void ResetDeltaTime()
        {
            axisUtility?.ResetDeltaTime();
        }

        private void NavigateUpdateFloat(float input)
        {
            var newSelectable = input > 0 ? navigation.selectOnUp : navigation.selectOnDown;

            Navigate(newSelectable);
        }

        private void NavigateUpdateVector(Vector2 input)
        {
            var newSelectable = input.y > 0 ? navigation.selectOnUp :
                (input.y < 0 ? navigation.selectOnDown :
                (input.x > 0 ? navigation.selectOnRight : navigation.selectOnLeft));

            Navigate(newSelectable);

        }

        // Update is called once per frame
        private void Active(string name)
        {
            InputManager.PerformedHandle(nextTextButton, NextText, ActionEvent.Add);
            ResetDeltaTime();
            _isSelected = true;
        }

        private void Deactive(string name)
        {
            InputManager.PerformedHandle(nextTextButton, NextText, ActionEvent.Remove);
            _isSelected = false;
        }

        private void Navigate(Selectable newSelectable)
        {
            var nav = newSelectable.navigation;
            _nextSelectable = newSelectable;
            _oldMode = nav.mode;
            nav.mode = Navigation.Mode.None;
            newSelectable.navigation = nav;

            if (newSelectable != null && newSelectable.TryGetComponent<PearlInputField>(out var inputFieldManager))
            {
                inputFieldManager.ResetDeltaTime();
            }

            if (newSelectable != null && newSelectable.isActiveAndEnabled && newSelectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                Sound(UIAudioStateEnum.OnScroll);
            }

            FocusManager.SetFocus(newSelectable, true);

            PearlInvoke.WaitForMethod(_minTime, ResetNavigation, TimeType.Unscaled);

        }

        private void ResetNavigation()
        {
            if (_nextSelectable == null)
            {
                return;
            }

            var nav = _nextSelectable.navigation;
            nav.mode = _oldMode;
            _nextSelectable.navigation = nav;

            _nextSelectable = null;
        }

        private void NextText()
        {
            var newSelectable = nextAxis == SemiAxis2DEnum.Down ? navigation.selectOnDown :
                (nextAxis == SemiAxis2DEnum.Right ? navigation.selectOnRight :
                (nextAxis == SemiAxis2DEnum.Up ? navigation.selectOnUp : navigation.selectOnLeft));

            if (newSelectable != null && newSelectable.isActiveAndEnabled && newSelectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                Sound(UIAudioStateEnum.OnScroll);
            }

            FocusManager.SetFocus(newSelectable, true);
        }

        private void Sound(UIAudioStateEnum stateEnum)
        {
            if (Application.isPlaying && useSound && interactable)
            {
                AudioUI.PlayAudioSound(stateEnum, useSoundInPause);
            }
        }
        #endregion
    }
}
