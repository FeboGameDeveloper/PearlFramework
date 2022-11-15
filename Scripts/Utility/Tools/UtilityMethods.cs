using Pearl.Debug;
using System;

namespace Pearl
{
    //metodi di utilità gnerici
    public static class UtilityMethods
    {
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            if (lhs.Equals(rhs))
            {
                return;
            }

            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public static bool Cast<T>(this object obj, out T result)
        {
            if (obj != null && obj is T aux)
            {
                result = aux;
                return true;
            }

            result = default;
            return false;
        }

        public static int Int(this in bool @this, bool zeroIsPositive = false)
        {
            return @this ? (zeroIsPositive ? 0 : 1) : -1;
        }

        public static bool IsSubClass(this Type a, Type b, bool acceptEquals = true)
        {
            return a.IsSubclassOf(b) || (acceptEquals && a == b);
        }

        public static bool IsSubClass<T, F>(bool acceptEquals = true)
        {
            Type a = typeof(T);
            a = a.IsArray ? a.GetElementType() : a;

            Type b = typeof(F);

            return IsSubClass(a, b, acceptEquals);
        }

        public static void Invert(this ref bool value)
        {
            value = !value;
        }

        public static bool IsNotNull<T>(this T @this, out T result)
        {
            result = @this;

            return !IsNull(@this);
        }

        public static bool IsNull(this object @this)
        {
            return @this is UnityEngine.Object unityObject ? unityObject == null : @this == null;
        }

        public static bool IsAlmostNull(params object[] objs)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (IsNull(objs[i]))
                {
                    LogManager.LogWarning("there is at least one null parameter");
                    return true;
                }
            }
            return false;
        }
    }

}