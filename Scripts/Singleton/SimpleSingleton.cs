namespace Pearl
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// </summary>
    public static class SimpleSingleton<T>
        where T : class
    {
        private static readonly object m_Lock = new();
        private static T m_Instance = null;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        /// 
        public static T GetIstance(params object[] parameters)
        {
            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    m_Instance = ReflectionExtend.CreateInstance<T>(parameters);
                }
            }
            return m_Instance;
        }

        public static bool GetIstance(out T result, params object[] parameters)
        {
            result = GetIstance(parameters);
            return result != null;
        }
    }
}
