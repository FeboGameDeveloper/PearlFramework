namespace Pearl.ClockManager
{
    /// <summary>
    /// The class is the abstract clock, it has the fundamental methods (the clocks function with Time.time)
    /// that any clock must have
    /// </summary>
    public abstract class ClockAbstract : IPause, IReset
    {
        #region Protected Fields
        /// <summary>
        /// The time when the clock starts
        /// </summary>
        protected float timestart;
        /// <summary>
        /// Determines if the clock is paused
        /// </summary>
        protected bool pause;
        /// <summary>
        /// The time preserved for pause and other.
        /// </summary>
        protected float preservedTime;
        /// <summary>
        /// The clock is on or off?
        /// </summary>
        protected bool on = false;
        /// <summary>
        /// The clock is on or off?
        /// </summary>
        protected TimeType timeType = TimeType.Scaled;
        #endregion

        #region Properties
        //Is On?
        public bool On { get { return on; } }
        //Is Pause?
        public bool IsPause { get { return pause; } }
        //the current time from the start
        public virtual float Time { get { return pause ? preservedTime : preservedTime + GetTimeClock() - timestart; } }
        #endregion

        #region Finalizer
        ~ClockAbstract()
        {
            ResetOff();
        }
        #endregion

        #region Public Methods
        public abstract void ResetOn(TimeType timeType = TimeType.Scaled, float preservedTime = 0);

        /// <summary>
        /// Resets the timer in off
        /// </summary>
        public void ResetOff()
        {
            on = false;
            preservedTime = 0;
            pause = false;
            timestart = 0;
        }

        public abstract void ForceTime(float newTime);
        #endregion

        #region Interface Methods
        /// <summary>
        /// For pause
        /// </summary>
        public void Pause(bool onPause)
        {
            if (on)
            {
                if (onPause && !pause)
                {
                    preservedTime = Time;
                    pause = true;
                }
                else if (pause)
                {
                    timestart = GetTimeClock();
                    pause = false;
                }
            }
        }

        public void ResetElement()
        {
            ResetOn();
        }

        public void DisableElement()
        {
            ResetOff();
        }
        #endregion

        #region Private Method
        protected float GetTimeClock()
        {
            return TimeExtend.GetTime(timeType);
        }
        #endregion
    }
}
