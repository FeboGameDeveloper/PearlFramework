using Pearl.Storage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class GameVersionManager
    {
        [Storage("General")]
        private static List<int> _gameVersion;
        private static List<int> _editorGameVersion;
        private static GameVersionManager _instance;

        //Used by reflection (GameManager)
        protected static string ControlVersion(Type versionType)
        {
            if (!versionType.IsSubClass(typeof(GameVersionManager)))
            {
                return null;
            }

            GetEditorVersion();
            GetSavedVersion();

            if (!_editorGameVersion.IsEqualContainer(_gameVersion))
            {
                Pearl.Testing.LogManager.Log("difference version");

                _instance = ReflectionExtend.CreateInstance<GameVersionManager>(versionType);
                if (_instance != null)
                {
                    _instance.DifferenceVersion(_gameVersion, _editorGameVersion);
                }

                _gameVersion = _editorGameVersion;
            }

            var gameVersionString = "";
            for (int i = 0; i < _gameVersion.Count; i++)
            {
                gameVersionString += _gameVersion[i].ToString();
                if (i != _gameVersion.Count - 1)
                {
                    gameVersionString += ".";
                }
            }

            StorageManager.Save(typeof(GameVersionManager), StorageTypeEnum.Hard, null, false);

            return gameVersionString;
        }

        protected virtual void DifferenceVersion(in List<int> savedVersion, in List<int> newVersion)
        {
        }

        private static void GetEditorVersion()
        {
            string version = Application.version;
            version = version.Trim();
            var versions = version.Split(".");
            if (versions.IsAlmostSpecificCount())
            {
                _editorGameVersion = new List<int>();
                for (int i = 0; i < versions.Length; i++)
                {
                    if (int.TryParse(versions[i], out int result))
                    {
                        _editorGameVersion.Add(result);
                    }
                    else
                    {
                        _editorGameVersion = null;
                        break;
                    }
                }
            }
        }

        private static void GetSavedVersion()
        {
            StorageManager.Load<GameVersionManager>(StorageTypeEnum.Hard);
            if (!_gameVersion.IsAlmostSpecificCount())
            {
                _gameVersion = _editorGameVersion;
            }
        }
    }
}
