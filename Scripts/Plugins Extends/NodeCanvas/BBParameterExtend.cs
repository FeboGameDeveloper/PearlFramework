#if NODE_CANVAS

using NodeCanvas.Framework;

namespace Pearl.NodeCanvas
{
    public static class BBParameterExtend
    {
        public static bool IsExist<T>(this BBParameter<T> parameter, out T value)
        {
            bool isExist = parameter != null && !parameter.isNoneOrNull;
            value = isExist ? parameter.value : default;
            return isExist;
        }
    }
}

#endif