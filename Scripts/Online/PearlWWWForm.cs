using UnityEngine;

namespace Pearl
{
    public static class PearlWWWForm
    {
        public static void AddBool(this WWWForm @this, string fieldName, bool value)
        {
            if (@this == null)
            {
                return;
            }

            string result = value ? "1" : "0";
            @this.AddField(fieldName, result);
        }
    }
}
