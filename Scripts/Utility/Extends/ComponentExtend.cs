using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Pearl
{
    public static class ComponentExtend
    {
        private readonly static List<Component> _componentCache = new();

        #region component no alloc
        /// <summary>
        /// Grabs a component without allocating memory uselessly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T GetComponentNoAlloc<T>(this GameObject @this) where T : Component
        {
            if (@this != null)
            {
                @this.GetComponents(typeof(T), _componentCache);
                Component component = _componentCache.Count > 0 ? _componentCache[0] : null;
                _componentCache.Clear();
                return component as T;
            }
            else
            {
                return null;
            }
        }

        public static bool TryGetComponentNoAlloc<T>(this GameObject @this, out T result) where T : Component
        {
            result = @this.GetComponentNoAlloc<T>();
            return result != null;
        }

        /// <summary>
        /// Grabs a component without allocating memory uselessly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static T GetComponentNoAlloc<T>(this Component @this) where T : Component
        {
            return @this != null ? @this.gameObject.GetComponentNoAlloc<T>() : null;
        }

        public static bool TryGetComponentNoAlloc<T>(this Component @this, out T result) where T : Component
        {
            if (@this != null)
            {
                return @this.gameObject.TryGetComponentNoAlloc<T>(out result);
            }
            else
            {
                result = null;
                return false;
            }
        }
        #endregion

        public static bool FindComponent<T>(this Component @this)
        {
            if (@this != null)
            {
                var t = @this.GetComponent<T>();
                return t != null;
            }
            return false;
        }

        public static T FindComponent<T>(this Component @this, string name)
        {
            if (@this != null)
            {
                Transform child = @this.transform.Find(name);
                if (child != null)
                {
                    var t = child.GetComponent<T>();
                    return t;
                }
            }
            return default;
        }

        public static bool FindComponent<T>(this GameObject @this)
        {
            if (@this != null)
            {
                var t = @this.GetComponent<T>();
                return t != null;
            }
            return false;
        }

        public static T GetComponentInParent<T>(this Component @this, bool meToo)
        {
            T result = default;
            if (@this == null)
            {
                return result;
            }

            if (meToo)
            {
                result = @this.GetComponent<T>();
            }
            if (result == null)
            {
                result = @this.GetComponentInParent<T>();
            }

            return result;
        }

        public static bool TryGetComponentInChildren<T>(this Component @this, out T result, bool onlyChildren = true)
        {
            if (@this == null)
            {
                result = default;
                return false;
            }

            result = onlyChildren ? @this.transform.GetChildInHierarchy<T>(true) : @this.GetComponentInChildren<T>();
            return result != null;
        }

        public static bool IsNotNullAndTryGetComponent<T>(this Component @this, out T result)
        {
            if (@this == null)
            {
                result = default;
                return false;
            }

            return @this.TryGetComponent<T>(out result);
        }

        public static bool TryGetComponents<T>(this Component @this, out T[] result)
        {
            if (@this == null)
            {
                result = default;
                return false;
            }

            result = @this.GetComponents<T>();
            return result.IsAlmostSpecificCount();
        }

        public static T AddOnlyOneComponent<T>(this GameObject obj) where T : Component
        {
            T aux = null;

            if (obj != null)
            {
                if (obj.TryGetComponent<T>(out aux))
                {
                    return aux;
                }
                return obj.AddComponent<T>();
            }
            return aux;
        }

        public static T AddOnlyOneComponent<T>(this Component component) where T : Component
        {
            return component != null ? AddOnlyOneComponent<T>(component.gameObject) : null;
        }

        public static bool TryAddOnlyOneComponent<T>(this GameObject obj, out T result) where T : Component
        {
            result = AddOnlyOneComponent<T>(obj);
            return result != null;
        }

        public static bool TryAddOnlyOneComponent<T>(this Component component, out T result) where T : Component
        {
            result = null;
            return component != null && TryAddOnlyOneComponent<T>(component.gameObject, out result);
        }

        public static T[] GetComponentsInChildren<T>(this Component @this, bool onlyChildren = false, bool includeInactive = true)
        {
            var result = @this.GetComponentsInChildren<T>(includeInactive);
            if (onlyChildren)
            {
                var thisComponents = @this.GetComponents<T>();
                result = result.Where(val => !Array.Exists(thisComponents, e => e.Equals(val))).ToArray();
            }

            return result;
        }

        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
                }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static T AddAndCopyComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        public static Component AddAndCopyComponent(this GameObject go, Component toAdd)
        {
            return go.AddComponent(toAdd.GetType()).GetCopyOf(toAdd);
        }

        public static Component[] GetAllComponents(this GameObject go)
        {
            return go.GetComponents(typeof(Component));
        }

        public static T AddComponent<T>(this GameObject obj, string param1, object value1) where T : Component
        {
            T container = obj.AddComponent<T>();

            ReflectionExtend.SetFieldOrPropertyOrMethod(container, param1, value1);

            return container;
        }

        public static T AddComponent<T>(this GameObject obj, ReflectionParameter parameter1) where T : Component
        {
            T container = obj.AddComponent<T>();

            ReflectionExtend.SetValue(container, parameter1);

            return container;
        }

        public static T AddComponent<T>(this GameObject obj, ReflectionParameter parameter1, ReflectionParameter parameter2) where T : Component
        {
            T container = obj.AddComponent<T>();

            ReflectionExtend.SetValue(container, parameter1);
            ReflectionExtend.SetValue(container, parameter2);

            return container;
        }

        public static T AddComponent<T>(this GameObject obj, ReflectionParameter parameter1, ReflectionParameter parameter2, ReflectionParameter parameter3) where T : Component
        {
            T container = obj.AddComponent<T>();

            ReflectionExtend.SetValue(container, parameter1);
            ReflectionExtend.SetValue(container, parameter2);
            ReflectionExtend.SetValue(container, parameter3);

            return container;
        }
    }
}