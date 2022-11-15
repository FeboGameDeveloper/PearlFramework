using System.IO;
using UnityEditor;

namespace Pearl
{
    public class CreateAssetBundles
    {
        [MenuItem("Assets/Build AssetBundles")]
        public static void BuildAllAssetBundles()
        {
            string assetBundleDirectory = AssetManager.GetAssetBoundlePath();
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }
            BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None,
                BuildTarget.StandaloneWindows);

        }
    }
}