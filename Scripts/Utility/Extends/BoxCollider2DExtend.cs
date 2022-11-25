using UnityEngine;

namespace Pearl
{
    public static class BoxCollider2DExtend
    {
        private readonly static Vector2[] vertex = new Vector2[4];

        public static Vector2[] GetBoxPoints2D(this BoxCollider2D box)
        {
            if (box == null)
            {
                return null;
            }

            var size = box.size * 0.5f;

            var mtx = Matrix4x4.TRS(box.bounds.center, box.transform.localRotation, box.transform.localScale);

            vertex[0] = mtx.MultiplyPoint3x4(new Vector3(-size.x, size.y));
            vertex[1] = mtx.MultiplyPoint3x4(new Vector3(-size.x, -size.y));
            vertex[2] = mtx.MultiplyPoint3x4(new Vector3(size.x, -size.y));
            vertex[3] = mtx.MultiplyPoint3x4(new Vector3(size.x, size.y));

            return vertex;
        }

        public static Vector2[] GetEdgesDirectionBoxPoints2D(this BoxCollider2D box, Vector2 dir)
        {
            if (box == null)
            {
                return null;
            }

            GetBoxPoints2D(box);
            var bounds = box.bounds;

            float distance = Mathf.Max(bounds.extents.x, bounds.extents.y);

            Vector2 aux = bounds.center + (Vector3)(dir.normalized * distance);
            Segment segment1 = new(bounds.center, aux);

            bool isIntersection = GeometryUtils.SegmentSegmentIntersection(out _, segment1, new(vertex[0], vertex[1]));
            if (isIntersection)
            {
                return new Vector2[] { vertex[0], vertex[1] };
            }
            isIntersection = GeometryUtils.SegmentSegmentIntersection(out _, segment1, new(vertex[1], vertex[2]));
            if (isIntersection)
            {
                return new Vector2[] { vertex[1], vertex[2] };
            }
            isIntersection = GeometryUtils.SegmentSegmentIntersection(out _, segment1, new(vertex[2], vertex[3]));
            if (isIntersection)
            {
                return new Vector2[] { vertex[2], vertex[3] };
            }

            return new Vector2[] { vertex[3], vertex[0] };
        }
    }
}
