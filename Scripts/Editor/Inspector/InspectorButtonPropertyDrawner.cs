using Pearl.Debug;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Pearl
{
    [CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonPropertyDrawer : PropertyDrawer
    {
        private MethodInfo _eventMethodInfo = null;

        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            InspectorButtonAttribute inspectorButtonAttribute = (InspectorButtonAttribute)attribute;
            Rect buttonRect = new Rect(position.x + (position.width - inspectorButtonAttribute.ButtonWidth) * 0.5f, position.y, inspectorButtonAttribute.ButtonWidth, position.height);
            if (GUI.Button(buttonRect, label.text))
            {
                string eventName = inspectorButtonAttribute.MethodName;

                System.Type eventOwnerType = prop.serializedObject.targetObject.GetType();

                while (_eventMethodInfo == null && eventOwnerType != null)
                {
                    _eventMethodInfo = eventOwnerType.GetMethod(eventName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    eventOwnerType = eventOwnerType.BaseType;

                }

                if (_eventMethodInfo != null)
                {
                    _eventMethodInfo.Invoke(prop.serializedObject.targetObject, null);
                }
                else
                {
                    LogManager.LogWarning(string.Format("InspectorButton: Unable to find method {0} in {1}", eventName, eventOwnerType));
                }
            }
        }
    }
}
