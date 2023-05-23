using Pearl.ClockManager;
using Pearl.Tweens;

namespace Pearl.Testing.ScreenVars
{
    public class PearlScreenVars : DebugScreenVarsNative
    {
        [DebugScreen("Tween", "activeTween")]
        public MemberComplexInfo TweensLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TweenContainer>("_tweenPool", "_poolListActive", "Count");
        }

        [DebugScreen("Timer", "activeTimer")]
        public MemberComplexInfo TimersLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TimerContainer>("_timersPool", "_poolListActive", "Count");
        }
    }
}