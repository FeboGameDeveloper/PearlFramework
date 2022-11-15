using UnityEditor;
using UnityEditor.UI;

namespace Pearl.UI
{
    [CustomEditor(typeof(PearlScrollRect))]
    [CanEditMultipleObjects]
    public class PearlScrollRectInspector : ScrollRectEditor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            base.DrawDefaultInspector();
            PearlScrollRect _scrollRect = (PearlScrollRect)target;
        }
    }
}
