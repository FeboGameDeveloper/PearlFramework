using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    public class PearlWaitForSeconds : CustomYieldInstruction, IPause
    {
        #region Private fields
        private float _delta;
        private readonly Timer _timer;
        #endregion

        #region Propiety
        public float Delta { get { return _delta; } }

        public override bool keepWaiting
        {
            get
            {
                return !_timer.IsFinish(out _, out _delta);
            }
        }
        #endregion

        #region Constructors
        public PearlWaitForSeconds(float time, TimeType timeType = TimeType.Scaled)
        {
            _timer = new Timer(time, true, timeType);
        }

        public PearlWaitForSeconds()
        {
            _timer = new Timer();
        }
        #endregion

        #region Public methods
        public void Pause(bool onPause)
        {
            _timer.Pause(onPause);
        }

        public PearlWaitForSeconds Reset(float time, TimeType timeType = TimeType.Scaled)
        {
            _timer.ResetOn(time, timeType);
            return this;
        }
        #endregion
    }
}
