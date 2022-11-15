#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;

namespace Pearl.NodeCanvas
{
    [Serializable]
    public class SingleParameterData
    {
        public BBParameter<TypeParameter> typeParameter;

        [Conditional("typeParameter", (int)TypeParameter.String)]
        public BBParameter<string> stringParameter;
        [Conditional("typeParameter", (int)TypeParameter.Float)]
        public BBParameter<float> floatParameter;
        [Conditional("typeParameter", (int)TypeParameter.Integer)]
        public BBParameter<int> integerParameter;
        [Conditional("typeParameter", (int)TypeParameter.Boolean)]
        public BBParameter<bool> booleanParameter;
        [Conditional("typeParameter", (int)TypeParameter.Various)]
        public BBParameter<object> parameter;

        public object Get()
        {
            if (typeParameter == null)
            {
                return default;
            }

            switch (typeParameter.value)
            {
                case TypeParameter.Boolean:
                    return booleanParameter != null ? booleanParameter.value : default;
                case TypeParameter.String:
                    return stringParameter != null ? stringParameter.value : default;
                case TypeParameter.Float:
                    return floatParameter != null ? floatParameter.value : default;
                case TypeParameter.Integer:
                    return integerParameter != null ? integerParameter.value : default;
                default:
                    return parameter != null ? parameter.value : default;
            }
        }
    }
}

#endif
