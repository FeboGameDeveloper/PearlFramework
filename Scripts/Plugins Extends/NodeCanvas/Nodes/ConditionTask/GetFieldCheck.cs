#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
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

        public BBParameter<MemberEnum> memberTypeBB = MemberEnum.Either;

        public BBParameter<string> nameField = default;

        protected override string info { get { return "[" + nameField.value + "]"; } }

        protected override bool OnCheck()
        {
            if (useAgent == null || nameClass == null)
            {
                return false;
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
                MemberEnum memberType = memberTypeBB != null ? memberTypeBB.value : MemberEnum.Either;
                if (component != null)
                {
                    ReflectionExtend.GetValue(component, nameField.value, out var result, memberType);
                    if (result != null && result is Boolean boolResult)
                    {
                        return boolResult;
                    }
                }
            }

            return false;
        }
    }
}

#endif