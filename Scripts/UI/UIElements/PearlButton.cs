using Pearl.Audio;
using Pearl.ClockManager;
using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pearl.UI
{
    [RequireComponent(typeof(FocusLayerElement))]
    public class PearlButton : Button, IPearlSelectable, IFocusLayer
    {
        #region Events
        public event Action OnSelected;
        public event Action OnDeselected;
        public event Action OnHighlighted;
        public event Action OnNotHighlighted;
        public event Action OnPressed;
        public event Action OnUp;
        #endregion

        #region Public Field
        public bool useDoubleClick;

        public bool useBackSound;
        public bool useSound;
        public bool useSoundInPause;

        public bool useDelayForClick;
        public float delayForClick;
        public GameObject fillObject;
        #endregion

        #region Private Field
        private const float timeForDoubleSelected = 0.2f;
        private bool _isSelectable = false;
        private Timer _timerForClick;
        private float _auxForDoubleClick = 0;
        private bool _correctClick;

        private IFill fillManager;
        #endregion

        #region Propierty
        public bool IsSelectable { get { return _isSelectable; } }
        #endregion

        #region Unity Callbacks
        protected override void Start()
        {
            base.Start();

            if (fillObject != null)
            {
                fillManager = fillObject.GetComponent<IFill>();
            }

            fillManager?.Fill(0);
        }

        protected void Update()
        {
            if (_timerForClick != null)
            {
                fillManager?.Fill(_timerForClick.TimeInPercent);

                if (_timerForClick.IsFinish())
                {
                    _correctClick = true;
                    _timerForClick = null;
                }
            }
        }
        #endregion

        #region Public Methods
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            fillManager?.Fill(0);
            _timerForClick = null;
            OnUp?.Invoke();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_correctClick && interactable)
            {
                if (useDoubleClick && !useDelayForClick)
                {
                    float delta = TimeExtend.GetTime(TimeType.Unscaled) - _auxForDoubleClick;
                    if (delta < timeForDoubleSelected)
                    {
                        EffectiveOnPointerClick(eventData);
                    }
                    else
                    {
                        _auxForDoubleClick = TimeExtend.GetTime(TimeType.Unscaled);
                    }
                }
                else
                {
                    EffectiveOnPointerClick(eventData);
                }
            }
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            Sound(useBackSound ? UIAudioStateEnum.OnBack : UIAudioStateEnum.OnClick);

            base.OnSubmit(eventData);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            OnHighlighted?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            _correctClick = !useDelayForClick || useDoubleClick;
            if (!_correctClick)
            {
                fillManager?.Fill(0);
                _timerForClick = new Timer(delayForClick, true, TimeType.Unscaled);
            }

            base.OnPointerDown(eventData);

            OnPressed?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            OnNotHighlighted?.Invoke();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            _isSelectable = true;
            OnSelected?.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            _isSelectable = false;
            OnDeselected?.Invoke();
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

        public static void CallEvent(string ev)
        {
            PearlEventsManager.CallEvent(ev);
        }

        public static void CallTriggerEvent(string ev)
        {
            PearlEventsManager.CallTrigger(ev);
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

        private void EffectiveOnPointerClick(PointerEventData eventData)
        {
            Sound(useBackSound ? UIAudioStateEnum.OnBack : UIAudioStateEnum.OnClick);

            base.OnPointerClick(eventData);
        }
        #endregion
    }
}
