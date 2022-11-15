using Pearl.Debug;

namespace Pearl.Examples.SingletonExamples
{
    public class SingletonTest1 : PearlBehaviour, ISingleton
    {
        public string s;

        public static bool GetIstance(out SingletonTest1 result)
        {
            return Singleton<SingletonTest1>.GetIstance(out result);
        }

        public static void DebugLog()
        {
            if (GetIstance(out SingletonTest1 test))
            {
                LogManager.Log(test.s);
            }
        }
    }
}
