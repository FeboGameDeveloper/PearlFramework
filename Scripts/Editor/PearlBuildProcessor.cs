using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Pearl.Editor
{
    //Start before build game
    class PearlBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildVersionManager.OnBuild();
        }
    }
}
