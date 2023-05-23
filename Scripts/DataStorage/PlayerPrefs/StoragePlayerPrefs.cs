using Pearl.Testing;
using System;
using System.Reflection;
using UnityEngine;

namespace Pearl.Storage
{
    public static class StoragePlayerPrefs
    {
        #region Save
        public static void SaveAll(bool saveInHardDisk = true)
        {
            var list = GameObjectExtend.FindAllInterfaces<IStoragePlayerPrefs>();
            foreach (var mono in list)
            {
                Save(mono, false);
            }

            if (saveInHardDisk)
            {
                PlayerPrefs.Save();
            }
        }

        public static void Save(object container, bool saveInHardDisk = true)
        {
            if (container == null)
            {
                return;
            }

            Type type = container.GetType();
            while (type != null)
            {
                MemberInfo[] memberInfos = ReflectionExtend.GetPropertiesAndFieldsWithAttribute<StoragePlayerPrefsAttribute>(type);
                type = type.BaseType;

                if (memberInfos == null)
                {
                    return;
                }

                foreach (MemberInfo memberInfo in memberInfos)
                {
                    Save(memberInfo, container, false);
                }
            }

            if (saveInHardDisk)
            {
                PlayerPrefs.Save();
            }
        }

        public static void Save(MemberInfo memberInfo, object container, bool saveInHardDisk = true)
        {
            var attribute = memberInfo.GetCustomAttribute<StoragePlayerPrefsAttribute>();

            if (attribute == null)
            {
                return;
            }

            var value = memberInfo.GetValue(container);

            var nameVar = attribute.NameStoredVar;
            nameVar ??= memberInfo.Name;

            var resultType = memberInfo.ValueType();

            if (resultType == typeof(string))
            {
                PlayerPrefs.SetString(nameVar, (string)value);
            }
            else if (resultType == typeof(bool))
            {
                PlayerPrefsExtend.SetBool(nameVar, (bool)value);
            }
            else if (resultType == typeof(float))
            {
                PlayerPrefs.SetFloat(nameVar, (float)value);
            }
            else if (ConvertExtend.TryConvertTo<int>(value, out int auxInteger))
            {
                PlayerPrefs.SetInt(nameVar, auxInteger);
            }
            else
            {
                LogManager.LogError("You can't save this var");
            }

            if (saveInHardDisk)
            {
                PlayerPrefs.Save();
            }
        }

        #endregion

        #region Load
        public static void LoadAll()
        {
            var list = GameObjectExtend.FindAllInterfaces<IStoragePlayerPrefs>();
            foreach (var mono in list)
            {
                Load(mono);
            }
        }

        public static void Load(object container)
        {
            if (container == null)
            {
                return;
            }

            Type type = container.GetType();
            while (type != null)
            {
                MemberInfo[] memberInfos = ReflectionExtend.GetPropertiesAndFieldsWithAttribute<StoragePlayerPrefsAttribute>(type);
                type = type.BaseType;

                if (memberInfos == null)
                {
                    return;
                }

                foreach (MemberInfo memberInfo in memberInfos)
                {
                    Load(memberInfo, container);
                }
            }
        }

        public static void Load(MemberInfo memberInfo, object container)
        {
            var attribute = memberInfo.GetCustomAttribute<StoragePlayerPrefsAttribute>();

            if (attribute == null)
            {
                return;
            }

            var value = memberInfo.GetValue(container);

            var nameVar = attribute.NameStoredVar;
            nameVar ??= memberInfo.Name;

            var resultType = memberInfo.ValueType();

            var membrComplexInfo = new MemberComplexInfo(memberInfo, container);

            if (resultType == typeof(string))
            {
                PlayerPrefsExtend.LoadString(nameVar, membrComplexInfo, (string)value);
            }
            else if (resultType == typeof(bool))
            {
                PlayerPrefsExtend.LoadBool(nameVar, membrComplexInfo, (bool)value);
            }
            else if (resultType == typeof(float))
            {
                PlayerPrefsExtend.LoadFloat(nameVar, membrComplexInfo, (float)value);
            }
            else if (ConvertExtend.TryConvertTo<int>(value, out int auxInteger))
            {
                PlayerPrefsExtend.LoadInt(nameVar, membrComplexInfo, auxInteger);
            }
            else
            {
                LogManager.LogWarning("You can't load this var");
            }
        }
        #endregion
    }
}
