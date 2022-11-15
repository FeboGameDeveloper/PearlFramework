using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pearl.Editor
{
    public static class BuildVersionManager
    {
        public static void OnBuild()
        {
            var options = EditorExtend.GetInstance<BuildOptionsScriptableObject>();
            bool augmentPreBuild = false;
            if (options != null)
            {
                augmentPreBuild = options.AugmentPreBuild;
            }

            if (augmentPreBuild)
            {
                BuildVersionManager.AugumentVersion(2);
            }
        }

        public static void AugumentVersion(int level = 2)
        {
            AugmentBundleVersionCode();
            AugmentBundleVersion(level);
        }

        private static void AugmentBundleVersionCode()
        {
            PlayerSettings.Android.bundleVersionCode += 1;
        }

        private static void AugmentBundleVersion(int level)
        {
            level = Mathf.Clamp(level, 0, 2);
            string newVersion = "";

            string version = PlayerSettings.bundleVersion;
            version = version.Trim();
            var versions = version.Split(".");
            bool error = false;

            if (versions.IsAlmostSpecificCount())
            {
                var _gameVersion = new List<int>();
                for (int i = 0; i < versions.Length; i++)
                {
                    if (int.TryParse(versions[i], out int result))
                    {
                        _gameVersion.Add(result);
                    }
                    else
                    {
                        error = true;
                        break;
                    }
                }

                if (!error)
                {
                    var difference = 3 - _gameVersion.Count;
                    for (int i = 0; i < Mathf.Abs(difference); i++)
                    {
                        if (difference > 0)
                        {
                            _gameVersion.Add(0);
                        }
                        else
                        {
                            _gameVersion.RemoveTail();
                        }
                    }


                    _gameVersion[level] += 1;

                    for (int i = 0; i < _gameVersion.Count; i++)
                    {
                        if (i > level)
                        {
                            _gameVersion[i] = 0;
                        }
                    }

                    for (int i = 0; i < _gameVersion.Count; i++)
                    {
                        newVersion += _gameVersion[i];
                        if (i != _gameVersion.Count - 1)
                        {
                            newVersion += ".";
                        }
                    }
                }
            }
            else
            {
                newVersion = "0.0.1";
            }

            PlayerSettings.bundleVersion = newVersion;
            PlayerSettings.iOS.buildNumber = newVersion;
        }
    }
}
