using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public class PearlBehaviour : MonoBehaviour
    {
        #region Private Fields
        protected bool _useStart = false;
        protected bool _useAwake = false;
        #endregion

        #region Event
        public event Action StartHandler;
        public event Action DestroyHandler;
        public event Action<PearlBehaviour> DisableHandler;
        public event Action<PearlBehaviour> EnableHandler;
        #endregion

        #region Unity Callbacks
        protected virtual void Awake()
        {
            if (!_useAwake)
            {
                _useAwake = true;
            }
        }

        protected virtual void Start()
        {
            if (!_useStart)
            {
                _useStart = true;
                StartHandler?.Invoke();
                OnEnableAfterStart();
            }
        }

        protected virtual void OnEnable()
        {
            EnableHandler?.Invoke(this);
            if (_useStart)
            {
                OnEnableAfterStart();
            }
        }

        protected virtual void OnDisable()
        {
            DisableHandler?.Invoke(this);
        }

        protected virtual void OnDestroy()
        {
            DestroyHandler?.Invoke();
        }

        protected virtual void OnEnableAfterStart()
        {
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += OnValidatePrivate;
        }

        protected virtual void OnValidateOnlyInPlaying()
        {

        }

        private void OnValidatePrivate()
        {
            UnityEditor.EditorApplication.delayCall -= OnValidatePrivate;

            if (this == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                OnValidateOnlyInPlaying();
            }
        }

#endif

#endregion

        #region Public Methods
        public void ForceStart()
        {
            if (!_useStart)
            {
                Start();
            }
        }

        public void ForceAwake()
        {
            if (!_useAwake)
            {
                Awake();
            }
        }

        #region Event
        public void AddAction(string constantStrings, Action action, DeleteGameObjectEnum onDestroy, bool solo = false)
        {
            PearlEventsManager.AddAction(constantStrings, action, solo);

            if (onDestroy == DeleteGameObjectEnum.Destroy)
            {
                DestroyHandler += () => PearlEventsManager.RemoveAction(constantStrings, action);
            }
            else if (onDestroy == DeleteGameObjectEnum.Disable)
            {
                EnableHandler += (PearlBehaviour @this) => PearlEventsManager.AddAction(constantStrings, action);
                DisableHandler += (PearlBehaviour @this) => PearlEventsManager.RemoveAction(constantStrings, action);
            }
        }

        public void AddAction<T>(string constantStrings, Action<T> action, DeleteGameObjectEnum onDestroy, bool solo = false)
        {
            PearlEventsManager.AddAction(constantStrings, action, solo);

            if (onDestroy == DeleteGameObjectEnum.Destroy)
            {
                DestroyHandler += () => PearlEventsManager.RemoveAction(constantStrings, action);
            }
            else if (onDestroy == DeleteGameObjectEnum.Disable)
            {
                EnableHandler += (PearlBehaviour @this) => PearlEventsManager.AddAction(constantStrings, action);
                DisableHandler += (PearlBehaviour @this) => PearlEventsManager.RemoveAction(constantStrings, action);
            }
        }

        public void AddAction<T, F>(string constantStrings, Action<T, F> action, DeleteGameObjectEnum onDestroy, bool solo = false)
        {
            PearlEventsManager.AddAction(constantStrings, action, solo);

            if (onDestroy == DeleteGameObjectEnum.Destroy)
            {
                DestroyHandler += () => PearlEventsManager.RemoveAction(constantStrings, action);
            }
            else if (onDestroy == DeleteGameObjectEnum.Disable)
            {
                EnableHandler += (PearlBehaviour @this) => PearlEventsManager.AddAction(constantStrings, action);
                DisableHandler += (PearlBehaviour @this) => PearlEventsManager.RemoveAction(constantStrings, action);
            }
        }

        public void AddAction<T, F, Z>(string constantStrings, Action<T, F, Z> action, DeleteGameObjectEnum onDestroy, bool solo = false)
        {
            PearlEventsManager.AddAction(constantStrings, action, solo);

            if (onDestroy == DeleteGameObjectEnum.Destroy)
            {
                DestroyHandler += () => PearlEventsManager.RemoveAction(constantStrings, action);
            }
            else if (onDestroy == DeleteGameObjectEnum.Disable)
            {
                EnableHandler += (PearlBehaviour @this) => PearlEventsManager.AddAction(constantStrings, action);
                DisableHandler += (PearlBehaviour @this) => PearlEventsManager.RemoveAction(constantStrings, action);
            }
        }

        public void AddUnityAction(UnityEvent unityEvent, UnityAction action, DeleteGameObjectEnum onDestroy)
        {
            if (unityEvent != null && action != null)
            {
                unityEvent.AddListener(action);

                if (onDestroy == DeleteGameObjectEnum.Destroy)
                {
                    DestroyHandler += () => unityEvent.RemoveListener(action);
                }
                else if (onDestroy == DeleteGameObjectEnum.Disable)
                {
                    EnableHandler += (PearlBehaviour @this) => unityEvent.AddListener(action);
                    DisableHandler += (PearlBehaviour @this) => unityEvent.RemoveListener(action);
                }
            }
        }

        public void AddUnityAction<T>(UnityEvent<T> unityEvent, UnityAction<T> action, DeleteGameObjectEnum onDestroy)
        {
            if (unityEvent != null && action != null)
            {
                unityEvent.AddListener(action);

                if (onDestroy == DeleteGameObjectEnum.Destroy)
                {
                    DestroyHandler += () => unityEvent.RemoveListener(action);
                }
                else if (onDestroy == DeleteGameObjectEnum.Disable)
                {
                    EnableHandler += (PearlBehaviour @this) => unityEvent.AddListener(action);
                    DisableHandler += (PearlBehaviour @this) => unityEvent.RemoveListener(action);
                }
            }
        }

        public void AddUnityAction<T, F>(UnityEvent<T, F> unityEvent, UnityAction<T, F> action, DeleteGameObjectEnum onDestroy)
        {
            if (unityEvent != null && action != null)
            {
                unityEvent.AddListener(action);

                if (onDestroy == DeleteGameObjectEnum.Destroy)
                {
                    DestroyHandler += () => unityEvent.RemoveListener(action);
                }
                else if (onDestroy == DeleteGameObjectEnum.Disable)
                {
                    EnableHandler += (PearlBehaviour @this) => unityEvent.AddListener(action);
                    DisableHandler += (PearlBehaviour @this) => unityEvent.RemoveListener(action);
                }
            }
        }

        public void AddUnityAction<T, F, Z>(UnityEvent<T, F, Z> unityEvent, UnityAction<T, F, Z> action, DeleteGameObjectEnum onDestroy)
        {
            if (unityEvent != null && action != null)
            {
                unityEvent.AddListener(action);

                if (onDestroy == DeleteGameObjectEnum.Destroy)
                {
                    DestroyHandler += () => unityEvent.RemoveListener(action);
                }
                else if (onDestroy == DeleteGameObjectEnum.Disable)
                {
                    EnableHandler += (PearlBehaviour @this) => unityEvent.AddListener(action);
                    DisableHandler += (PearlBehaviour @this) => unityEvent.RemoveListener(action);
                }
            }
        }
        #endregion

        #endregion
    }
}
