#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class GetFieldCheck : ConditionTask
    {
        public BBParameter<NameClass> nameClass = default;
        public BBParameter<bool> useAgent = true;
        public BBParameter<PrimitiveEnum> primitiveStruct = default;

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

        protected override string info { get { return "[" + "]"; } }

        protected override bool OnCheck()
        {
            return false;
        }
    }
}

#endif