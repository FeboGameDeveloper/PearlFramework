using Pearl.Debug;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.ClockManager
{
    /// <summary>
    /// This class manages complex timers: it performs updates and notifies the user when they expire.
    /// </summary>
    public class TimerContainer : IReset, IPause
    {
        #region static

        #region Private Fields
        private readonly static SimplePool<TimerContainer> _timersPool = new(false);
        private readonly static List<TimerContainer> _timerActiveList = new();
        #endregion

        #region Constructor
        static TimerContainer()
        {
            GameManager.OnUpate += Update;
        }
        #endregion

        #region CreateTimer
        //to create a complex timer: To do after awake
        public static bool CreateTimer(out TimerContainer timer, bool autoKill = false, Action<TimerContainer, float, float> OnComplete = null, in TimeType timeType = TimeType.Scaled)
        {
            return CreateTimer(out timer, 0, false, autoKill, OnComplete, timeType);
        }

        //to create a complex timer: To do after awake
        public static bool CreateTimer(out TimerContainer timer, Range randomDuration, bool active = true, bool autoKill = false, Action<TimerContainer, float, float> OnComplete = null, in TimeType timeType = TimeType.Scaled)
        {
            return CreateTimer(out timer, RandomExtend.RandomRangeFloat(randomDuration), active, autoKill, OnComplete, timeType);
        }

        //to create a complex timer: To do after awake
        public static bool CreateTimer(out TimerContainer timer, Vector2 randomDuration, bool active = true, bool autoKill = false, Action<TimerContainer, float, float> OnComplete = null, in TimeType timeType = TimeType.Scaled)
        {
            return CreateTimer(out timer, RandomExtend.RandomVectorFloat(randomDuration), active, autoKill, OnComplete, timeType);
        }

        //to create a complex timer: To do after awake
        public static bool CreateTimer(out TimerContainer timer, float duration, bool active = true, bool autoKill = false, Action<TimerContainer, float, float> OnComplete = null, in TimeType timeType = TimeType.Scaled)
        {
            timer = _timersPool?.Get();

            if (timer != null)
            {
                return timer.Set(duration, active, autoKill, OnComplete, timeType);
            }
            else
            {
                LogManager.LogWarning("Creation timer failed");
                return false;
            }
        }
        #endregion

        #region Private Methods
        private static void Update()
        {
            if (_timerActiveList == null)
            {
                return;
            }

            for (int i = _timerActiveList.Count - 1; i >= 0; i--)
            {
                _timerActiveList[i]?.IsFinish();
            }
        }

        private static void Kill(TimerContainer timerContainer)
        {
            if (_timerActiveList != null && _timersPool != null)
            {
                _timerActiveList.Remove(timerContainer);
                _timersPool.Remove(timerContainer);
            }
        }

        private static void ResetOn(TimerContainer timerContainer)
        {
            if (_timerActiveList != null)
            {
                _timerActiveList.AddOnce(timerContainer);
            }
        }

        private static void ResetOff(TimerContainer timerContainer)
        {
            if (_timerActiveList != null)
            {
                _timerActiveList.Remove(timerContainer);
            }
        }
        #endregion

        #endregion

        #region no static

        #region Private fields
        private readonly Timer _timer;
        private bool _enable = false;
        private bool _isAutoKill;


        private float _differenceLeft;
        private float _differenceRight;
        #endregion

        #region Event
        public event Action<TimerContainer, float, float> OnComplete = null;
        #endregion

        #region Proprietes
        public bool On
        {
            get { return _timer.On; }
        }

        //Is Enable?
        public bool Enable
        {
            get { return _enable; }
            private set { _enable = value; }
        }

        //After the timer expires, it self-destructs
        public bool AutoKill
        {
            get { return _isAutoKill; }
            set { _isAutoKill = value; }
        }

        /// <summary>
        /// The maximum limit of timer
        /// </summary>
        public float Limit
        {
            get { return _timer.Limit; }
            set { _timer.Limit = value; }
        }

        /// <summary>
        /// How much time has passed in percentage[0 - 1]
        /// </summary>
        public float TimeInPercent { get { return _timer.TimeInPercent; } }

        /// <summary>
        /// How much time has passed
        /// </summary>
        public float Time { get { return _timer.Time; } }

        /// <summary>
        /// How much time has passed in countDown [limit-0]
        /// </summary>
        public float TimeReversed { get { return _timer.TimeReversed; } }

        /// <summary>
        /// How much time has passed in percentage[0 - 1] in countdown
        /// </summary>
        public float TimeInPercentReversed { get { return _timer.TimeInPercentReversed; } }

        public bool IsPause { get { return _timer.IsPause; } }
        #endregion

        #region Constructors
        private TimerContainer()
        {
            _timer = new Timer();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Force timer in a new limit
        /// </summary>
        public void ForceTimeInPercent(float percent)
        {
            if (_timer != null)
            {
                _timer.ForceTimeInPercent(percent);
                IsFinish();
            }
        }

        /// <summary>
        /// Resets the timer in off
        /// </summary>
        public void ResetOff()
        {
            if (_timer == null)
            {
                return;
            }

            _timer.ResetOff();
            ResetOff(this);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        public bool ResetOn(in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            return ResetOn(Limit, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        public bool ResetOn(in Vector2 duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            float randomNumber = RandomExtend.RandomVectorFloat(duration);
            return ResetOn(randomNumber, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        public bool ResetOn(in Range duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            float randomNumber = RandomExtend.RandomRangeFloat(duration);
            return ResetOn(randomNumber, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        public bool ResetOn(in float duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            if (!_enable)
            {
                LogManager.LogWarning("the container is not enable");
            }
            else
            {
                if (_timer != null)
                {
                    _timer.ResetOn(duration, timeType, preservedTime);
                    ResetOn(this);
                    return !IsFinish();
                }
            }

            return false;
        }

        /// <summary>
        /// Kill the complex timer (do it when you no longer need the timer or it will stay in memory)
        /// </summary>
        public void Kill()
        {
            if (_enable)
            {
                _timer.ResetOff();
                Kill(this);
                _enable = false;
                OnComplete = null;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Set the complex timer
        /// </summary>
        private bool Set(in float duration, in bool active = true, in bool autoKill = false, in Action<TimerContainer, float, float> OnActon = null, in TimeType timeType = TimeType.Scaled)
        {
            Limit = duration;
            AutoKill = autoKill;
            if (OnActon != null)
            {
                OnComplete += OnActon;
            }
            Enable = true;

            if (active)
            {
                return ResetOn(timeType);
            }

            return true;
        }

        /// <summary>
        /// Is the timer finished?
        /// </summary>
        private bool IsFinish()
        {
            if (!_enable)
            {
                LogManager.LogWarning("the container is not enable");
            }

            if (_timer == null)
            {
                return false;
            }

            bool isFinish = _timer.IsFinish(out _differenceLeft, out _differenceRight);

            if (isFinish)
            {
                OnComplete?.Invoke(this, _differenceLeft, _differenceRight);

                if (!AutoKill)
                {
                    ResetOff();
                }
                else
                {
                    Kill();
                }
            }

            return isFinish;
        }
        #endregion

        #region Interface methods
        public void DisableElement()
        {
            _timer.DisableElement();
        }

        public void ResetElement()
        {
            _timer.ResetElement();

            OnComplete = null;
            ResetOff();
        }

        public void Pause(bool onPause)
        {
            if (!_enable)
            {
                LogManager.LogWarning("the container is not enable");
            }

            if (_timer == null)
            {
                return;
            }

            _timer.Pause(onPause);
        }
        #endregion

        #endregion
    }
}