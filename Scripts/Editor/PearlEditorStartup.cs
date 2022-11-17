using UnityEditor;

namespace Pearl.Editor
{
    [InitializeOnLoad]
    public static class PearlEditorStartup
    {
        static PearlEditorStartup()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
            }
        }
    }
}