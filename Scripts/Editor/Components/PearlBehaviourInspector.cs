using UnityEditor;

namespace Pearl.Editor
{
    [CustomEditor(typeof(PearlBehaviour))]
    public class PearlBehaviourInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
    }
}
