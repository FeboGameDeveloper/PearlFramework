using UnityEngine;

namespace Pearl
{
    /// <summary>
    /// The singleton class: is a special class that holds the unique class reference
    /// </summary>
    /// 
    public enum CreateSingletonEnum { Null, Create, CreateStatic }

    public static class Singleton<T> where T : PearlBehaviour, ISingleton
    {
        private readonly static object m_Lock = new();
        private static T m_Instance;

        #region GetIstance

        //Method to retrieve the instance
        /// <param name = "root">Indeed to search throughout the project, the method will search from that specific path</param>
        public static T GetIstance(in CreateSingletonEnum create = CreateSingletonEnum.Null, in string root = null, in bool repeatSearch = false)
        {
            lock (m_Lock)
            {
                if (repeatSearch || m_Instance == null)
                {
                    m_Instance = root == null ? GameObject.FindObjectOfType<T>() : GameObjectExtend.FindInHierarchy<T>(root);


                    if (m_Instance == null && create != CreateSingletonEnum.Null)
                    {
                        m_Instance = GameObjectExtend.CreateGameObjectWithComponent<T>(typeof(T).Name);
                        if (m_Instance != null && create == CreateSingletonEnum.CreateStatic)
                        {
                            GameObject.DontDestroyOnLoad(m_Instance);
                        }
                    }

                    // If the component is destroyed, the singleton will also be destroyed
                    if (m_Instance != null)
                    {
                        m_Instance.DestroyHandler += OnDestroy;
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning("The " + typeof(T).Name + " doesn't exist");
                    }
                }

                return m_Instance;
            }
        }

        //Method to retrieve the instance
        /// <param name = "root">Indeed to search throughout the project, the method will search from that specific path</param>
        public static bool GetIstance(out T result, in CreateSingletonEnum create = CreateSingletonEnum.Null, in string root = null, in bool repeatSearch = false)
        {
            result = GetIstance(create, root, repeatSearch);
            return result != null;
        }

        public static bool GetIstance(out T result, in string path, bool dontDstoryAtLoad, in string root = null, in bool repeatSearch = false)
        {
            if (Singleton<T>.GetIstance(out result, CreateSingletonEnum.Null, root, repeatSearch))
            {
                return true;
            }
            else
            {
                return GameObjectExtend.CreateGameObjectFromBindings<T>(path, out result, typeof(T).Name, dontDstoryAtLoad);
            }
        }
        #endregion

        private static void OnDestroy()
        {
            m_Instance = null;
        }
    }
}