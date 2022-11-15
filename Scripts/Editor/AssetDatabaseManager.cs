using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Pearl.Editor
{
    public class AssetDatabaseManager
    {
        private static string _pearlPath;

        public static string PearlPath { get { return _pearlPath; } }


        static AssetDatabaseManager()
        {
            var filter = string.Format("{0} t:script", typeof(AssetDatabaseManager).Name);
            var paths = AssetDatabaseManager.GetPaths(filter);

            if (paths == null)
            {
                return;
            }

            foreach (var p in paths)
            {
                var fi1 = new FileInfo(p);
                var info = new DirectoryInfo(fi1.DirectoryName);

                while (info != null)
                {
                    var files = info.GetFiles("Pearl.asmdef");
                    if (files != null)
                    {
                        _pearlPath = info.FullName;
                        break;
                    }
                    else
                    {
                        info = info.Parent;
                    }
                }
            }
        }

        public static string GetPath(string filter, Predicate<string> match = null)
        {
            var paths = GetPaths(filter, match);
            if (paths.IsAlmostSpecificCount())
            {
                return paths[0];
            }
            return null;
        }

        public static List<string> GetPaths(string filter, Predicate<string> match = null)
        {
            var guids = AssetDatabase.FindAssets(filter);
            List<string> listPaths = new();

            if (guids.Length > 0)
            {
                foreach (var guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    var fi1 = new FileInfo(path);
                    var info = new DirectoryInfo(fi1.DirectoryName);

                    if (match == null || match.Invoke(info.FullName))
                    {
                        listPaths.Add(path);
                    }
                }
            }
            return listPaths;
        }
    }
}
