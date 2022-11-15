using Pearl.Audio;
using Pearl.Events;
using Pearl.Storage;
using System;
using TypeReferences;
using UnityEngine;

namespace Pearl
{
    [DisallowMultipleComponent]
    /// <summary>
    /// The general Game manager. It will be the father of every game-specific game manager
    /// </summary>
    public class GameManager : PearlBehaviour, IStoragePlayerPrefs, ISingleton
    {
        #region Static
        public static event Action OnUpate;
        public static event Action OnFixedUpdate;
        public static event Action OnLateUpdate;

        public static void ChangeUpdateAction(ActionEvent actionEvent, UpdateModes updateMode, Action action)
        {
            if (updateMode == UpdateModes.Update)
            {
                if (actionEvent == ActionEvent.Add)
                {
                    OnUpate += action;
                }
                else
                {
                    OnUpate -= action;
                }
            }
            else if (updateMode == UpdateModes.FixedUpdate)
            {
                if (actionEvent == ActionEvent.Add)
                {
                    OnFixedUpdate += action;
                }
                else
                {
                    OnFixedUpdate -= action;
                }
            }
            else
            {
                if (actionEvent == ActionEvent.Add)
                {
                    OnLateUpdate += action;
                }
                else
                {
                    OnLateUpdate -= action;
                }
            }
        }
        #endregion

        #region Inspector Fields
        [Header("General setting")]

        [SerializeField]
        [Tooltip("Game Name")]
        [ReadOnly]
        private string gameName = "Game";

        [SerializeField]
        [ClassImplements(typeof(GameVersionManager))]
        public ClassTypeReference versionType;

        [SerializeField]
        [ReadOnly]
        private string gameVersionString;

        [SerializeField]
        private bool thereIsOnline = true;

        [SerializeField]
        private bool updateWindowSize = true;

        [Header("Components")]

        [InterfaceType(typeof(IFSM))]
        [SerializeField]
        protected UnityEngine.Object FSMObject = null;

        [Header("Prefabs")]

        [SerializeField]
        private GameObject canvasPrefab = null;

        [SerializeField]
        private GameObject blackPagePrefab = null;
        #endregion

        #region Private Fields
        private BlackPageManager _blackPageManager;
        private bool _isInternetReachability = false;
        #endregion

        #region Propieties
        public static string NameGame
        {
            get
            {
                if (Singleton<GameManager>.GetIstance(out var gameManager))
                {
                    return gameManager.gameName;
                }
                return null;
            }
        }

        public static bool ThereIsOnline
        {
            get
            {
                if (Singleton<GameManager>.GetIstance(out var gameManager))
                {
                    return gameManager.thereIsOnline;
                }
                return false;
            }
        }

        /// <summary>
        /// The current scene in the game
        /// </summary>
        public string GameVersion
        {
            get { return gameVersionString; }
        }

        private IFSM FSM;
        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            if (FSMObject != null)
            {
                FSM = (IFSM)FSMObject;
            }

            gameName = Application.productName;

            if (canvasPrefab)
            {
                GameObjectExtend.CreateGameObject(canvasPrefab, out _, true);
            }

            if (blackPagePrefab)
            {
                GameObjectExtend.CreateUIlement<BlackPageManager>(blackPagePrefab, out _blackPageManager, UI.CanvasTipology.UI, ChangeTypeEnum.Modify, true);
                if (_blackPageManager)
                {
                    _blackPageManager.Disappear();
                }
            }
        }

        protected override void Start()
        {
            base.Start();

            if (versionType != null)
            {
                ReflectionExtend.UseStaticMethodWithResult<String>(versionType.Type, "ControlVersion", out gameVersionString, versionType.Type);
            }

            TextManager.SetVariableString("gameName", gameName);
        }

        protected virtual void Update()
        {
            OnUpate?.Invoke();

            ControlIsOnline();

            if (updateWindowSize)
            {
                WindowManager.OnUpdate();
            }
        }

        protected virtual void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        protected virtual void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        #endregion

        #region Init Methods
        private void ControlIsOnline()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                if (_isInternetReachability)
                {
                    _isInternetReachability = false;
                    PearlEventsManager.CallEvent(ConstantStrings.InternetReachability, PearlEventType.Normal, _isInternetReachability);
                }
            }
            else if (!_isInternetReachability)
            {
                _isInternetReachability = true;
                PearlEventsManager.CallEvent(ConstantStrings.InternetReachability, PearlEventType.Normal, _isInternetReachability);
            }
        }
        #endregion

        #region Singleton Methods
        public static void Quit()
        {
#if UNITY_STANDALONE
            Application.Quit();
#endif

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public static void SaveOption()
        {
            AudioManager.SaveAudio();
#if LOCALIZATION
            LocalizationManager.SaveLanguage();
#endif
        }

        #region FSM
        public static void CheckTransitions(bool finishState)
        {
            if (Singleton<GameManager>.GetIstance(out var gameManager) && gameManager.FSM != null)
            {
                gameManager.FSM.CheckTransitions(finishState);
            }
        }

        public static void CheckTransitionsAfterChangeLabel(string newLabel, bool forceFinishState = true)
        {
            if (Singleton<GameManager>.GetIstance(out var gameManager) && newLabel != null && gameManager.FSM != null)
            {
                gameManager.FSM.ChangeLabel(newLabel);
                gameManager.FSM.CheckTransitions(forceFinishState);
            }
        }

        public static void UpdateVariableFSM<T>(string nameVar, T content)
        {
            if (Singleton<GameManager>.GetIstance(out var gameManager) && nameVar != null && gameManager.FSM != null)
            {
                gameManager.FSM.UpdateVariable<T>(nameVar, content);
            }
        }

        public static T GetVariableFSM<T>(string nameVar)
        {
            if (Singleton<GameManager>.GetIstance(out var gameManager) && gameManager.FSM != null)
            {
                return gameManager.FSM.GetVariable<T>(nameVar);
            }
            return default;
        }

        public static void RemoveVariableFSM<T>(string nameVar)
        {
            if (Singleton<GameManager>.GetIstance(out var gameManager) && gameManager.FSM != null)
            {
                gameManager.FSM.RemoveVariable<T>(nameVar);
            }
        }
        #endregion


        #endregion
    }
}
