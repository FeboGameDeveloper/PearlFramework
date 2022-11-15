using Pearl.ClockManager;
using Pearl.Tweens;

namespace Pearl.Debug
{
    public class DebugScreenVars : DebugScreenVarsNative
    {
        [DebugScreen("Timer", "activeTimers")]
        public MemberComplexInfo TimerLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TimerContainer>("_timersPool", "Actives", "Count");
        }

        [DebugScreen("Tween", "activeTween")]
        public MemberComplexInfo TweenLActiveLenght()
        {
            return ReflectionExtend.GetValueInfo<TweenContainer>("_containerPool", "Actives", "Count");
        }
    }
}
