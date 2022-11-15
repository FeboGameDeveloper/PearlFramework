#if NODE_CANVAS

using System;

namespace ParadoxNotion.Design
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ConditionalAttribute : DrawerAttribute
    {
        readonly public string fieldName;
        readonly public int[] checkValues;
        public override bool isDecorator { get { return true; } }
        public override int priority { get { return 1; } }
        public ConditionalAttribute(string fieldName, params int[] checkValues)
        {
            this.fieldName = fieldName;
            this.checkValues = checkValues;
        }
    }
}

#endif
