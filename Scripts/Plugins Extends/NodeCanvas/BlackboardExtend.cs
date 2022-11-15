#if NODE_CANVAS

using NodeCanvas.Framework;

namespace Pearl.NodeCanvas
{
    public static class BlackboardExtend
    {
        public static void UpdateVariable<T>(this IBlackboard blackboard, string nameVar, T newValue)
        {
            if (blackboard == null || nameVar == null)
            {
                return;
            }

            var v = blackboard.GetVariable<T>(nameVar);

            if (v != null)
            {
                v.SetValue(newValue);
            }
            else
            {
                blackboard.AddVariable<T>(nameVar, newValue);
            }
        }
    }
}

#endif