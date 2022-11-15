using UnityEngine;

namespace Pearl
{
    public static class SpriteRenderExtend
    {
        public static int MaxSortingLayer { get { return 32767; } }

        public static void ChangeAlpha(this SpriteRenderer @this, float alpha)
        {
            if (@this)
            {
                Color color = @this.color;
                color.a = alpha;
                @this.color = color;
            }
        }
    }
}
