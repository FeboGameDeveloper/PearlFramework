#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Debug;
using System;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class PearlSetFieldTask : ActionTask<Transform>
    {
        public BBParameter<NameClass> nameClass = default;
        public BBParameter<bool> useAgent = true;
        public BBParameter<PrimitiveEnum> primitiveStruct = default;
        public BBParameter<MemberEnum> memberTypeBB = MemberEnum.Either;

        [Conditional("useAgent", 0)]
        public BBParameter<string> nameRoot = default;

        public BBParameter<string> nameField = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Bool)]
        public BBParameter<bool> boolValue = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Enum)]
        public BBParameter<string> enumValue = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Float)]
        public BBParameter<float> floatValue = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Integer)]
        public BBParameter<int> integerValue = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.String)]
        public BBParameter<string> stringValue = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Vector2)]
        public BBParameter<Vector2> vector2Value = default;

        [Conditional("primitiveStruct", (int)PrimitiveEnum.Vector3)]
        public BBParameter<Vector3> vector3Value = default;

        protected override void OnExecute()
        {
            if (useAgent == null || nameClass == null)
            {
                EndAction();
            }


            GameObject obj;
            if (!useAgent.value && nameRoot != null)
            {
                string root = nameRoot.value;
                obj = GameObject.Find(root);
            }
            else
            {
                obj = agent.gameObject;
            }

            Type type = nameClass.value.GetTypeFromName();

            if (obj != null)
            {
                Component component = obj.GetChildInHierarchy(type);

                if (component != null)
                {
                    MemberEnum memberType = memberTypeBB != null ? memberTypeBB.value : MemberEnum.Either;
                    ReflectionExtend.SetValue(component, nameField.value, GetValue(component));
                }
            }

            EndAction();
        }

        private object GetValue(Component component)
        {
            if (primitiveStruct != null)
            {
                switch (primitiveStruct.value)
                {
                    case PrimitiveEnum.Bool:
                        return boolValue != null && boolValue.value;
                    case PrimitiveEnum.Enum:
                        int index = 0;
                        if (nameField == null || enumValue == null)
                        {
                            LogManager.LogWarning("Enum problem");
                            return index;
                        }

                        if (ReflectionExtend.GetField(component, nameField.value, out object result))
                        {
                            index = EnumExtend.ParseEnum(enumValue.value, result.GetType());
                        }
                        else
                        {
                            LogManager.LogWarning("Enum problem");
                        }
                        return index;
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