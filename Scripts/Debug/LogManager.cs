using System;
using UnityEngine;

namespace Pearl.Testing
{
    public class LogManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private StringLogInfoDictionary debugCategory = null;

        [System.Serializable]
        public struct LogInfo
        {
            public InformationTypeEnum informationType;
        }

        #region Public Methods
        public static void Log(in object message, in string category = "")
        {
            Log(message, false, false, category, null);
        }

        public static void LogWarning(in object message, in string category = "")
        {
            Log(message, true, false, category, null);
        }

        public static void LogError(in object message, in string category = "")
        {
            Log(message, false, true, category, null);
        }

        public static void Log(in object message, UnityEngine.Object context, in string category = "")
        {
            Log(message, false, false, category, context);
        }

        public static void LogWarning(in object message, UnityEngine.Object context, in string category = "")
        {
            Log(message, true, false, category, context);
        }

        public static void LogError(in object message, UnityEngine.Object context, in string category = "")
        {
            Log(message, false, true, category, context);
        }

        #endregion

        #region Private Methods
        private static bool GetIstance(out LogManager result)
        {
            return Singleton<LogManager>.GetIstance(out result);
        }

        private static void Log(in object message, in bool isWarning, in bool isError, in string category, UnityEngine.Object context)
        {
            if (category == "" || !GetIstance(out var result) || !result.debugCategory.ContainsKey(category) ||
                (result.debugCategory.IsNotNullAndTryGetValue(category, out LogInfo logInfo) &&
                ((isWarning && logInfo.informationType == InformationTypeEnum.Warning) || (isError && logInfo.informationType == InformationTypeEnum.Error) || (!isError && !isWarning && logInfo.informationType == InformationTypeEnum.Info))))
            {
                Log(message, isWarning, isError, context);
            }
        }

        private static void Log(in object message, in bool isWarning, in bool isError, UnityEngine.Object context)
        {
            string messageString = message.ToString();
            messageString += " || " + DateTime.Now.ToString();
            if (isWarning)
            {
                UnityEngine.Debug.LogWarning(messageString, context);
            }
            else if (isError)
            {
                UnityEngine.Debug.LogError(messageString, context);
            }
            else
            {
                UnityEngine.Debug.Log(messageString, context);
            }
        }
        #endregion
    }
}
