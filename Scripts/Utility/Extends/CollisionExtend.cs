using Pearl.Multitags;
using UnityEngine;

namespace Pearl
{
    public static class CollisionExtend
    {
        public static bool HasTags(this Collision collision, bool only, params string[] tags)
        {
            return MultiTagsManager.HasTags(collision.gameObject, only, tags);
        }
    }
}