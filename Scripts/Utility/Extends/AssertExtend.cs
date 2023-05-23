using Pearl.Testing;

namespace Pearl
{
    public static class AssertExtend
    {
        public static void PreConditions(params object[] conditions)
        {
            ParamsCondition(conditions);
        }

        public static void PostConditions(params object[] conditions)
        {
            ParamsCondition(conditions);
        }

        public static void Invaiants(params object[] conditions)
        {
            ParamsCondition(conditions);
        }

        public static void ParamsCondition(params object[] conditions)
        {
#if UNITY_ASSERTIONS
            if (conditions != null && conditions.Length % 2 == 0)
            {
                for (int i = 0; i < conditions.Length; i += 2)
                {
                    var value = conditions[i + 1];
                    var nameVar = conditions[i];

                    if (value is bool conditionBool)
                    {
                        if (!conditionBool)
                        {
                            LogManager.LogWarning("Assertion: the " + nameVar + " condition is false");
                        }
                    }
                    else if (UtilityMethods.IsNull(value))
                    {
                        LogManager.LogWarning("Assertion: the " + nameVar + " var is null");
                    }
                }
            }
            else
            {
                LogManager.LogWarning("Assertion: The conditions are null");
            }
#endif
        }
    }
}
