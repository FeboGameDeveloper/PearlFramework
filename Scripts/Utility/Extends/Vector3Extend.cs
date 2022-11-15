using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct FilterAxis3D
    {
        public bool useX;
        public bool useY;
        public bool useZ;
    }

    [Serializable]
    public struct Line
    {
        public Vector3 point;
        public Vector3 direction;

        public Line(Vector3 point, Vector3 direction)
        {
            this.point = point;
            this.direction = direction.normalized;
        }
    }

    [Serializable]
    public struct Segment
    {
        public Vector3 pointStart;
        public Vector3 pointFinish;

        public Segment(Vector3 pointStart, Vector3 pointFinish)
        {
            this.pointStart = pointStart;
            this.pointFinish = pointFinish;
        }
    }

    public struct Plane
    {
        public Vector3 planeNormal;
        public Vector3 planePoint;

        public Plane(Vector3 planePoint, Vector3 planeNormal)
        {
            this.planePoint = planePoint;
            this.planeNormal = planeNormal.normalized;
        }
    }

    public static class Vector3Extend
    {
        public static bool ApproxZero(this Vector3 vector)
        {
            return vector.x.ApproxZero() && vector.y.ApproxZero() && vector.z.ApproxZero();
        }

        public static bool Approx(Vector3 vectorA, Vector3 vectorB)
        {
            return vectorA.x.Approx(vectorB.x) && vectorA.y.Approx(vectorB.y) && vectorA.z.Approx(vectorB.z);
        }

        public static Vector3 Clamp(Vector3 vector, Range rangeX)
        {
            vector.x = MathfExtend.Clamp(vector.x, rangeX);
            return vector;
        }

        public static Vector3 Clamp(Vector3 vector, Range rangeX, Range rangeY)
        {
            vector.x = MathfExtend.Clamp(vector.x, rangeX);
            vector.y = MathfExtend.Clamp(vector.y, rangeY);
            return vector;
        }

        public static Vector3 Clamp(Vector3 vector, Range rangeX, Range rangeY, Range rangeZ)
        {
            vector.x = MathfExtend.Clamp(vector.x, rangeX);
            vector.y = MathfExtend.Clamp(vector.y, rangeY);
            vector.z = MathfExtend.Clamp(vector.z, rangeZ);
            return vector;
        }

        public static Vector3 Clamp(Vector3 vector, Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ)
        {
            vector.x = MathfExtend.Clamp(vector.x, rangeX);
            vector.y = MathfExtend.Clamp(vector.y, rangeY);
            vector.z = MathfExtend.Clamp(vector.z, rangeZ);
            return vector;
        }

        public static Vector3 Lerp(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ, float t, FunctionEnum function = FunctionEnum.Linear)
        {
            return Lerp(rangeX, rangeY, rangeZ, Vector3.one * t, function);
        }

        public static Vector3 Lerp(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ, Vector3 t, FunctionEnum function = FunctionEnum.Linear)
        {
            Vector3 vector;
            vector.x = MathfExtend.Lerp(rangeX.x, rangeX.y, t.x, function);
            vector.y = MathfExtend.Lerp(rangeY.x, rangeY.y, t.y, function);
            vector.z = MathfExtend.Lerp(rangeZ.x, rangeZ.y, t.z, function);
            return vector;
        }

        public static Vector3 Lerp(float t, params Vector3[] vectors)
        {
            if (vectors.Length == 1)
            {
                return vectors[0];
            }
            else if (vectors.Length > 1)
            {
                int keysNum = vectors.Length;
                float delta = 1f / (keysNum - 1);
                int piece = t == 1 ? keysNum - 2 : Mathf.FloorToInt(t / delta);

                float r = MathfExtend.Percent(t - (delta * piece), delta);

                return Vector3.Lerp(vectors[piece], vectors[piece + 1], r);
            }
            else
            {
                return Vector3.zero;
            }
        }

        public static Vector3 ChangeVector(Vector3 vector, in Vector3 newValue, in ChangeTypeEnum changeTypeTransform, in AxisCombined axisCombined = AxisCombined.XYZ)
        {
            switch (axisCombined)
            {
                case AxisCombined.X:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    break;
                case AxisCombined.Y:
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    break;
                case AxisCombined.Z:
                    vector.z = MathfExtend.ChangeValue(vector.z, newValue.z, changeTypeTransform);
                    break;
                case AxisCombined.XY:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    break;
                case AxisCombined.XZ:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    vector.z = MathfExtend.ChangeValue(vector.z, newValue.z, changeTypeTransform);
                    break;
                case AxisCombined.YZ:
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    vector.z = MathfExtend.ChangeValue(vector.z, newValue.z, changeTypeTransform);
                    break;
                case AxisCombined.XYZ:
                    vector.x = MathfExtend.ChangeValue(vector.x, newValue.x, changeTypeTransform);
                    vector.y = MathfExtend.ChangeValue(vector.y, newValue.y, changeTypeTransform);
                    vector.z = MathfExtend.ChangeValue(vector.z, newValue.z, changeTypeTransform);
                    break;
            }

            return vector;
        }

        public static Vector3 ChangeVector(Vector3 vector, AxisCombined axisEnum, float newValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            range = range == Vector2.zero ? new Vector2(float.MinValue, float.MaxValue) : range;

            if (axisEnum == AxisCombined.X)
            {
                vector.x = MathfExtend.ChangeValue(vector.x, newValue, changeTypeTransform, range.x, range.y);
            }
            else if (axisEnum == AxisCombined.Y)
            {
                vector.y = MathfExtend.ChangeValue(vector.y, newValue, changeTypeTransform, range.x, range.y);
            }
            else
            {
                vector.z = MathfExtend.ChangeValue(vector.z, newValue, changeTypeTransform, range.x, range.y);
            }
            return vector;
        }

        public static Vector3 Tangent(this Vector3 vector)
        {
            Vector3 normal = vector.normalized;
            Vector3 tangent = Vector3.Cross(normal, Vector3.forward);
            if (tangent.magnitude == 0)
            {
                tangent = Vector3.Cross(normal, Vector3.up);
            }

            return tangent;
        }
    }
}