using Pearl.Multitags;
using UnityEngine;

namespace Pearl
{
    public static class Collision2DExtend
    {
        public static bool HasTags(this Collision2D collision, bool only, params string[] tags)
        {
            return MultiTagsManager.HasTags(collision.gameObject, only, tags);
        }
    }
}
