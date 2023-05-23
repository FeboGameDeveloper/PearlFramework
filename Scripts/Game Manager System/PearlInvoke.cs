using Pearl.ClockManager;
using System;
using System.Collections.Generic;

namespace Pearl
{
    public static class PearlInvoke
    {
        #region Private Fields
        private static readonly Dictionary<Delegate, TimerContainer> _timers = new();
        #endregion

        #region WaitForMethod

        public static int TimerCount
        {
            get { return _timers != null ? _timers.Count : 0; }
        }

        #region stop
        public static void StopTimer(Action action)
        {
            StopTimerInternal(action);
        }

        public static void StopTimer<T>(Action<T> action)
        {
            StopTimerInternal(action);
        }

        public static void StopTimer<T, F>(Action<T, F> action)
        {
            StopTimerInternal(action);
        }

        public static void StopAccurateTimer(Action<float, float> action)
        {
            StopTimerInternal(action);
        }

        public static void StopAccurateTimer<T>(Action<float, float, T> action)
        {
            StopTimerInternal(action);
        }

        public static void StopAccurateTimer<T, F>(Action<float, float, T, F> action)
        {
            StopTimerInternal(action);
        }

        private static void StopTimerInternal(Delegate action)
        {
            if (_timers.IsNotNullAndTryGetValue(action, out var currentCroutine))
            {
                currentCroutine.Kill();
                _timers.Remove(action);
            }
        }
        #endregion

        #region pause
        public static void PauseTimer(Action action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        public static void PauseTimer<T>(Action<T> action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        public static void PauseTimer<T, F>(Action<T, F> action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        public static void PauseAccurateTimer(Action<float, float> action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        public static void PauseAccurateTimer<T>(Action<float, float, T> action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        public static void PauseAccurateTimer<T, F>(Action<float, float, T, F> action, bool pause)
        {
            PauseTimerInternal(action, pause);
        }

        private static void PauseTimerInternal(Delegate action, bool pause)
        {
            if (_timers.IsNotNullAndTryGetValue(action, out var currentTimer))
            {
                currentTimer.Pause(pause);
            }
        }
        #endregion

        #region Wait
        private static void UpdateTimers(Delegate action, TimerContainer newTimer)
        {
            if (_timers.IsNotNullAndTryGetValue(action, out var oldTimer))
            {
                oldTimer.Kill();
            }
            _timers?.Update(action, newTimer);
        }

        public static void WaitForMethod(float waitTime, Action action, TimeType timeType = TimeType.Scaled)
        {
            Action<TimerContainer, float, float> superAction = null;
            superAction += (_, _, _) => _timers?.Remove(action);
            superAction += (_, _, _) => action.Invoke();

            bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
            if (isTimer)
            {
                UpdateTimers(action, timer);
            }
        }

        public static void WaitForMethod<T>(float waitTime, Action<T> action, T parm1, TimeType timeType = TimeType.Scaled)
        {
            Action<TimerContainer, float, float> superAction = null;
            superAction += (_, _, _) => _timers?.Remove(action);
            superAction += (_, _, _) => action.Invoke(parm1);

            bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
            if (isTimer)
            {
                UpdateTimers(action, timer);
            }
        }

        public static void WaitForMethod<T, F>(float waitTime, Action<T, F> action, T parm1, F parm2, TimeType timeType = TimeType.Scaled)
        {
            Action<TimerContainer, float, float> superAction = null;
            superAction += (_, _, _) => _timers?.Remove(action);
            superAction += (_, _, _) => action.Invoke(parm1, parm2);

            bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
            if (isTimer)
            {
                UpdateTimers(action, timer);
            }
        }

        public static void WaitAccurateForMethod(float waitTime, Action<float, float> action, TimeType timeType = TimeType.Scaled)
        {
            if (waitTime <= 0)
            {
                action.Invoke(0, MathF.Abs(waitTime));
            }
            else
            {
                Action<TimerContainer, float, float> superAction = null;
                superAction += (_, _, _) => _timers?.Remove(action);
                superAction += (_, left, right) => action.Invoke(left, right);

                bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
                if (isTimer)
                {
                    UpdateTimers(action, timer);
                }
            }
        }

        public static void WaitAccurateForMethod<T>(float waitTime, Action<float, float, T> action, T parm1, TimeType timeType = TimeType.Scaled)
        {
            if (waitTime <= 0)
            {
                action.Invoke(0, MathF.Abs(waitTime), parm1);
            }
            else
            {
                Action<TimerContainer, float, float> superAction = null;
                superAction += (_, _, _) => _timers?.Remove(action);
                superAction += (_, left, right) => action.Invoke(left, right, parm1);

                bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
                if (isTimer)
                {
                    UpdateTimers(action, timer);
                }
            }
        }

        public static void WaitAccurateForMethod<T, F>(float waitTime, Action<float, float, T, F> action, T parm1, F parm2, TimeType timeType = TimeType.Scaled)
        {
            if (waitTime <= 0)
            {
                action.Invoke(0, MathF.Abs(waitTime), parm1, parm2);
            }
            {
                Action<TimerContainer, float, float> superAction = null;
                superAction += (_, _, _) => _timers?.Remove(action);
                superAction += (_, left, right) => action.Invoke(left, right, parm1, parm2);

                bool isTimer = TimerContainer.CreateTimer(out var timer, waitTime, true, true, superAction, timeType);
                if (isTimer)
                {
                    UpdateTimers(action, timer);
                }
            }
        }
        #endregion

        #endregion
    }
}
