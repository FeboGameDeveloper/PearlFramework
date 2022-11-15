using UnityEngine;

namespace Pearl
{
    public static class GeometryUtils
    {
        public static bool LineLineIntersection(out Vector3 intersection, Line line1, Line line2)
        {
            return Math3D.LineLineIntersection(out intersection, line1.point, line1.direction, line2.point, line2.direction);
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
            return Vector3.Magnitude(ProjectPointOnLineSegment(segment, point) - point);
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
    }
}
