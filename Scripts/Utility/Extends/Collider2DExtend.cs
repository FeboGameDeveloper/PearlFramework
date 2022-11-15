using UnityEngine;

namespace Pearl
{
    public static class Collider2DExtend
    {
        public static bool IsTouching(this Collider2D collider1, Collider2D collider2)
        {
            if (collider1 != null && collider2 != null)
            {
                return collider1.bounds.Intersects(collider2.bounds);
            }
            return false;
        }
    }
}
