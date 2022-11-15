using UnityEngine;

namespace Pearl.Mobile
{
    public static class DeviceExtend
    {
        public static string DownloadDataPath
        {
            get
            {
                string[] temp = (Application.persistentDataPath.Replace("Android", "")).Split(new string[] { "//" }, System.StringSplitOptions.None);
                return (temp[0] + "/Download");
            }
        }
    }
}
