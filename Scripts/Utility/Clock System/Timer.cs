using UnityEngine;

namespace Pearl.ClockManager
{
    /// <summary>
    /// The struct is the simple timer , it has the fundamental methods (the clock functions with Time.deltaTime)
    /// that any clock must have
    /// </summary>
    public struct SimpleTimer
    {
        #region Private fields
        private float _duration;
        private float _auxTime;
        private bool _pause;
        private TimeType timeType;
        #endregion

        public bool IsPause { get { return _pause; } }

        #region Constructors
        public SimpleTimer(in float duration, in TimeType timeType = TimeType.Scaled)
        {
            this._duration = duration;
            this.timeType = timeType;
            _pause = false;
            _auxTime = 0;
        }

        public SimpleTimer(in Vector2 duration, in TimeType timeType = TimeType.Scaled)
        {
            this._duration = RandomExtend.RandomVectorFloat(duration);
            this.timeType = timeType;
            _pause = false;
            _auxTime = 0;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Reset the simple timer in on for the new perform
        /// </summary>
        public void Reset(in TimeType timeType = TimeType.Scaled)
        {
            this.timeType = timeType;
            _auxTime = 0;
        }

        /// <summary>
        /// Reset the simple timer in on for the new perform
        /// </summary>
        public void Reset(in float duration, in TimeType timeType = TimeType.Scaled)
        {
            this._duration = duration;
            Reset(duration, timeType);
        }

        /// <summary>
        /// Reset the simple timer in on for the new perform
        /// </summary>
        public void Reset(in Vector2 duration, in TimeType timeType = TimeType.Scaled)
        {
            Reset(RandomExtend.RandomVectorFloat(duration), timeType);
        }

        /// <summary>
        /// Pause the simple timer
        /// </summary>
        public void Pause(in bool pause)
        {
            this._pause = pause;
        }

        /// <summary>
        /// Has the timer finish?
        /// </summary>
        public bool IsFinish()
        {
            float deltaTime = TimeExtend.GetDeltaTime(timeType);
            float addTime = _pause ? 0 : deltaTime;
            _auxTime += addTime;
            return _auxTime >= _duration;
        }

        public bool IsFinish(out float differenceLeft, out float differenceRight)
        {
            float deltaTime = TimeExtend.GetDeltaTime(timeType);
            float addTime = _pause ? 0 : deltaTime;
            _auxTime += addTime;
            if (_auxTime >= _duration)
            {
                differenceRight = _auxTime - _duration;
                differenceLeft = deltaTime - differenceRight;
                return true;
            }
            else
            {
                differenceLeft = 0;
                differenceRight = 0;
                return false;
            }
        }
        #endregion
    }

    /// <summary>
    /// The class performs a timer
    /// </summary>
    public class Timer : ClockAbstract
    {
        #region Private fields
        /// <summary>
        /// The maximum limit of clock
        /// </summary>
        private float _limit = Mathf.Infinity;
        #endregion

        #region Properties
        /// <summary>
        /// The maximum limit of timer
        /// </summary>
        public float Limit { get { return _limit; } set { _limit = value; } }

        /// <summary>
        /// How much time has passed in percentage[0 - 1]
        /// </summary>
        public float TimeInPercent
        {
            get
            {
                if (this.on)
                {
                    return Mathf.Clamp01(Time / this._limit);
                }
                else
                {
                    return 0f;
                }
            }
        }

        /// <summary>
        /// How much time has passed
        /// </summary>
        public override float Time { get { return pause ? preservedTime : Mathf.Min(preservedTime + GetTimeClock() - timestart, _limit); } }

        /// <summary>
        /// How much time has passed in percentage[0 - 1] in countdown
        /// </summary>
        public float TimeInPercentReversed { get { return 1 - TimeInPercent; } }

        /// <summary>
        /// How much time has passed in countDown [limit-0]
        /// </summary>
        public float TimeReversed { get { return _limit - Time; } }
        #endregion

        #region Constructors
        /// <summary>
        /// This constructor creates a Timer.
        /// </summary>
        /// <param name = "duration"> The duration of timer</param>
        public Timer(in float duration, in bool active, in TimeType timeType = TimeType.Scaled) : this(new Vector2(duration, duration), active, timeType)
        {
        }

        /// <summary>
        /// This constructor creates a Timer.
        /// </summary>
        /// <param name = "duration"> The duration of timer</param>
        public Timer(in Range duration, in bool active, in TimeType timeType = TimeType.Scaled) : this(new Vector2(duration.Min, duration.Max), active, timeType)
        {
        }

        /// <summary>
        /// This constructor creates a Timer.
        /// </summary>
        /// <param name = "duration"> The duration of timer</param>
        public Timer(in Vector2 duration, in bool active, in TimeType timeType = TimeType.Scaled)
        {
            if (active)
            {
                ResetOn(duration, timeType);
            }
            else
            {
                _limit = RandomExtend.RandomVectorFloat(duration);
                ResetOff();
            }
        }

        /// <summary>
        /// This constructor create a Timer.
        /// </summary>
        public Timer(in TimeType timeType = TimeType.Scaled)
        {
            this.timeType = timeType;
            ResetOff();
        }
        #endregion

        #region Public Methods
        public override void ForceTime(float newTime)
        {
            newTime = Mathf.Clamp(newTime, 0, _limit);
            timestart = GetTimeClock();
            this.preservedTime = newTime;
        }

        public void ForceTimeInPercent(float newTime)
        {
            newTime *= _limit;
            ForceTime(newTime);
        }

        public bool IsFinish()
        {
            return !on || Time >= _limit;
        }

        public bool IsFinish(out float differenceRight)
        {
            return IsFinish(out float differenceLeft, out differenceRight);
        }

        /// <summary>
        /// Is the timer finished?
        /// </summary>
        public bool IsFinish(out float differenceLeft, out float differenceRight)
        {
            float time = pause ? preservedTime : Mathf.Min(preservedTime + GetTimeClock() - timestart, float.MaxValue);
            if (!on || time >= _limit)
            {
                differenceRight = time - Limit;
                differenceLeft = TimeExtend.GetDeltaTime(timeType) - differenceRight;
                return true;
            }
            else
            {
                differenceLeft = differenceRight = 0;
                return false;
            }
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        public override void ResetOn(TimeType timeType = TimeType.Scaled, float preservedTime = 0)
        {
            ResetOn(_limit, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        /// <param name = "duration"> The duration of timer</param>
        public void ResetOn(in Range duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            float randomNumber = RandomExtend.RandomRangeFloat(duration);
            ResetOn(randomNumber, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        /// <param name = "duration"> The random duration of timer</param>
        public void ResetOn(in Vector2 duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            float randomNumber = RandomExtend.RandomVectorFloat(duration);
            ResetOn(randomNumber, timeType, preservedTime);
        }

        /// <summary>
        /// Reset the timer in on for the new perform
        /// </summary>
        /// <param name = "duration"> The random duration of timer</param>
        public void ResetOn(in float duration, in TimeType timeType = TimeType.Scaled, in float preservedTime = 0)
        {
            if (on)
            {
                ResetOff();
            }

            this.timeType = timeType;
            this.preservedTime = preservedTime;

            on = true;
            timestart = GetTimeClock();
            _limit = duration;
        }
        #endregion
    }
}