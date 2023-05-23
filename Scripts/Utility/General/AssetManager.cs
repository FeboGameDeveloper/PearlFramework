using Pearl.Testing;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Pearl
{
    public static class AssetManager
    {
        #region Private Fields
        private readonly static string assetBundlestring = "AssetBundles";
        private readonly static string assetFolderString = "Assets";

        #endregion

        #region Public Methods
        static AssetManager()
        {
        }

        public static string GetAssetBoundlePath()
        {
            return CombinePath(assetFolderString, assetBundlestring);
        }

        public static void DestroyDirectory(in string filePath, in bool recursive)
        {
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, recursive);
            }
        }

        public static string CombinePath(params object[] paths)
        {
            if (!paths.IsAlmostSpecificCount())
            {
                return null;
            }

            string completePath = paths[0]?.ToString();
            if (paths.Length != 1)
            {
                for (int i = 1; i < paths.Length; i++)
                {
                    completePath = Path.Combine(completePath, paths[i]?.ToString());
                }
            }

            return completePath;
        }

        public static void RenameDirectory(in object filePath, in object newFilePath)
        {
            string filePathString = filePath.ToString();
            if (Directory.Exists(filePathString))
            {
                string pathParent = Directory.GetParent(filePathString).FullName;
                Directory.Move(filePathString, CombinePath(pathParent, newFilePath));
            }
        }

        public static void CreateSubDirectoriesForPath(in string filePath)
        {
            string parent = Directory.GetParent(filePath).FullName;
            List<string> newDirectories = new();
            if (newDirectories != null)
            {
                while (!Directory.Exists(parent))
                {
                    newDirectories.Add(parent);
                    parent = Directory.GetParent(parent).FullName;
                }

                foreach (string newDirctory in newDirectories)
                {
                    Directory.CreateDirectory(newDirctory);
                }
            }
        }

        public static T GetAssetBoundleAsset<T>(string nameAssetBoundle, string nameAsset) where T : UnityEngine.Object
        {
            if (nameAssetBoundle != null && nameAsset != null)
            {
                var url = Path.Combine(AssetManager.GetAssetBoundlePath(), nameAssetBoundle);
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(url);
                if (myLoadedAssetBundle == null)
                {
                    LogManager.Log("Failed to load AssetBundle!");
                    return default;
                }
                T prefab = myLoadedAssetBundle.LoadAsset<T>(nameAsset);
                myLoadedAssetBundle.Unload(false);
                return prefab;
            }
            return null;
        }

        public static T LoadAssetFromPath<T>(in string path) where T : class
        {
            if (path == null)
            {
                return null;
            }

            var aux = Resources.Load(path);
            return aux as T;
        }

        public static Sprite[] LoadSpritesFromPath(in string path)
        {
            if (path == null)
            {
                return null;
            }

            Texture2D[] aux = Resources.LoadAll<Texture2D>(path);

            if (aux != null)
            {
                return aux.CreateSprites();
            }

            return null;
        }

        public static Sprite LoadSpriteFromPath(in string path)
        {
            if (path == null)
            {
                return null;
            }

            Texture2D aux = Resources.Load(path) as Texture2D;

            if (aux != null)
            {
                return aux.CreateSprite();
            }

            return null;
        }

        public static T LoadAsset<T>(in string nameAsset) where T : class
        {
            string path = "";
            if (UtilityMethods.IsSubClass<T, ScriptableObject>())
            {
                path = "ScriptableObjects";
            }

            string fullName = CombinePath(path, nameAsset);
            if (nameAsset != null)
            {
                if (typeof(T).IsArray)
                {
                    Type elementType = typeof(T).GetElementType();
                    object[] result = Resources.LoadAll(fullName, elementType);
                    return (T)(object)ArrayExtend.ConvertArray(result, elementType);
                }
                else
                {
                    return Resources.Load(fullName) as T;
                }
            }
            return null;
        }

        public static GameObject LoadPrefab(in string namePrefab)
        {
            return LoadAsset<GameObject>(namePrefab);
        }

        public static GameObject LoadGameObjectFromPath(in string path)
        {
            return path != null ? LoadAssetFromPath<GameObject>(path) : null;
        }
        #endregion
    }
}
