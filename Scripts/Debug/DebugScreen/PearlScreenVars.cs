using Pearl.ClockManager;
using Pearl.Tweens;
using System;
using UnityEngine;

namespace Pearl.Testing.ScreenVars
{
    public class PearlScreenVars : DebugScreenVarsNative
    {
        private float _currentTime = 0;
        private int _frameCounter = 0;
        private int _lastFramerate = 0;


        [DebugScreen("Tween", "activeTween")]
        public MemberComplexInfo GetTweensLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TweenContainer>("_tweenPool", "_poolListActive", "Count");
        }

        [DebugScreen("Timer", "activeTimer")]
        public MemberComplexInfo GetTimersLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TimerContainer>("_timersPool", "_poolListActive", "Count");
        }

        [DebugScreen("FPS", "FPS")]
        public MemberComplexInfo GetFPS()
        {
            Func<int> getFPSInternal = () =>
            {
                _currentTime += Time.deltaTime;

                if (_currentTime >= 1)
                {
                    _lastFramerate = Mathf.RoundToInt((float)_frameCounter / _currentTime);
                    _frameCounter = 0;
                    _currentTime = 0;
                }
                else
                {
                    _frameCounter++;
                }

                return _lastFramerate;
            };

            MemberComplexInfo memberComplexInfo = new(getFPSInternal.Method, this);
            return memberComplexInfo;
        }
    }
}