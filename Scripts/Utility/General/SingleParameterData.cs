using System;

namespace Pearl
{
    [Serializable]
    public struct SingleParameterData
    {
        public TypeParameter typeParameter;

        [ConditionalField("@typeParameter == String")]
        public string stringParameter;
        [ConditionalField("@typeParameter == Float")]
        public float floatParameter;
        [ConditionalField("@typeParameter == Integer")]
        public int integerParameter;
        [ConditionalField("@typeParameter == Boolean")]
        public bool booleanParameter;
        [ConditionalField("@typeParameter == Various")]
        public object parameter;

        public object Get()
        {
            switch (typeParameter)
            {
                case TypeParameter.Boolean:
                    return booleanParameter;
                case TypeParameter.String:
                    return stringParameter;
                case TypeParameter.Float:
                    return floatParameter;
                case TypeParameter.Integer:
                    return integerParameter;
                default:
                    return parameter;
            }
        }
    }
}
