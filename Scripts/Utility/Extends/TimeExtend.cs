using UnityEngine;

namespace Pearl
{
    public static class TimeExtend
    {
        public static float GetDeltaTime(TimeType timeType = TimeType.Scaled, UpdateModes mode = UpdateModes.Update)
        {
            if (timeType == TimeType.Scaled)
            {
                return mode == UpdateModes.Update || mode == UpdateModes.LateUpdate ? Time.deltaTime : Time.fixedDeltaTime;
            }
            else
            {
                return mode == UpdateModes.Update || mode == UpdateModes.LateUpdate ? Time.unscaledDeltaTime : Time.fixedUnscaledDeltaTime;
            }
        }

        public static float GetTime(TimeType timeType = TimeType.Scaled, UpdateModes mode = UpdateModes.Update)
        {
            if (timeType == TimeType.Scaled)
            {
                return mode == UpdateModes.Update || mode == UpdateModes.LateUpdate ? Time.time : Time.fixedTime;
            }
            else
            {
                return mode == UpdateModes.Update || mode == UpdateModes.LateUpdate ? Time.unscaledTime : Time.fixedUnscaledTime;
            }
        }
    }
}
