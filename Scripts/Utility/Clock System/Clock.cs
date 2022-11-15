using UnityEngine;

namespace Pearl.ClockManager
{
    /// <summary>
    /// The class performs a simple clock
    /// </summary>
    public class Clock : ClockAbstract
    {
        #region Constructors
        /// <summary>
        /// This constructor creates the clock
        /// </summary>
        /// <param name = "on">The bool rappresent if the clock must be on or off.</param>
        public Clock(bool on = true)
        {
            if (on)
            {
                ResetOn();
            }
            else
            {
                ResetOff();
            }
        }

        /// <summary>
        /// This constructor creates the clock (with type of scaled Time)
        /// </summary>
        /// <param name = "on">The bool rappresent if the clock must be on or off.</param>
        public Clock(TimeType timeType, bool on = true) : this(on)
        {
            this.timeType = timeType;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Reset the timer for the new perform
        /// </summary>
        public override void ResetOn(TimeType timeType = TimeType.Scaled, float preservedTime = 0)
        {
            if (on)
            {
                ResetOff();
            }

            this.timeType = timeType;
            this.preservedTime = preservedTime;
            this.on = true;
            this.timestart = GetTimeClock();
        }

        public override void ForceTime(float newTime)
        {
            newTime = Mathf.Abs(newTime);
            timestart = GetTimeClock();
            this.preservedTime = newTime;
        }
        #endregion
    }

}