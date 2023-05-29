using Pearl.Input;
using UnityEngine;
using Pearl.Events;
using System;
#if NODE_CANVAS
using Pearl.NodeCanvas;
#endif

namespace Pearl
{
    #region Enum
    public enum StateLevelEnum { Pause, InGame, GameOver, }
    #endregion

    [DisallowMultipleComponent]
    public abstract class LevelManager : PearlBehaviour, ISingleton
    {
        #region Inspector Fields
        [SerializeField]
        private bool isOnApplicationPause = true;
        [SerializeField]
        protected string pauseInputMap = "UI";
        [SerializeField]
        protected string gameplayInputMap = "Gameplay";

#if NODE_CANVAS
        [SerializeField]
        protected PearlFSMOwner FSM = null;
#endif
        [ReadOnly, SerializeField]
        protected StateLevelEnum _stateLevel = StateLevelEnum.InGame;
        #endregion

        #region Private Fields


        #endregion

        #region Static
        public static bool GetIstance(out LevelManager result)
        {
            return Singleton<LevelManager>.GetIstance(out result);
        }

        public static StateLevelEnum StateLevel
        {
            get
            {
                if (GetIstance(out var manager))
                {
                    return manager._stateLevel;
                }
                else return StateLevelEnum.InGame;
            }
            private set
            {
                if (GetIstance(out var manager))
                {
                    manager._stateLevel = value;
                }
            }
        }

        public static bool IsPause { get { return StateLevel == StateLevelEnum.Pause; } }

        public static void ResetGame()
        {
            if (GetIstance(out var manager))
            {
                PearlEventsManager.CallEvent(ConstantStrings.Reset);
                manager.ResetGamePrivate();
            }
        }

        public static void GameOver()
        {
            if (GetIstance(out var manager))
            {
                manager._stateLevel = StateLevelEnum.InGame;
                PearlEventsManager.CallEvent(ConstantStrings.Gameover);
                manager.GameOverPrivate();
            }
        }

        public static void CallPause(bool pause)
        {
            if (GetIstance(out var manager))
            {
#if INK
                DialogsManager.Pause(pause);
#endif
                if (pause && StateLevel == StateLevelEnum.InGame)
                {
                    StateLevel = StateLevelEnum.Pause;
                    PearlEventsManager.CallEvent(ConstantStrings.Pause, pause);
                    manager.PauseInternal();
                }
                else if (!pause && StateLevel == StateLevelEnum.Pause)
                {
                    StateLevel = StateLevelEnum.InGame;
                    PearlEventsManager.CallEvent(ConstantStrings.Pause, pause);
                    manager.UnpauseInternal();
                }
            }
        }
        #endregion

        #region Unity Callbacks
        protected virtual void Reset()
        {
#if NODE_CANVAS
            FSM = GetComponent<PearlFSMOwner>();
#endif
        }

        protected override void Start()
        {
            base.Start();
        } 

        //si attiva quado la window non è selezionata
        private void OnApplicationPause(bool pause)
        {
            if (isOnApplicationPause && pause && _stateLevel == StateLevelEnum.InGame)
            {
                CallPause(true);
            }
        }
        #endregion

        #region Abstract
        protected abstract void PauseInternal();

        protected abstract void UnpauseInternal();

        protected abstract void ResetGamePrivate();

        protected abstract void GameOverPrivate();
        #endregion
    }
}