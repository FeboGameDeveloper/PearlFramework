using UnityEngine;

namespace Pearl.Storage
{
    public static class PlayerPrefsExtend
    {
        #region Load
        public static void LoadString(string nameStoredVar, MemberComplexInfo complexMemberInfo, string defaultValue = "")
        {
            if (nameStoredVar != null)
            {
                string result = PlayerPrefs.GetString(nameStoredVar, defaultValue);
                ReflectionExtend.SetValue(complexMemberInfo, result);
            }
        }

        public static void LoadString(MemberComplexInfo complexMemberInfo, string defaultValue = "")
        {
            LoadString(complexMemberInfo.memberInfo.Name, complexMemberInfo, defaultValue);
        }

        public static void LoadInt(string nameStoredVar, MemberComplexInfo complexMemberInfo, int defaultValue = 0)
        {
            if (nameStoredVar != null)
            {
                int result = PlayerPrefs.GetInt(nameStoredVar, defaultValue);
                ReflectionExtend.SetValue(complexMemberInfo, result);
            }
        }

        public static void LoadInt(MemberComplexInfo complexMemberInfo, int defaultValue = 0)
        {
            LoadInt(complexMemberInfo.memberInfo.Name, complexMemberInfo, defaultValue);
        }

        public static void LoadFloat(string nameStoredVar, MemberComplexInfo complexMemberInfo, float defaultValue = 0f)
        {
            if (nameStoredVar != null)
            {
                float result = PlayerPrefs.GetFloat(nameStoredVar, defaultValue);
                ReflectionExtend.SetValue(complexMemberInfo, result);
            }
        }

        public static void LoadFloat(MemberComplexInfo complexMemberInfo, float defaultValue = 0f)
        {
            LoadFloat(complexMemberInfo.memberInfo.Name, complexMemberInfo, defaultValue);
        }

        public static void LoadBool(string nameStoredVar, MemberComplexInfo complexMemberInfo, bool defaultValue = false)
        {
            if (nameStoredVar != null)
            {
                bool result = GetBool(nameStoredVar, defaultValue);
                ReflectionExtend.SetValue(complexMemberInfo, result);
            }
        }

        public static void LoadBool(MemberComplexInfo complexMemberInfo, bool defaultValue = false)
        {
            LoadBool(complexMemberInfo.memberInfo.Name, complexMemberInfo, defaultValue);
        }
        #endregion

        #region Set
        public static void SetBool(string nameStoredVar, bool value)
        {
            if (nameStoredVar != null)
            {
                int aux = System.Convert.ToInt32(value);
                PlayerPrefs.SetInt(nameStoredVar, aux);
            }
        }
        #endregion

        #region Get
        public static bool GetBool(string nameStoredVar, bool defaultValue = false)
        {
            if (nameStoredVar != null)
            {
                int i = PlayerPrefs.GetInt(nameStoredVar, System.Convert.ToInt32(defaultValue));
                return System.Convert.ToBoolean(i);
            }
            return false;
        }
        #endregion
    }

}