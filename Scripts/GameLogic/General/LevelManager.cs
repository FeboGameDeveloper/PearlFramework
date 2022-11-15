using Pearl.Input;
using UnityEngine;
using Pearl.Events;
using System;
#if NODE_CANVAS
using Pearl.NodeCanvas;
#endif

namespace Pearl
{
    public enum StateLevelEnum { Pause, InGame, GameOver, }

    [DisallowMultipleComponent]
    public abstract class LevelManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private bool isOnApplicationPause = true;
        [SerializeField]
        private string pauseInputMap = "UI";

#if NODE_CANVAS
        [SerializeField]
        protected PearlFSMOwner FSM = null;
#endif

        [ReadOnly]
        [SerializeField]
        protected StateLevelEnum _stateLevel = StateLevelEnum.InGame;
        private Action _functionPause;



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
            set
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
                manager.ResetGamePrivate();
            }
        }

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

            _functionPause = () => CallPause(true);

            PearlEventsManager.AddAction(ConstantStrings.Gameover, OnGameOver);
            PearlEventsManager.AddAction<bool>(ConstantStrings.Pause, CallPause);
            var input = InputManager.Input;
            if (input)
            {
                input.PerformedHandle(ConstantStrings.Pause, _functionPause, ActionEvent.Add, StateButton.Down, pauseInputMap);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _stateLevel = StateLevelEnum.InGame;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PearlEventsManager.RemoveAction(ConstantStrings.Gameover, OnGameOver);
            PearlEventsManager.RemoveAction<bool>(ConstantStrings.Pause, CallPause);
            var input = InputManager.Input;
            if (input)
            {
                input.PerformedHandle(ConstantStrings.Pause, _functionPause, ActionEvent.Remove, StateButton.Down, pauseInputMap);
            }
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

        protected virtual void OnGameOver()
        {
            _stateLevel = StateLevelEnum.GameOver;
        }

        protected virtual void ResetGamePrivate()
        {
        }

        private static void CallPause(bool pause)
        {
            if (GetIstance(out var manager))
            {
#if INK
                DialogsManager.Pause(pause);
#endif

                if (pause)
                {
                    manager.PauseInternal();
                }
                else
                {
                    manager.UnpauseInternal();
                }
            }
        }

#if NODE_CANVAS
        protected void ChangeLabel(string label)
        {
            GameManager.CheckTransitionsAfterChangeLabel(label);
        }
#endif

        protected abstract void PauseInternal();

        protected abstract void UnpauseInternal();
    }

}