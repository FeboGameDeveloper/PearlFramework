#if UNITY_EDITOR && NODE_CANVAS

using NodeCanvas.Framework;
using UnityEngine;

namespace ParadoxNotion.Design
{
    public class ConditionalDrawer : AttributeDrawer<ConditionalAttribute>
    {
        public override object OnGUI(GUIContent content, object instance)
        {
            var targetField = context.GetType().RTGetField(attribute.fieldName);
            if (targetField != null)
            {
                //var fieldValue = System.Convert.ChangeType(targetField.GetValue(context), typeof(int));
                var fieldValue = targetField.GetValue(context);
                int integer = 0;
                if (fieldValue is BBParameter parameterContainer)
                {
                    integer = (int)System.Convert.ChangeType(parameterContainer.value, typeof(int));
                }
                else
                {
                    integer = (int)System.Convert.ChangeType(fieldValue, typeof(int));
                }

                bool isDifferent = true;
                foreach (int index in attribute.checkValues)
                {
                    if (integer == index)
                    {
                        isDifferent = false;
                        break;
                    }
                }

                if (isDifferent)
                {
                    return instance; //return instance without any editor (thus hide it)
                }
            }
            return MoveNextDrawer();
        }
    }
}

#endif
