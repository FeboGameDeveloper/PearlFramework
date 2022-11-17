using Pearl.Audio;
using Pearl.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl.UI
{
    public class InputUIElement : MonoBehaviour, IFocusLayer
    {
        #region Inspector
        [SerializeField]
        private string inputEvent = null;
        [SerializeField]
        private StateButton stateButton = StateButton.Down;
        [SerializeField]
        private AudioClip audioFeedbck = null;
        [SerializeField]
        private UnityEvent unityEvent = new();
        #endregion

        #region Private fields
        private bool _useInput;
        private bool _isRightLayer = true;
        #endregion

        #region Property
        public string InputEvent { get { return inputEvent; } set { inputEvent = value; } }
        public AudioClip AudioFeedback { get { return audioFeedbck; } set { audioFeedbck = value; } }
        public StateButton StateButton { get { return stateButton; } set { stateButton = value; } }
        #endregion

        #region UnityCallbacks
        // Start is called before the first frame update
        protected void Start()
        {
            if (TryGetComponent<FocusLayerElement>(out var element))
            {
                _isRightLayer = element.IsRightLayer();
            }

            if (_isRightLayer)
            {
                AddInput();
            }
        }

        protected void OnDisable()
        {
            RemoveImput();
        }
        #endregion

        #region Public Methods
        public void Setting(string inputEvent, StateButton stateButton, AudioClip audioFeedbck, UnityAction unityAction)
        {
            this.inputEvent = inputEvent;
            this.stateButton = stateButton;
            this.audioFeedbck = audioFeedbck;
            AddAction(unityAction);
        }

        public void InvokeAction()
        {
            unityEvent.Invoke();
            if (audioFeedbck != null)
            {
                AudioManager.Play2DAudio(audioFeedbck, ChannelDeafultEnum.Effects, false);
            }
        }

        public void AddAction(UnityAction unityAction)
        {
            unityEvent.AddListener(unityAction);
        }

        public void RemoveAction(UnityAction unityAction)
        {
            unityEvent.RemoveListener(unityAction);
        }

        private void AddInput()
        {
            if (InputManager.Input && !_useInput)
            {
                _useInput = true;
                InputManager.PerformedHandle(inputEvent, InvokeAction, ActionEvent.Add, stateButton);
            }
        }

        private void RemoveImput()
        {
            if (InputManager.Input && _useInput)
            {
                _useInput = false;
                InputManager.PerformedHandle(inputEvent, InvokeAction, ActionEvent.Remove, stateButton);
            }
        }

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
    }
}
