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
        private AudioClip audioFeedbck = null;
        [SerializeField]
        private UnityEvent unityEvent = new();
        #endregion

        #region Private fields
        private bool _useInput;
        private bool _isRightLayer = true;
        #endregion

        #region Property
        public string Input { get { return inputEvent; } set { inputEvent = value; } }
        public AudioClip Audio { get { return audioFeedbck; } set { audioFeedbck = value; } }
        #endregion


        #region UnityCallbacks
        // Start is called before the first frame update
        void Start()
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
                InputManager.PerformedHandle(inputEvent, InvokeAction, ActionEvent.Add, StateButton.Down);
            }
        }

        private void RemoveImput()
        {
            if (InputManager.Input && _useInput)
            {
                _useInput = false;
                InputManager.PerformedHandle(inputEvent, InvokeAction, ActionEvent.Remove, StateButton.Down);
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
