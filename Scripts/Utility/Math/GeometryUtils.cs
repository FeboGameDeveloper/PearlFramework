using UnityEngine;

namespace Pearl
{
    public static class GeometryUtils
    {
        public static bool LineLineIntersection(out Vector3 intersection, Line line1, Line line2)
        {
            return Math3D.LineLineIntersection(out intersection, line1.point, line1.direction, line2.point, line2.direction);
        }

        public static bool SegmentSegmentIntersection(out Vector3 intersection, Segment segment1, Segment segment2)
        {
            bool result = Math3D.LineLineIntersection(out intersection, segment1.pointStart, segment1.pointFinish - segment1.pointStart, segment2.pointStart, segment2.pointFinish - segment2.pointStart);
            return result && MathfExtend.IsContains(intersection.x, segment1.pointStart.x, segment1.pointFinish.x) && MathfExtend.IsContains(intersection.y, segment1.pointStart.y, segment1.pointFinish.y) && MathfExtend.IsContains(intersection.z, segment1.pointStart.z, segment1.pointFinish.z)
                && MathfExtend.IsContains(intersection.x, segment2.pointStart.x, segment2.pointFinish.x) && MathfExtend.IsContains(intersection.y, segment2.pointStart.y, segment2.pointFinish.y) && MathfExtend.IsContains(intersection.z, segment2.pointStart.z, segment2.pointFinish.z);
        }

        public static bool LineSegmentIntersection(out Vector3 intersection, Line line, Segment segment)
        {
            bool result = Math3D.LineLineIntersection(out intersection, line.point, line.direction, segment.pointStart, segment.pointFinish - segment.pointStart);
            return result && MathfExtend.IsContains(intersection.x, segment.pointStart.x, segment.pointFinish.x) && MathfExtend.IsContains(intersection.y, segment.pointStart.y, segment.pointFinish.y) && MathfExtend.IsContains(intersection.z, segment.pointStart.z, segment.pointFinish.z);
        }

        public static bool LinePlaneIntersection(out Vector3 intersection, Line line, Plane plane)
        {
            return Math3D.LinePlaneIntersection(out intersection, line.point, line.direction, plane.planeNormal, plane.planePoint);
        }

        /// <summary>
        /// Returns the distance between a point and a line.
        /// </summary>
        /// <returns>The between point and line.</returns>
        /// <param name="point">Point.</param>
        /// <param name="lineStart">Line start.</param>
        /// <param name="lineEnd">Line end.</param>
        public static float DistanceBetweenPointAndLine(Vector3 point, Segment segment)
        {
            return Vector3.Magnitude(ProjectPointOnLine(point, segment) - point);
        }

        /// <summary>
        /// Projects a point on a line (perpendicularly) and returns the projected point.
        /// </summary>
        /// <returns>The point on line.</returns>
        /// <param name="point">Point.</param>
        /// <param name="lineStart">Line start.</param>
        /// <param name="lineEnd">Line end.</param>
        public static Vector3 ProjectPointOnLine(Vector3 point, Segment segment)
        {
            Vector3 rhs = point - segment.pointStart;
            Vector3 vector2 = segment.pointFinish - segment.pointStart;
            float magnitude = vector2.magnitude;
            Vector3 lhs = vector2;
            if (magnitude > 1E-06f)
            {
                lhs = (Vector3)(lhs / magnitude);
            }
            float num2 = Mathf.Clamp(Vector3.Dot(lhs, rhs), 0f, magnitude);
            return (segment.pointStart + ((Vector3)(lhs * num2)));
        }

        public static Vector3 ProjectPointOnLineSegment(Segment segment, Vector3 point)
        {
            return Math3D.ProjectPointOnLineSegment(segment.pointStart, segment.pointFinish, point);
        }

        /// <summary>
        /// Computes and returns the angle between two vectors, on a 360° scale
        /// </summary>
        /// <returns>The <see cref="System.Single"/>.</returns>
        /// <param name="vectorA">Vector a.</param>
        /// <param name="vectorB">Vector b.</param>
        public static float Angle2DBetween(Vector2 vectorA, Vector2 vectorB)
        {
            float angle = Vector2.Angle(vectorA, vectorB);
            Vector3 cross = Vector3.Cross(vectorA, vectorB);

            if (cross.z > 0)
            {
                angle = 360 - angle;
            }

            return angle;
        }

        public static bool IsInsideRectangle(float x, float y, float width, float height, float pointX, float pointY)
        {
            return pointX >= x - width * .5f &&
                pointX <= x + width * .5f &&
                pointY >= y - height * .5f &&
                pointY <= y + height * .5f;
        }

        public static bool IsInsideCircle(float x, float y, float radius, float pointX, float pointY)
        {
            return (pointX - x) * (pointX - x) + (pointY - y) * (pointY - y) < radius * radius;
        }
    }
}
