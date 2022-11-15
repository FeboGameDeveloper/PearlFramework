using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace Pearl.Editor
{
    [InitializeOnLoad]
    public static class PearlEditorStartup
    {
        private static ListRequest listRequest;
        private static PackageCollection packages;

        static PearlEditorStartup()
        {
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
            }
        }

        #region Packages
        private static void CheckPackage()
        {
            listRequest = Client.List(offlineMode: true);
            EditorApplication.update += ListProgress;
        }

        private static void ListProgress()
        {
            if (listRequest.IsCompleted)
            {
                if (listRequest.Status == StatusCode.Success)
                {
                    packages = listRequest.Result;

                    string[] packagesToVerifed = ArrayExtend.CreateArray("com.unity.localization");

                    var target = EditorUserBuildSettings.activeBuildTarget;
                    var group = BuildPipeline.GetBuildTargetGroup(target);
                    var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(group);


                    PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out string[] currentSymbols);
                    List<string> newSymbols = new(currentSymbols);

                    string symbol;
                    foreach (var package in packagesToVerifed)
                    {
                        if (package == "com.unity.localization")
                        {
                            symbol = "LOCALIZATION";
                            if (ContainsPackage(package) && !currentSymbols.Contains(symbol))
                            {
                                newSymbols.Add(symbol);
                            }
                            else if (!ContainsPackage(package) && currentSymbols.Contains(symbol))
                            {
                                newSymbols.Remove(symbol);
                            }
                        }
                    }

                    PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newSymbols.ToArray());



                }
                else if (listRequest.Status >= StatusCode.Failure)
                {
                    UnityEngine.Debug.Log("Could not check for packages: " + listRequest.Error.message);
                }

                EditorApplication.update -= ListProgress;
            }
        }

        private static bool ContainsPackage(string packageId)
        {
            foreach (var package in packages)
            {
                if (string.Compare(package.name, packageId) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}