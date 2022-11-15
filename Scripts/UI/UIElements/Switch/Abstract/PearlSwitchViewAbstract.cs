using Pearl.Audio;
using Pearl.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    public abstract class PearlSwitchViewAbstract<T> : PearlSwitchViewNative, IContentFiller<T>
    {
        [Serializable]
        public struct OptionsListStruct
        {
            public List<T> optionsList;
        }

        #region Inspector Field
        [SerializeField]
        private ReaderNumericInput axisUtility = null;
        [SerializeField]
        protected bool useInputWhenNotFocus = false;
        [SerializeField]
        protected bool useFiller = false;
        [SerializeField]
        private PearlSelectableManager button = null;
        [SerializeField]
        [ConditionalField("!@useFiller")]
        protected OptionsListStruct optionsListStruct = default;

        [Header("Feedback arrows")]

        [SerializeField]
        private Color colorSelect = default;
        [SerializeField]
        private Color colorDeselect = default;
        [SerializeField]
        private Image rightButtonImage = null;
        [SerializeField]
        private Image leftButtonImage = null;
        [SerializeField]
        private float deltaTimeForSelection = 0.3f;
        #endregion

        #region Private Fields
        protected int _currentIndex = 0;
        private Dictionary<int, T> _dictonaryOptions;
        protected Filler<T> _filler;
        private bool _isSelected;
        private Coroutine _rightButtonDeselect;
        private Coroutine _leftButtonDeselect;
        #endregion

        #region Proprietes
        public T Value
        {
            get
            {
                _dictonaryOptions.TryGetValue(_currentIndex, out T currentValue);
                return currentValue;
            }
        }
        #endregion

        #region Unity CallBacks
        protected virtual void Awake()
        {
            _dictonaryOptions = new Dictionary<int, T>();

            if (button && button.OnSelected != null && button.OnDeselected != null)
            {
                button.OnSelected.AddListener(OnSelect);
                button.OnDeselected.AddListener(OnDeselect);
            }

            if (!useFiller)
            {
                FillContent(optionsListStruct.optionsList);
            }

            if (rightButtonImage != null && rightButtonImage.TryGetComponent<Selectable>(out var selectable))
            {
                var colorStruct = selectable.colors;
                colorStruct.selectedColor = colorSelect;
                colorStruct.highlightedColor = colorSelect;
                colorStruct.pressedColor = colorSelect;
                colorStruct.normalColor = colorDeselect;
                selectable.colors = colorStruct;
            }

            if (leftButtonImage != null && leftButtonImage.TryGetComponent<Selectable>(out selectable))
            {
                var colorStruct = selectable.colors;
                colorStruct.selectedColor = colorSelect;
                colorStruct.highlightedColor = colorSelect;
                colorStruct.pressedColor = colorSelect;
                colorStruct.normalColor = colorDeselect;
                selectable.colors = colorStruct;
            }
        }

        private void Start()
        {
            if (useFiller && _filler != null)
            {
                _filler.Fill(this);
            }

            T currentValue = Value;
            SetContentView(currentValue);
        }

        private void Update()
        {
            if (_isSelected || useInputWhenNotFocus)
            {
                ReadInputAxis();
            }
        }

        private void OnDestroy()
        {
            if (button && button.OnSelected != null && button.OnDeselected != null)
            {
                button.OnSelected.RemoveListener(OnSelect);
                button.OnDeselected.RemoveListener(OnDeselect);
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            if (rightButtonImage && leftButtonImage)
            {
                rightButtonImage.color = colorDeselect;
                leftButtonImage.color = colorDeselect;
            }
        }
        #endregion

        #region Public Methods
        public void ScrollRight()
        {
            ChangeScrollView(1);
        }

        public void ScrollLeft()
        {
            ChangeScrollView(-1);
        }
        #endregion

        #region IContentFiller<T>
        public void FillContent(List<T> content)
        {
            optionsListStruct.optionsList = content;

            if (optionsListStruct.optionsList != null)
            {
                for (int i = 0; i < optionsListStruct.optionsList.Count; i++)
                {
                    _dictonaryOptions.Add(i, optionsListStruct.optionsList[i]);
                }
            }
        }


        public void SetContent(T content)
        {
            if (_dictonaryOptions.TryGetKey(content, out int newIndex))
            {
                ChangeElement(newIndex);
            }
        }
        #endregion

        #region Private Methods
        private void ChangeElement(int newIndex)
        {
            SetIndex(newIndex);

            T currentValue = Value;

            SetContentView(currentValue);
            InvokeEvent(currentValue);
        }

        private void SetIndex(int newIndex)
        {
            _currentIndex = newIndex;
        }

        protected virtual void InvokeEvent(in T currentValue)
        {
        }

        protected void InvokeEventIntern(in string currentValue)
        {
            if (currentValue != null && onValueChanged != null)
            {
                onValueChanged.Invoke(currentValue);
            }
        }

        protected virtual void SetContentView(in T currentValue)
        {
        }

        private void ReadInputAxis()
        {
            if (axisUtility != null)
            {
                axisUtility.GetValue(ReadInputAxis, true);
            }
        }

        private void ReadInputAxis(float valueInput)
        {
            ChangeScrollView(valueInput);
        }

        private IEnumerator DeselectButton(Image imageContainer)
        {
            if (imageContainer)
            {
                yield return new WaitForSecondsRealtime(deltaTimeForSelection);
                imageContainer.color = colorDeselect;
            }
        }

        private void ChangeScrollView(float valueInput)
        {
            if (valueInput > 0 && rightButtonImage)
            {
                rightButtonImage.color = colorSelect;
                if (_rightButtonDeselect != null)
                {
                    StopCoroutine(_rightButtonDeselect);
                }
                _rightButtonDeselect = StartCoroutine(DeselectButton(rightButtonImage));
            }

            if (valueInput < 0 && leftButtonImage)
            {
                leftButtonImage.color = colorSelect;
                if (_leftButtonDeselect != null)
                {
                    StopCoroutine(_leftButtonDeselect);
                }
                _leftButtonDeselect = StartCoroutine(DeselectButton(leftButtonImage));
            }


            if (optionsListStruct.optionsList != null && optionsListStruct.optionsList.Count != 0)
            {
                AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);

                int newIndex = MathfExtend.ChangeInCircle(_currentIndex, MathfExtend.Sign(valueInput), optionsListStruct.optionsList.Count);
                ChangeElement(newIndex);
            }

            FocusManager.SetFocus(button.gameObject, true);
        }

        private void OnSelect()
        {
            _isSelected = true;
        }

        private void OnDeselect()
        {
            _isSelected = false;
        }
        #endregion
    }
}
