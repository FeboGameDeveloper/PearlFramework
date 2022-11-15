using UnityEngine;

namespace Pearl
{
    public static class ColliderExtend
    {
        public static bool IsTouching(this Collider collider1, Collider collider2)
        {
            if (collider1 != null && collider2 != null)
            {
                return collider1.bounds.Intersects(collider2.bounds);
            }
            return false;
        }

        public static bool IsTouching(this Collider collider1, Collider2D collider2)
        {
            if (collider1 != null && collider2 != null)
            {
                return collider1.bounds.Intersects(collider2.bounds);
            }
            return false;
        }
    }
}
