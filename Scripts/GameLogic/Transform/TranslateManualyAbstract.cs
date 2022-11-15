using UnityEngine;

namespace Pearl
{
    public abstract class TranslateManualyAbstract : MonoBehaviour
    {
        [SerializeField]
        private bool useLimitX = false;

        [SerializeField]
        private bool useLimitY = false;

        [SerializeField]
        private bool useLimitZ = false;

        [SerializeField]
        private bool isPercent = false;

        [ConditionalField("@useLimitX")]
        [SerializeField]
        private Range rangeX = null;

        [ConditionalField("@useLimitY")]
        [SerializeField]
        private Range rangeY = null;

        [ConditionalField("@useLimitZ")]
        [SerializeField]
        private Range rangeZ = null;

        private void Awake()
        {
            if (rangeX != null && rangeY != null && rangeZ != null)
            {
                Vector3 _initPosition = GetCurrentValue();
                if (useLimitX)
                {
                    rangeX.Set(_initPosition.x - rangeX.Min, _initPosition.x + rangeX.Max);
                }
                if (useLimitY)
                {
                    rangeY.Set(_initPosition.y - rangeY.Min, _initPosition.y + rangeY.Max);
                }
                if (useLimitZ)
                {
                    rangeZ.Set(_initPosition.z - rangeZ.Min, _initPosition.z + rangeZ.Max);

                }
            }
        }

        public void Translate(Vector3 translateVector, FunctionEnum function = FunctionEnum.Linear)
        {
            Vector3 newVector = GetCurrentValue() + translateVector;

            if (useLimitX)
            {
                newVector.x = isPercent ? MathfExtend.Lerp(rangeX, translateVector.x, function) : MathfExtend.Clamp(newVector.x, rangeX);
            }
            if (useLimitY)
            {
                newVector.y = isPercent ? MathfExtend.Lerp(rangeY, translateVector.y, function) : MathfExtend.Clamp(newVector.y, rangeY);
            }
            if (useLimitZ)
            {
                newVector.z = isPercent ? MathfExtend.Lerp(rangeZ, translateVector.z, function) : MathfExtend.Clamp(newVector.z, rangeZ);
            }

            SetCurrentValue(newVector);
        }


        protected abstract Vector3 GetCurrentValue();

        protected abstract void SetCurrentValue(Vector3 vector);
    }
}
