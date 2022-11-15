using UnityEditor;

namespace Pearl.Editor
{
    public static class EditorExtend
    {
        public static T[] GetAllInstances<T>() where T : UnityEngine.Object
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);  //FindAssets uses tags check documentation for more info
            T[] a = new T[guids.Length];
            for (int i = 0; i < guids.Length; i++)         //probably could get optimized 
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static T GetInstance<T>() where T : UnityEngine.Object
        {
            T[] result = GetAllInstances<T>();
            if (result.IsAlmostSpecificCount())
            {
                return result[0];
            }
            return null;
        }
    }
}
