#if SPLINE

using UnityEngine;

namespace Pearl.Paths
{
    public static class PathUtility
    {
#region Public methods
        // Get the location of the path based on how far you have traveled
        public static bool GetPointByLength(this PearlSpline path, out Vector3 result, ref float length)
        {
            if (path == null)
            {
                result = default;
                return false;
            }

            var totalLength = path.CalculateLength();
            length = MathfExtend.Clamp0(length, totalLength);
            float t = path.GetPercentByLength(length, totalLength);
            result = path.EvaluatePosition(t);

            return t >= 1;
        }

        // Get the inverse percentage of the path based on how far you have traveled
        public static float GetInversePercentByLength(this PearlSpline path, float length)
        {
            if (path == null)
            {
                return 0f;
            }

            float totalLength = path.CalculateLength();
            return path.GetPercentByLength(totalLength - length, totalLength);
        }

        // Get the length of the path made based on the percentage
        public static float GetLengthByPercent(this PearlSpline path, float t)
        {
            if (path == null)
            {
                return 0f;
            }

            t = Mathf.Clamp01(t);
            return path.CalculateLength() * t;
        }

        // Get the percentage of the path based on how far you have traveled
        public static float GetPercentByLength(this PearlSpline path, float length, float totalLength = 0)
        {
            if (path == null)
            {
                return 0f;
            }

            if (totalLength == 0)
            {
                totalLength = path.CalculateLength();
            }

            if (totalLength == 0)
            {
                return 0;
            }

            length = MathfExtend.Clamp0(length, totalLength);
            return length / totalLength;
        }

        // Get the location of the path based on how far you have traveled to a initial relative point
        public static bool GetPointReltiveByLength(this PearlSpline path, out Vector3 result, float length, Vector3 initPosition)
        {
            if (path == null)
            {
                result = default;
                return false;
            }

            bool isFinish = path.GetPointByLength(out result, ref length);
            Vector3 initPath = path.EvaluatePosition(0);
            result = initPosition + (result - initPath);
            return isFinish;
        }

        // Get the location of the path based on percent to a initial relative point
        public static void GetPointReltive(this PearlSpline path, out Vector3 result, float t, Vector3 initPosition)
        {
            if (path == null)
            {
                result = default;
                return;
            }

            result = path.EvaluatePosition(t);
            Vector3 initPath = path.EvaluatePosition(0);
            result = initPosition + (result - initPath);
        }
#endregion
    }
}

#endif