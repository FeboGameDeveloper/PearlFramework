using Pearl.Debug;
using Pearl.Events;
using Pearl.Input;
using UnityEngine;

namespace Pearl
{
    public class TriggerInputManagment : MonoBehaviour
    {
        [SerializeField]
        private InputInfo inputInfo = default;
        [SerializeField]
        private bool initAtStart = false;
        [SerializeField]
        private bool isWait = false;
        [SerializeField]
        private bool useHelpUIEvent = true;
        [SerializeField]
        [ConditionalField("@useHelpUIEvent")]
        private string helpEvent = string.Empty;
        [SerializeField]
        private GameObjectEvent action = null;

        private GameObject _colliderObj;
        private bool _isEnable = true;

        private bool _isEnableSource;
        private int _methodWaitForCount;

        private void Start()
        {
            if (initAtStart)
            {
                ChangeInput(ActionEvent.Add);
            }
        }

        private void OnDestroy()
        {
            ChangeInput(ActionEvent.Remove);
        }


        private void OnFinishWait()
        {
            _isEnable = true;

            if (_isEnableSource)
            {
                NearUsableObj(_colliderObj);
            }
        }

        private void Use()
        {
            if (isWait)
            {
                var list = action?.GetListComponents();
                _methodWaitForCount = list.Count;

                if (list.IsAlmostSpecificCount())
                {
                    FarUsableObj(_colliderObj);
                    _isEnable = false;

                    foreach (var element in list)
                    {
                        WaitManager.Wait(element, OnFinishWait);
                    }

                    action?.Invoke(_colliderObj);

                }
                else
                {
                    LogManager.LogWarning("There isn't one action with IWaitAction interface");
                }
            }
            else
            {
                action?.Invoke(_colliderObj);
            }
        }

        public void OnNearUsableObj(GameObject colliderObj)
        {
            _isEnableSource = true;
            NearUsableObj(colliderObj);
        }

        public void OnFarUsableObj(GameObject colliderObj)
        {
            _isEnableSource = false;
            FarUsableObj(colliderObj);
        }

        private void NearUsableObj(GameObject colliderObj)
        {
            if (_isEnable)
            {
                this._colliderObj = colliderObj;
                UseHelpInputUI(true);
                ChangeInput(ActionEvent.Add);
            }
        }

        private void FarUsableObj(GameObject colliderObj)
        {
            if (_isEnable)
            {
                UseHelpInputUI(false);
                ChangeInput(ActionEvent.Remove);
            }
        }

        private void ChangeInput(ActionEvent actionEvent)
        {
            if (InputManager.Input.IsNotNull(out var input))
            {
                input.PerformedHandle(inputInfo, Use, actionEvent);
            }
        }

        private void UseHelpInputUI(bool value)
        {
            if (useHelpUIEvent)
            {
                PearlEventsManager.CallEvent(helpEvent, PearlEventType.Normal, value);
            }
        }
    }
}
