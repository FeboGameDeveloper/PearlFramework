using Pearl.Debug;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Events
{
    /// <summary>
    /// this class allows a method to wait for one or more specific objects
    /// </summary>
    public class WaitManager : MonoBehaviour
    {
        private static readonly Dictionary<object, Delegate> _dictionaryEvent = new();
        private static readonly Dictionary<Delegate, List<object>> methodWaits = new();

        #region Call
        /// <summary>
        /// The object warns that it has finished taking its action
        /// </summary>
        public static void Call(object container)
        {
            if (CallInternal(container, out var del))
            {
                del.DynamicInvoke();
            }
        }

        /// <summary>
        /// The object warns that it has finished taking its action
        /// </summary>
        public static bool Call<T>(object container, T param)
        {
            if (CallInternal(container, out var del))
            {
                try
                {
                    del.DynamicInvoke(param);
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e.Message);
                    return false;
                }
            }

            return true;
        }

        public static bool Call<T, F>(object container, T param1, F param2)
        {
            if (CallInternal(container, out var del))
            {
                try
                {
                    del.DynamicInvoke(param1, param2);
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e.Message);
                    return false;
                }
            }

            return true;
        }

        private static bool CallInternal(object container, out Delegate del)
        {
            if (_dictionaryEvent.IsNotNullAndTryGetValue(container, out del))
            {
                _dictionaryEvent.Remove(container);

                foreach (var action in del.GetInvocationList())
                {
                    if (methodWaits.TryGetValue(action, out var list))
                    {
                        list.Remove(container);
                        if (list.Count == 0)
                        {
                            methodWaits.Remove(action);
                        }
                        else
                        {
                            del = Delegate.Remove(del, action);
                        }
                    }
                }
            }

            return del != null;
        }
        #endregion

        #region Wait
        /// <summary>
        /// The action warns that to be performed it must wait for the wait of that container
        /// </summary>
        public static bool Wait(object container, Action action)
        {
            return WaitInternal(container, action);
        }

        /// <summary>
        /// The action warns that to be performed it must wait for the wait of that container
        /// </summary>
        public static bool Wait<T>(object container, Action<T> action)
        {
            return WaitInternal(container, action);
        }

        public static bool Wait<T, F>(object container, Action<T, F> action)
        {
            return WaitInternal(container, action);
        }

        private static bool WaitInternal(object container, Delegate action)
        {
            if (_dictionaryEvent.IsNotNullAndTryGetValue(container, out var del))
            {
                try
                {
                    _dictionaryEvent.Update(container, Delegate.Combine(del, action));
                }
                catch (ArgumentException e)
                {
                    LogManager.LogWarning(e.Message);
                    return false;
                }
            }
            else
            {
                _dictionaryEvent.Add(container, action);
            }

            if (methodWaits.IsNotNullAndTryGetValue(action, out var list))
            {
                list.Add(container);
            }
            else
            {
                list = new List<object>
                {
                    container
                };
                methodWaits.Add(action, list);
            }

            return true;
        }
        #endregion

        #region Event
        public static void WaitInvokeEvent(Delegate del, Action action)
        {
            foreach (var container in del.GetInvocationList())
            {
                Wait(container.Target, action);
            }

            del.DynamicInvoke();
        }

        public static void WaitInvokeEvent<T>(Delegate del, Action action, T param)
        {
            foreach (var container in del.GetInvocationList())
            {
                Wait(container.Target, action);
            }

            del.DynamicInvoke(param);
        }

        public static void WaitInvokeEvent<T, F>(Delegate del, Action action, T param1, F param2)
        {
            foreach (var container in del.GetInvocationList())
            {
                Wait(container.Target, action);
            }

            del.DynamicInvoke(param1, param2);
        }
        #endregion
    }
}
