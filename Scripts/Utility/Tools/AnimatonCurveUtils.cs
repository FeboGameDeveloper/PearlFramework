using Pearl.Testing;
using System;
using UnityEngine;

namespace Pearl
{
    public enum CurveType { SpecificCurve, Custom }

    [Serializable]
    public class AnimationCurveInfo
    {
        public CurveType curveType;
        [ConditionalField("@curveType == Custom")]
        [SerializeField]
        private AnimationCurve animationCurve;
        [ConditionalField("@curveType == SpecificCurve")]
        [SerializeField]
        private FunctionEnum functionEnum;
        private Func<float, float> specificCurve;

        public FunctionEnum Function
        {
            get { return curveType == CurveType.SpecificCurve ? functionEnum : FunctionEnum.Null; }
            set
            {
                functionEnum = value;
                CreateSpecificCurve();
            }
        }

        public AnimationCurve Curve
        {
            get { return animationCurve; }
        }

        public AnimationCurveInfo(AnimationCurve animationCurve)
        {
            curveType = CurveType.Custom;
            this.animationCurve = animationCurve;
        }

        public AnimationCurveInfo(FunctionEnum functionEnum)
        {
            curveType = CurveType.SpecificCurve;
            this.functionEnum = functionEnum;
            CreateSpecificCurve();
        }

        public void CreateSpecificCurve()
        {
            specificCurve = FunctionDefinition.GetFunction(functionEnum);
        }

        public float Evaluate(float x, bool isPercent = true)
        {
            float result = 0;
            if (curveType == CurveType.Custom)
            {
                if (animationCurve == null)
                {
                    LogManager.LogWarning("This is not initialized");
                }

                result = animationCurve.Evaluate(x);
            }
            else
            {
                if (specificCurve == null)
                {
                    CreateSpecificCurve();
                }

                result = specificCurve.Invoke(x);
            }

            if (isPercent)
            {
                result = Mathf.Clamp01(result);
            }

            return result;
        }
    }
}
