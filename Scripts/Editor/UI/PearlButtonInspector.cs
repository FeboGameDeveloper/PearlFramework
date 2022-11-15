using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Pearl.UI
{
    [CustomEditor(typeof(PearlButton))]
    [CanEditMultipleObjects]
    public class PearlButtonInspector : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PearlButton _button = (PearlButton)target;

            EditorGUILayout.LabelField("Complex Click");
            EditorGUILayout.Space();

            if (!_button.useDelayForClick)
            {
                _button.useDoubleClick = EditorGUILayout.Toggle("Use Double Click", _button.useDoubleClick);
            }

            if (!_button.useDoubleClick)
            {
                _button.useDelayForClick = EditorGUILayout.Toggle("Use Delay For Click", _button.useDelayForClick);
                if (_button.useDelayForClick)
                {
                    _button.delayForClick = EditorGUILayout.FloatField("Time For Delay", _button.delayForClick);

                    _button.fillObject = (GameObject)EditorGUILayout.ObjectField("Fill Object", _button.fillObject, typeof(GameObject), true);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Sound vars");
            EditorGUILayout.Space();

            _button.useSound = EditorGUILayout.Toggle("Use Sound", _button.useSound);

            if (_button.useSound)
            {
                _button.useBackSound = EditorGUILayout.Toggle("Use Back Sound", _button.useBackSound);
                _button.useSoundInPause = EditorGUILayout.Toggle("Use Sound In Pause", _button.useSoundInPause);
            }
        }
    }
}
