using UnityEngine;

namespace Pearl
{
    public class TranslateManualyScale : TranslateManualyAbstract
    {
        protected override Vector3 GetCurrentValue()
        {
            return transform.localScale;
        }

        protected override void SetCurrentValue(Vector3 vector)
        {
            transform.localScale = vector;
        }
    }
}
