using TMPro;
using UnityEngine;

namespace Pearl
{
    public static class TMPTextExtend
    {
        public static void SetAlpha(this TMP_Text @this, float alpha)
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
