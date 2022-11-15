using UnityEngine;

namespace Pearl
{
    public static class CubicBezier
    {
        public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float OneMinusT = 1f - t;
            return
                OneMinusT * OneMinusT * OneMinusT * p0 +
                3f * OneMinusT * OneMinusT * t * p1 +
                3f * OneMinusT * t * t * p2 +
                t * t * t * p3;
        }

        public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                3f * oneMinusT * oneMinusT * (p1 - p0) +
                6f * oneMinusT * t * (p2 - p1) +
                3f * t * t * (p3 - p2);
        }

        public static float BezierSingleLength(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var _p0 = p0 - p1;
            var _p1 = p2 - p1;
            var _p2 = new Vector3();
            var _p3 = p3 - p2;

            var l0 = _p0.magnitude;
            var l1 = _p1.magnitude;
            var l3 = _p3.magnitude;
            if (l0 > 0) _p0 /= l0;
            if (l1 > 0) _p1 /= l1;
            if (l3 > 0) _p3 /= l3;

            _p2 = -_p1;
            var a = Mathf.Abs(Vector3.Dot(_p0, _p1)) + Mathf.Abs(Vector3.Dot(_p2, _p3));
            if (a > 1.98f || l0 + l1 + l3 < (4 - a) * 8) return l0 + l1 + l3;

            var bl = new Vector3[4];
            var br = new Vector3[4];

            bl[0] = p0;
            bl[1] = (p0 + p1) * 0.5f;

            var mid = (p1 + p2) * 0.5f;

            bl[2] = (bl[1] + mid) * 0.5f;
            br[3] = p3;
            br[2] = (p2 + p3) * 0.5f;
            br[1] = (br[2] + mid) * 0.5f;
            br[0] = (br[1] + bl[2]) * 0.5f;
            bl[3] = br[0];

            return BezierSingleLength(bl[0], bl[1], bl[2], bl[3]) + BezierSingleLength(br[0], br[1], br[2], br[3]);
        }
    }
}