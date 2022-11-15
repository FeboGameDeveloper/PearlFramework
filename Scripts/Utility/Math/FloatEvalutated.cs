using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class FloatEvalutated
    {
        [SerializeField]
        private FunctionEnum functionEnum;

        private float _rawValue;
        private float _value;
        private Func<float, float> _function;

        public FloatEvalutated(FunctionEnum functionEnumParam)
        {
            FunctionEnum = functionEnumParam;
            RawValue = 0;
        }

        public FloatEvalutated()
        {
            RawValue = 0;
        }

        private FunctionEnum FunctionEnum
        {
            set
            {
                functionEnum = value;
                _function = FunctionDefinition.GetFunction(functionEnum);
            }
        }

        public float RawValue
        {
            get { return _rawValue; }
            set
            {
                _rawValue = value;

                if (_function == null)
                {
                    FunctionEnum = functionEnum;
                }
                _value = _function.Invoke(_rawValue);
            }
        }

        public float Value
        {
            get { return _value; }
        }
    }
}
