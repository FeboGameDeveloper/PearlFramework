using UnityEngine;
using UnityEngine.UI;

namespace Pearl
{
    public static class ImageExtend
    {
        public static void SetAlpha(this Image @this, float alpha)
        {
            if (@this != null)
            {
                Color color = @this.color;
                color.a = alpha;
                @this.color = color;
            }
        }
    }
}
