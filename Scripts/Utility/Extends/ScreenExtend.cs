using UnityEngine;

namespace Pearl
{
    public static class ScreenExtend
    {
        public static float AspectRatio
        {
            get
            {
                return (float)Screen.width / Screen.height;
            }
        }

        public static bool IsInnerWorldScreen(Vector2 position, Axis2DCombined axisCombined = Axis2DCombined.XY)
        {
            var screenPosition = Camera.main.WorldToScreenPoint(position);
            return IsInnerScreen(screenPosition, axisCombined);
        }

        public static bool IsInnerScreen(Vector2 screenPosition, Axis2DCombined axisCombined = Axis2DCombined.XY)
        {
            if (axisCombined == Axis2DCombined.XY)
            {
                return screenPosition.x <= Screen.width && screenPosition.y <= Screen.height
                    && screenPosition.x >= 0 && screenPosition.y >= 0;
            }
            else if (axisCombined == Axis2DCombined.X)
            {
                return screenPosition.x <= Screen.width && screenPosition.x >= 0;
            }
            else
            {
                return screenPosition.y <= Screen.height && screenPosition.y >= 0;
            }
        }
    }
}
