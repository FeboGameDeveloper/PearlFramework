using TMPro.EditorUtilities;
using UnityEditor;

namespace Pearl.UI
{
    [CustomEditor(typeof(PearlInputField))]
    [CanEditMultipleObjects]
    public class PearlInputFieldInspector : TMP_InputFieldEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PearlInputField _field = (PearlInputField)target;

            EditorGUILayout.LabelField("Navigation");
            EditorGUILayout.LabelField("Reader Numeric Info");
            EditorGUI.indentLevel++;
            _field.axisUtility.InputString = EditorGUILayout.TextField("Input String", _field.axisUtility.InputString);
            _field.axisUtility.IsVector = EditorGUILayout.Toggle("Is Vector", _field.axisUtility.IsVector);
            _field.axisUtility.AxisEnum = (Axis2DEnum)EditorGUILayout.EnumPopup("Axis Enum", _field.axisUtility.axisEnum);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _field.nextTextButton = EditorGUILayout.TextField("Next Text Button", _field.nextTextButton);
            _field.clearOnDisactive = EditorGUILayout.Toggle("Clear On Disactive", _field.clearOnDisactive);
            _field.isVector = EditorGUILayout.Toggle("Is Vector", _field.isVector);
            _field.nextAxis = (SemiAxis2DEnum)EditorGUILayout.EnumPopup("Next Axis", _field.nextAxis);

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Sound vars");
            _field.useSound = EditorGUILayout.Toggle("Use Sound", _field.useSound);

            if (_field.useSound)
            {
                _field.useSoundInPause = EditorGUILayout.Toggle("Use Sound In Pause", _field.useSoundInPause);
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Other Setting");
            _field.useAutoSizeFont = EditorGUILayout.Toggle("Use Auto Size Font", _field.useAutoSizeFont);
            if (_field.useAutoSizeFont)
            {
                _field.minSizeFont = EditorGUILayout.FloatField("Min Size Font", _field.minSizeFont);
            }
        }
    }
}