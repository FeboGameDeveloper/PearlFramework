using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct FilterAxis2D
    {
        public bool useX;
        public bool useY;
    }

    public static class Vector2Extend
    {
        public static Vector2 ChangeVector(this Vector2 vector, in Vector2 newValue, in ChangeTypeEnum changeTypeTransform, in Axis2DCombined axisCombined = Axis2DCombined.XY)
        {
            switch (axisCombined)
            {
                case Axis2DCombined.X:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    break;
                case Axis2DCombined.Y:
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    break;
                case Axis2DCombined.XY:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    break;
            }

            return vector;
        }

        public static float CrossProduct2D(Vector2 a, Vector2 b)
        {
            return a.x * b.y - b.x * a.y;
        }

        public static Vector2 ChangeVector(this Vector2 vector, Axis2DEnum axisEnum, float newValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            range = range == Vector2.zero ? new Vector2(float.MinValue, float.MaxValue) : range;

            if (axisEnum == Axis2DEnum.X)
            {
                vector.x = MathfExtend.ChangeValue(vector.x, newValue, changeTypeTransform, range.x, range.y);
            }
            else
            {
                vector.y = MathfExtend.ChangeValue(vector.y, newValue, changeTypeTransform, range.x, range.y);
            }
            return vector;
        }

        public static bool ApproxZero(this Vector2 vector)
        {
            return vector.x.ApproxZero() && vector.y.ApproxZero();
        }

        public static bool Approx(Vector2 vectorA, Vector2 vectorB)
        {
            return vectorA.x.Approximately(vectorB.x) && vectorA.y.Approximately(vectorB.y);
        }


        //è più efficente di Distance
        public static float DistancePow2(Vector2 pointA, Vector2 pointB)
        {
            return DistanceVector(pointA, pointB).magnitude;
        }

        public static Vector2 Direction(Vector2 pointA, Vector2 pointB)
        {
            return DistanceVector(pointA, pointB).normalized;
        }

        public static Vector2 Sign(Vector2 point)
        {
            MathfExtend.Sign(point.x);
            MathfExtend.Sign(point.y);
            return point;
        }

        public static Vector2 ConvertAxisXY(SemiAxis2DEnum axisEnum)
        {
            if (axisEnum == SemiAxis2DEnum.Down)
            {
                return Vector2.down;
            }
            if (axisEnum == SemiAxis2DEnum.Left)
            {
                return Vector2.left;
            }
            if (axisEnum == SemiAxis2DEnum.Up)
            {
                return Vector2.up;
            }

            return Vector2.right;
        }

        public static Vector2 DistanceVector(Vector2 pointA, Vector2 pointB)
        {
            return pointB - pointA;
        }

        public static Vector2 Clamp(Vector2 vector, Range rangeX, Range rangeY)
        {
            vector.x = MathfExtend.Clamp(vector.x, rangeX);
            vector.y = MathfExtend.Clamp(vector.y, rangeY);
            return vector;
        }

        public static Vector2 Lerp(Vector2 rangeX, Vector2 rangeY, float t, FunctionEnum function = FunctionEnum.Linear)
        {
            return Lerp(rangeX, rangeY, Vector2.one * t, function);
        }

        public static Vector3 Lerp(Vector2 rangeX, Vector2 rangeY, Vector2 t, FunctionEnum function = FunctionEnum.Linear)
        {
            Vector2 vector;
            vector.x = MathfExtend.Lerp(rangeX.x, rangeX.y, t.x, function);
            vector.y = MathfExtend.Lerp(rangeY.x, rangeY.y, t.y, function);
            return vector;
        }

        public static float Tan(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        public static float Sin(Vector2 vector)
        {
            return vector.y;
        }

        public static float Cos(Vector2 vector)
        {
            return vector.x;
        }
    }
}
