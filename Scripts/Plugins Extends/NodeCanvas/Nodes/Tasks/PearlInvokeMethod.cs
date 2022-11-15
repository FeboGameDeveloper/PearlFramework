#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class PearlInvokeMethod : ActionTask<Transform>
    {
        public BBParameter<NameClass> nameClassBB = default;
        public BBParameter<bool> useAgentBB = true;
        public BBParameter<bool> useParameterBB = false;
        [Conditional("useParameterBB", 1)]
        public BBParameter<PrimitiveEnum> primitiveStructBB = default;

        [Conditional("useAgentBB", 0)]
        public BBParameter<string> nameRoot = default;

        public BBParameter<string> nameMethod = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Bool)]
        public BBParameter<bool> boolValue = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Enum)]
        public BBParameter<string> enumValue = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Float)]
        public BBParameter<float> floatValue = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Integer)]
        public BBParameter<int> integerValue = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.String)]
        public BBParameter<string> stringValue = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Vector2)]
        public BBParameter<Vector2> vector2Value = default;

        [Conditional("useParameterBB", 1)]
        [Conditional("primitiveStructBB", (int)PrimitiveEnum.Vector3)]
        public BBParameter<Vector3> vector3Value = default;

        protected override void OnExecute()
        {
            if (useAgentBB == null || nameClassBB == null)
            {
                EndAction();
            }


            GameObject obj;
            if (!useAgentBB.value && nameRoot != null)
            {
                string root = nameRoot.value;
                obj = GameObject.Find(root);
            }
            else
            {
                obj = agent.gameObject;
            }

            Type type = nameClassBB.value.GetTypeFromName();

            if (obj != null)
            {
                Component component = obj.GetChildInHierarchy(type);

                if (component != null)
                {
                    var method = component.GetType().GetMethod(nameMethod.value, ReflectionExtend.FLAGS_ALL);

                    object[] parameters = null;
                    if (useParameterBB != null && useParameterBB.value)
                    {
                        parameters = new object[1];
                        parameters[0] = GetParameter();
                    }

                    method.Invoke(component, parameters);
                }
            }

            EndAction();
        }

        private object GetParameter()
        {
            if (primitiveStructBB != null)
            {
                switch (primitiveStructBB.value)
                {
                    case PrimitiveEnum.Bool:
                        return boolValue != null && boolValue.value;
                    case PrimitiveEnum.Enum:
                        return integerValue != null ? integerValue.value : 0;
                    case PrimitiveEnum.Float:
                        return floatValue != null ? floatValue.value : 0f;
                    case PrimitiveEnum.Integer:
                        return integerValue != null ? integerValue.value : 0;
                    case PrimitiveEnum.Vector2:
                        return vector2Value != null ? vector2Value.value : Vector2.zero;
                    case PrimitiveEnum.Vector3:
                        return vector3Value != null ? vector3Value.value : Vector3.zero;
                    case PrimitiveEnum.String:
                        return stringValue != null ? stringValue.value : "";
                }
            }
            return null;
        }
    }
}

#endif