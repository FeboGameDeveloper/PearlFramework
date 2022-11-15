using UnityEngine;

namespace Pearl
{
    public static class Camera2DExtend
    {
        public static bool IsEncapsulated(this Camera camera, Bounds container, Vector3 newValue = default)
        {
            if (IsOrthographic(camera))
            {
                Bounds bounds = camera.OrthographicBounds();
                bounds.center += newValue;
                return bounds.IsEncapsulated(container);
            }
            return false;
        }

        public static Bounds OrthographicBounds(this Camera camera)
        {
            if (IsOrthographic(camera))
            {
                var t = camera.transform;
                var x = t.position.x;
                var y = t.position.y;
                var size = camera.orthographicSize * 2;
                var width = size * (float)Screen.width / Screen.height;
                var height = size;

                return new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));
            }

            return new Bounds();
        }

        public static Vector2 BoundsMin(this Camera camera)
        {
            return (Vector2)camera.transform.position - camera.Extents();
        }

        public static Vector2 BoundsMax(this Camera camera)
        {
            return (Vector2)camera.transform.position + camera.Extents();
        }

        public static Vector2 Extents(this Camera camera)
        {
            if (IsOrthographic(camera))
            {
                return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
            }

            return Vector2.zero;
        }

        private static bool IsOrthographic(Camera camera)
        {
            AssertExtend.PreConditions("camera", camera, "isOrthographic", camera != null && camera.orthographic);

            if (camera == null || !camera.orthographic)
            {
                return false;
            }
            return true;
        }
    }
}
