using UnityEditor;
using UnityEditor.UI;

namespace Pearl.UI
{
    [CustomEditor(typeof(PearlSlider))]
    [CanEditMultipleObjects]
    public class PearSliderInspector : SliderEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PearlSlider _slider = (PearlSlider)target;

            EditorGUILayout.LabelField("Sound vars");
            EditorGUILayout.Space();

            _slider.useSound = EditorGUILayout.Toggle("Use Sound", _slider.useSound);

            if (_slider.useSound)
            {
                _slider.useSoundInPause = EditorGUILayout.Toggle("Use Sound In Pause", _slider.useSoundInPause);
            }
        }
    }
}
