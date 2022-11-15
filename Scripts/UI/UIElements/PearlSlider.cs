using Pearl.Audio;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pearl.UI
{
    [RequireComponent(typeof(FocusLayerElement))]
    public class PearlSlider : Slider, IPearlSelectable, IFocusLayer
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
        public bool useSound;
        public bool useSoundInPause;
        #endregion

        #region Private Fields
        private bool _isSelectable = false;
        #endregion

        #region Property
        public bool IsSelectable { get { return _isSelectable; } }
        #endregion

        #region UnityCallbacks
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();

            var handleTransform = transform.GetChildInHierarchy("Handle");
            var fillTransform = transform.GetChildInHierarchy("Handle");

            if (handleTransform != null && fillTransform != null)
            {
                targetGraphic = handleTransform.GetComponent<Image>();
                fillRect = fillTransform.GetComponent<RectTransform>();
                handleRect = handleTransform.GetComponent<RectTransform>();
            }
        }
#endif

        #endregion

        #region Public Methods
        public void AddValue(float input, bool sendCallback = true)
        {
            Set(m_Value + input, sendCallback);
        }

        public void SetValue(float input, bool sendCallback = true)
        {
            Set(input, sendCallback);
        }

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

        public override void OnMove(AxisEventData eventData)
        {
            Selectable selectable = null;
            switch (eventData.moveDir)
            {
                case MoveDirection.Right:
                    selectable = base.FindSelectableOnRight();
                    break;

                case MoveDirection.Up:
                    selectable = base.FindSelectableOnUp();
                    break;

                case MoveDirection.Left:
                    selectable = base.FindSelectableOnLeft();
                    break;

                case MoveDirection.Down:
                    selectable = base.FindSelectableOnDown();
                    break;
            }

            if (selectable != null && selectable.isActiveAndEnabled && selectable.GetComponent<RectTransform>().IsVisibleFrom())
            {
                Sound(UIAudioStateEnum.OnScroll);
            }

            base.OnMove(eventData);
        }
        #endregion

        #region IFocusLayer
        public void SetFocusLayer(bool isRightLayer)
        {
            enabled = isRightLayer;
            interactable = isRightLayer;
        }
        #endregion

        #region Private Fields
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
