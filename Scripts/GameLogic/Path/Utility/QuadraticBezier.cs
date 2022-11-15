using UnityEngine;

namespace Pearl
{
    public static class QuadraticBezier
    {
        #region Quadratic Bezier Simplified
        public static Vector3 GetPoint(Vector3 p0, Vector3 p2, float curveFactor, float t)
        {
            Vector3 p1 = CreateMiddlePoint(p0, p2, curveFactor);

            return GetPoint(p0, p1, p2, t);
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p2, float curveFactor, float t)
        {
            Vector3 p1 = CreateMiddlePoint(p0, p2, curveFactor);

            return GetFirstDerivative(p0, p1, p2, t);
        }

        public static float BezierSingleLength(Vector3 p0, Vector3 p2, float curveFactor)
        {
            Vector3 p1 = CreateMiddlePoint(p0, p2, curveFactor);

            return BezierSingleLength(p0, p1, p2);
        }
        #endregion

        #region Quadratic Bezier
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float OneMinusT = 1f - t;
            return
                OneMinusT * OneMinusT * p0 +
                2f * OneMinusT * t * p1 +
                t * t * p2;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                2f * oneMinusT * (p1 - p0) +
                2f * t * (p2 - p1);
        }

        public static float BezierSingleLength(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            var ax = p0.x - 2 * p1.x + p2.x;
            var ay = p0.y - 2 * p1.y + p2.y;
            var bx = 2 * p1.x - 2 * p0.x;
            var by = 2 * p1.y - 2 * p0.y;
            var A = 4 * (ax * ax + ay * ay);
            var B = 4 * (ax * bx + ay * by);
            var C = bx * bx + by * by;

            var Sabc = 2 * Mathf.Sqrt(A + B + C);
            var A_2 = Mathf.Sqrt(A);
            var A_32 = 2 * A * A_2;
            var C_2 = 2 * Mathf.Sqrt(C);
            var BA = B / A_2;

            return (A_32 * Sabc + A_2 * B * (Sabc - C_2) + (4 * C * A - B * B) *
                    Mathf.Log((2 * A_2 + BA + Sabc) / (BA + C_2))) / (4 * A_32);
        }

        #endregion

        #region Private Methods
        private static Vector3 CreateMiddlePoint(Vector3 p0, Vector3 p2, float curveFactor)
        {
            Vector3 line = p2 - p0;
            Vector3 middlePoint = Vector3.Lerp(p0, p2, 0.5f);
            Vector3 tangent = line.Tangent();
            return middlePoint + (tangent * curveFactor);
        }

        #endregion
    }
}