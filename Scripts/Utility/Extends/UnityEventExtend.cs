using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Events;

namespace Pearl
{
    public static class UnityEventExtend
    {
        public static List<UnityAction> GetPersistentUnityActionsWithoutParameter<T>(this T ev) where T : UnityEventBase
        {
            List<UnityAction> result = new();
            for (int i = 0; i < ev.GetPersistentEventCount(); i++)
            {
                MethodInfo info = UnityEventBase.GetValidMethodInfo(ev.GetPersistentTarget(i), ev.GetPersistentMethodName(i), new Type[] { });
                UnityAction execute = () => info.Invoke(ev.GetPersistentTarget(i), new object[] { });
                result.Add(execute);
            }
            return result;
        }

        public static List<F> GetListComponents<F>(this UnityEventBase ev) where F : UnityEngine.Object
        {
            List<F> result = new List<F>();
            var aux = GetListComponents(ev);
            for (int i = 0; i < aux.Count; i++)
            {
                if (aux[i] is F component)
                {
                    result.Add(component);
                }

            }
            return result;
        }

        public static List<UnityEngine.Object> GetListComponents(this UnityEventBase ev)
        {
            List<UnityEngine.Object> result = new List<UnityEngine.Object>();
            for (int i = 0; i < ev.GetPersistentEventCount(); i++)
            {
                result.Add(ev.GetPersistentTarget(i));
            }
            return result;
        }
    }
}