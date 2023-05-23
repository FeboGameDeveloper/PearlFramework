using Pearl.Testing;
using Pearl.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Pearl.Storage
{
    public enum StorageTypeEnum { Hard, Light, Onlylight }

    public static class StorageManager
    {
        public enum StorageActionEnum
        {
            Save,
            Load,
            Remove,
        }

        private class ElementStorage
        {
            public string nameVar;
            public object value;
            public MemberComplexInfo memberComplexInfo;
            public string completePath;
            public string key;
            public bool encrypted;
            public bool onlyLight;

            public ElementStorage(string nameVar, string completePath, object value, string key, bool encrypted, bool onlyLight, MemberComplexInfo memberComplexInfo)
            {
                this.nameVar = nameVar;
                this.value = value;
                this.memberComplexInfo = memberComplexInfo;
                this.completePath = completePath;
                this.key = key;
                this.encrypted = encrypted;
                this.onlyLight = onlyLight;
            }

            public override bool Equals(object obj)
            {
                if (obj is ElementStorage elementStorage)
                {
                    return nameVar == elementStorage.nameVar && completePath == elementStorage.completePath;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(nameVar, completePath);
            }
        }

        private const string slotString = "Slot";
        private const int _defaultIndexSlot = 0;
        private static string _slotCompleteString;
        private static readonly Dictionary<string, ElementStorage> storageData = new();
        private static readonly string _persistentDataPath = Application.persistentDataPath;
        private static readonly Queue<Action> _listActionStorage = new();
        private static bool _isRunning = false;

        private static byte[] encryptedKey = { 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15, 0x16, 0x15 };
        public static byte[] EncryptedKey { get { return encryptedKey; } set { encryptedKey = value; } }
        public static string PersistentDataPath { get { return _persistentDataPath; } }
        public static bool IsRunning { get { return _isRunning; } }


        #region Constructors
        static StorageManager()
        {
            ChangeSlot(_defaultIndexSlot);
        }
        #endregion

        #region Public Methods

        #region Save
        public static bool SaveAll()
        {
            if (_isRunning)
            {
                return false;
            }

            if (storageData != null)
            {
                foreach (var element in storageData.Values)
                {
                    if (!element.onlyLight)
                    {
                        Save(element, UseSameFileEnum.Ignore);
                    }
                }
            }

            return true;
        }

        public static async void SaveAllAsync(Action OnSave)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => SaveAllAsync(OnSave));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                if (storageData != null)
                {
                    foreach (var element in storageData.Values)
                    {
                        if (!element.onlyLight)
                        {
                            Save(element, UseSameFileEnum.Ignore);
                        }
                    }
                }
            });

            OnSave?.Invoke();
            FinishTask();
        }

        public static bool Save(object container, StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Save(container, null, typeStorage, ID, encrypted, specificMember);
            return true;
        }

        public static async void SaveAsync(object container, StorageTypeEnum typeStorage, Action OnSave, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => SaveAsync(container, typeStorage, OnSave, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Save(container, null, typeStorage, ID, encrypted, specificMember);
            });

            OnSave?.Invoke();
            FinishTask();
        }

        public static bool Save(Type type, StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Save(null, type, typeStorage, ID, encrypted, specificMember);
            return true;
        }

        public static async void SaveAsync(Type type, StorageTypeEnum typeStorage, Action OnSave, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => SaveAsync(type, typeStorage, OnSave, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Save(null, type, typeStorage, ID, encrypted, specificMember);
            });

            OnSave?.Invoke();
            FinishTask();
        }

        public static bool Save<T>(StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Save(null, typeof(T), typeStorage, ID, encrypted, specificMember);
            return true;
        }

        public static async void SaveAsync<T>(StorageTypeEnum typeStorage, Action OnSave, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => SaveAsync<T>(typeStorage, OnSave, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Save(null, typeof(T), typeStorage, ID, encrypted, specificMember);
            });

            OnSave?.Invoke();
            FinishTask();
        }
        #endregion

        #region Load
        public static bool LoadAll()
        {
            if (_isRunning)
            {
                return false;
            }

            bool result = false;

            if (storageData != null)
            {
                foreach (var element in storageData.Values)
                {
                    if (!element.onlyLight)
                    {
                        result = Load(element);
                    }
                }
            }

            return result;
        }

        public static async void LoadAllAsync(Action<bool> OnLoad)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => LoadAllAsync(OnLoad));
                return;
            }

            _isRunning = true;
            bool result = false;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                if (storageData != null)
                {
                    foreach (var element in storageData.Values)
                    {
                        if (!element.onlyLight)
                        {
                            result = Load(element);
                        }
                    }
                }
            });

            OnLoad?.Invoke(result);
            FinishTask();
        }

        public static bool Load(object container, StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            return Load(container, null, typeStorage, ID, encrypted, specificMember);
        }

        public static async void LoadAsync(object container, StorageTypeEnum typeStorage, Action<bool> OnLoad, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => LoadAsync(container, typeStorage, OnLoad, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            bool result = false;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                result = Load(container, null, typeStorage, ID, encrypted, specificMember);
            });

            OnLoad?.Invoke(result);
            FinishTask();
        }

        public static bool Load(Type type, StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            return Load(null, type, typeStorage, ID, encrypted, specificMember);
        }

        public static async void LoadAsync(Type type, StorageTypeEnum typeStorage, Action<bool> OnLoad, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => LoadAsync(type, typeStorage, OnLoad, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            bool result = false;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                result = Load(null, type, typeStorage, ID, encrypted, specificMember);
            });

            OnLoad?.Invoke(result);
            FinishTask();
        }

        public static bool Load<T>(StorageTypeEnum typeStorage, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            return Load(null, typeof(T), typeStorage, ID, encrypted, specificMember);
        }

        public static async void LoadAsync<T>(StorageTypeEnum typeStorage, Action<bool> OnLoad, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => LoadAsync<T>(typeStorage, OnLoad, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            bool result = false;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                result = Load(null, typeof(T), typeStorage, ID, encrypted, specificMember);
            });

            OnLoad?.Invoke(result);
            FinishTask();
        }
        #endregion

        #region Remove
        public static bool RemoveAll(StorageTypeEnum typeStorage)
        {
            if (_isRunning)
            {
                return false;
            }

            if (storageData != null)
            {
                for (int i = storageData.Count - 1; i >= 0; i--)
                {
                    var element = storageData.Values.Get(i);

                    if (typeStorage != StorageTypeEnum.Onlylight || element.onlyLight)
                    {
                        Remove(typeStorage != StorageTypeEnum.Hard, element.key);
                    }
                }
            }

            return true;
        }

        public static async void RemoveAllAsync(StorageTypeEnum typeStorage, Action OnRemove)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => RemoveAllAsync(typeStorage, OnRemove));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                if (storageData != null)
                {
                    for (int i = storageData.Count - 1; i >= 0; i--)
                    {
                        var element = storageData.Values.Get(i);

                        if (typeStorage != StorageTypeEnum.Onlylight || element.onlyLight)
                        {
                            Remove(typeStorage != StorageTypeEnum.Hard, element.key);
                        }
                    }
                }
            });

            OnRemove?.Invoke();
            FinishTask();
        }

        public static bool Remove(object container, bool isLight, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Remove(container, null, isLight, ID, encrypted, specificMember);
            return true;
        }

        public static async void RemoveAsync(object container, bool isLight, Action OnRemove, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => RemoveAsync(container, isLight, OnRemove, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Remove(container, null, isLight, ID, encrypted, specificMember);
            });

            OnRemove?.Invoke();
            FinishTask();
        }

        public static bool Remove(Type type, bool isLight, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Remove(null, type, isLight, ID, encrypted, specificMember);

            return true;
        }

        public static async void RemoveAsync(Type type, bool isLight, Action OnRemove, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => RemoveAsync(type, isLight, OnRemove, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Remove(null, type, isLight, ID, encrypted, specificMember);
            });

            OnRemove?.Invoke();
            FinishTask();
        }

        public static bool Remove<T>(bool isLight, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                return false;
            }

            Remove(null, typeof(T), isLight, ID, encrypted, specificMember);
            return true;
        }

        public static async void RemoveAsync<T>(bool isLight, Action OnRemove, string ID = null, bool encrypted = false, string specificMember = null)
        {
            if (_isRunning)
            {
                _listActionStorage.Enqueue(() => RemoveAsync<T>(isLight, OnRemove, ID, encrypted, specificMember));
                return;
            }

            _isRunning = true;
            PearlEventsManager.CallEvent(ConstantStrings.StartStorage, PearlEventType.Normal);
            await Task.Run(() =>
            {
                Remove(null, typeof(T), isLight, ID, encrypted, specificMember);
            });

            OnRemove?.Invoke();
            FinishTask();
        }
        #endregion

        public static void ChangeSlot(int numberSlot)
        {
            _slotCompleteString = numberSlot.ToString() + slotString;
        }
        #endregion

        #region Private Methods

        #region Path
        private static string GetRelativePath(string relativePath, GetterEnum? getterEnum, object behaviour, Type type)
        {
            if (getterEnum == null)
            {
                return relativePath;
            }
            else
            {
                return behaviour != null ? ReflectionExtend.Getter<String>(behaviour, relativePath, (GetterEnum)getterEnum) :
                    ReflectionExtend.Getter<String>(type, relativePath, (GetterEnum)getterEnum);
            }
        }

        private static string GetCompletePath(string relativePath, string absolutePath, bool iSlot)
        {

            /*
#if UNITY_ANDROID && !UNITY_EDITOR
    try 
    {
        IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
        IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");
 
        using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
        {
            using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
            {
                IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
                IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
                IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");   
                                 
                absolutePath = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);                    
 
                if (absolutePath != null) 
                {
                    Debug.LogManager.Log("Got internal path: " + absolutePath);
                }
                else 
                {
                    Debug.LogManager.Log("Using fallback path");
                    absolutePath = "/data/data/" + Application.identifier + "/files";
                }
            }
        }
    }
    catch(Exception e) 
    {
        Debug.LogManager.Log(e.ToString());
    }
#endif
            */

            if (string.IsNullOrWhiteSpace(relativePath))
            {
                relativePath = "Default";
            }

            string path = iSlot ? Path.Combine("Slots", _slotCompleteString, relativePath) : relativePath;
            path = Path.Combine(absolutePath, path);
            path = Path.ChangeExtension(path, "json");

            if (DebugManager.GetActiveDebug(ConstantStrings.debugStorage))
            {
                path += "Debug";
            }

            return path;
        }

        #endregion

        #region ElementToStoage
        private static void UpdateStorageData(ElementStorage elementStorage)
        {
            storageData?.Update(elementStorage.key, elementStorage);
        }

        private static void SaveElementStorage(object behaviour, Type currentType, List<MemberInfo> members, string ID, bool encrypted, bool onlyLight)
        {
            if (members.IsAlmostSpecificCount())
            {
                string IDForVar = string.IsNullOrWhiteSpace(ID) ? "" : ID + ".";

                foreach (var member in members)
                {
                    if (member.GetCustomAttribute(typeof(StorageAttribute), false) is StorageAttribute attribute)
                    {
                        object currentContainer = behaviour ?? member.Name;

                        string key = ID + member.Name + GetRelativePath(attribute.RelativePath, attribute.Getter, behaviour, currentType);
                        var value = member.GetValue(currentContainer);
                        var nameVar = IDForVar + member.Name;
                        MemberComplexInfo memberComplexInfo = new(member, currentContainer);

                        string completePath = GetCompletePath(GetRelativePath(attribute.RelativePath, attribute.Getter, behaviour, currentType), attribute.AbsolutePath, attribute.IsSlot);

                        ElementStorage elementStorage = new(nameVar, completePath, value, key, encrypted, onlyLight, memberComplexInfo);
                        UpdateStorageData(elementStorage);
                    }
                }
            }
        }
        #endregion

        #region Load
        private static bool Load(object behaviour, Type type, StorageTypeEnum typeStorage, string ID, bool encrypted, string specificMember = null)
        {
            if (behaviour == null && type == null)
            {
                return false;
            }

            Type currentType = behaviour != null ? behaviour.GetType() : type;
            List<MemberInfo> members = GetStorageAttributes(currentType, specificMember);

            if (members.IsAlmostSpecificCount())
            {
                bool result = true;
                int count = members.Count;
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];

                    if (member.GetCustomAttribute(typeof(StorageAttribute), false) is StorageAttribute attribute)
                    {
                        string key = ID + member.Name + GetRelativePath(attribute.RelativePath, attribute.Getter, behaviour, currentType);

                        if (!storageData.ContainsKey(key))
                        {
                            SaveElementStorage(behaviour, currentType, members, ID, encrypted, typeStorage == StorageTypeEnum.Onlylight);
                        }

                        if (storageData.IsNotNullAndTryGetValue(key, out var element))
                        {
                            UseSameFileEnum useSameFile = count == 1 ? UseSameFileEnum.Ignore : i == count - 1 ? UseSameFileEnum.End : UseSameFileEnum.Start;
                            bool aux = Load(typeStorage, element, useSameFile);
                            if (!aux)
                            {
                                result = aux;
                            }
                        }
                    }
                }

                return result;
            }
            return false;
        }

        private static bool Load(StorageTypeEnum typeStorage, ElementStorage element, UseSameFileEnum useSameFile = UseSameFileEnum.Ignore)
        {
            bool result = true;
            if (typeStorage == StorageTypeEnum.Hard)
            {
                result = Load(element, useSameFile);
            }

            LightLoad(element);
            return result;
        }

        private static bool Load(ElementStorage elementStorage, UseSameFileEnum useSameFile = UseSameFileEnum.Ignore)
        {
            bool isLoaded = false;
            try
            {
                isLoaded = StorageUtility.Load(elementStorage.nameVar, elementStorage.completePath, elementStorage.value, elementStorage.encrypted, out object result, useSameFile);
                elementStorage.value = result;
            }
            catch (Exception e)
            {
                LogManager.LogWarning(e);
            }

            UpdateStorageData(elementStorage);
            return isLoaded;
        }

        private static void LightLoad(ElementStorage elementStorage)
        {
            ReflectionExtend.SetValue(elementStorage.memberComplexInfo, elementStorage.value);
        }

        #endregion

        #region Save
        private static void Save(object behaviour, Type type, StorageTypeEnum typeStorage, string ID, bool encrypted, string specificMember = null)
        {
            if (behaviour == null && type == null)
            {
                return;
            }

            Type currentType = behaviour != null ? behaviour.GetType() : type;
            List<MemberInfo> members = GetStorageAttributes(currentType, specificMember);

            if (members.IsAlmostSpecificCount())
            {
                int count = members.Count;
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];

                    if (member.GetCustomAttribute(typeof(StorageAttribute), false) is StorageAttribute attribute)
                    {
                        string key = ID + member.Name + GetRelativePath(attribute.RelativePath, attribute.Getter, behaviour, currentType);
                        if (!storageData.ContainsKey(key))
                        {
                            SaveElementStorage(behaviour, currentType, members, ID, encrypted, typeStorage == StorageTypeEnum.Onlylight);
                        }

                        if (storageData.IsNotNullAndTryGetValue(key, out var element))
                        {
                            UseSameFileEnum useSameFile = count == 1 ? UseSameFileEnum.Ignore : i == count - 1 ? UseSameFileEnum.End : UseSameFileEnum.Start;
                            Save(typeStorage != StorageTypeEnum.Hard, element, useSameFile);
                        }
                    }
                }
            }
        }

        private static void Save(bool isLight, ElementStorage element, UseSameFileEnum useSameFile = UseSameFileEnum.Ignore)
        {
            SaveLight(element);

            if (!isLight)
            {
                Save(element, useSameFile);
            }
        }

        private static void Save(ElementStorage elementStorage, UseSameFileEnum useSameFile = UseSameFileEnum.Ignore)
        {
            try
            {
                StorageUtility.Save(elementStorage.nameVar, elementStorage.value, elementStorage.completePath, elementStorage.encrypted, useSameFile);
            }
            catch (Exception e)
            {
                LogManager.LogWarning(e);
            }
        }

        private static void SaveLight(ElementStorage element)
        {
            var memberInfo = element.memberComplexInfo.memberInfo;
            var container = element.memberComplexInfo.container;
            element.value = memberInfo.GetValue(container);
        }
        #endregion

        #region Remove
        private static void Remove(object behaviour, Type type, bool isLight, string ID, bool encrypted, string specificMember)
        {
            if (behaviour == null && type == null)
            {
                return;
            }

            Type currentType = behaviour != null ? behaviour.GetType() : type;
            List<MemberInfo> members = GetStorageAttributes(currentType, specificMember);

            if (members.IsAlmostSpecificCount())
            {
                ID = string.IsNullOrWhiteSpace(ID) ? "" : ID + ".";
                int count = members.Count;
                for (int i = 0; i < members.Count; i++)
                {
                    var member = members[i];

                    if (member.GetCustomAttribute(typeof(StorageAttribute), false) is StorageAttribute attribute)
                    {
                        string key = ID + member.Name + GetRelativePath(attribute.RelativePath, attribute.Getter, behaviour, currentType);

                        if (!storageData.ContainsKey(key))
                        {
                            SaveElementStorage(behaviour, currentType, members, ID, encrypted, false);
                        }

                        UseSameFileEnum useSameFile = count == 1 ? UseSameFileEnum.Ignore : i == count - 1 ? UseSameFileEnum.End : UseSameFileEnum.Start;
                        Remove(isLight, key, useSameFile);
                    }
                }
            }
        }

        private static void Remove(bool isLight, string key, UseSameFileEnum useSameFile = UseSameFileEnum.Ignore)
        {
            if (storageData.Remove(key, out var element))
            {
                try
                {
                    if (!isLight)
                    {
                        StorageUtility.Remove(element.nameVar, element.completePath, element.encrypted, useSameFile);
                    }
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e);
                }
            }
        }
        #endregion

        private static void FinishTask()
        {
            if (_listActionStorage != null && _listActionStorage.Count >= 1)
            {
                var action = _listActionStorage.Dequeue();
                action?.Invoke();
            }
            else
            {
                _isRunning = false;
                PearlEventsManager.CallEvent(ConstantStrings.FinishStorage, PearlEventType.Normal);
            }
        }

        private static List<MemberInfo> GetStorageAttributes(Type type, string specificMember = null)
        {
            List<MemberInfo> result = new();

            while (type != null)
            {
                MemberInfo[] membersInfo = string.IsNullOrWhiteSpace(specificMember) ?
                    ReflectionExtend.GetPropertiesAndFieldsWithAttribute<StorageAttribute>(type) :
                    type.GetCustomAttribute<StorageAttribute>(specificMember);

                if (membersInfo != null)
                {
                    result.AddRange(membersInfo);
                }
                type = type.BaseType;
            }
            return result;
        }
        #endregion
    }
}