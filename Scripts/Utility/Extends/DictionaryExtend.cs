using System;
using System.Collections.Generic;

namespace Pearl
{
    /// <summary>
    /// This class extends the dictonary
    /// </summary>
    public static class DictionaryExtend
    {
        #region Public Methods
        /// <summary>
        /// This method update a value with specific key, 
        /// if there isn't key, the method create new pair key-value
        /// </summary>
        /// <param name = "key">The key of value.</param>
        /// <param name = "value">The value.</param>
        public static bool Update<T, F>(this IDictionary<T, F> dictonary, T key, F value)
        {
            if (dictonary == null)
            {
                return false;
            }


            if (value != null)
            {
                if (dictonary.ContainsKey(key))
                {
                    dictonary[key] = value;
                }
                else
                {
                    dictonary.Add(key, value);
                }
                return true;
            }
            else
            {
                return dictonary.Remove(key);
            }
        }

        public static void AddOnce<T, F>(this IDictionary<T, F> dictonary, T key, F value)
        {
            if (!dictonary.ContainsKey(key))
            {
                dictonary.Add(key, value);
            }
        }

        public static T[] GetSpecificKeys<T, F>(this IDictionary<T, F> dictonary, Predicate<T> filter)
        {
            List<T> result = new List<T>();

            if (dictonary == null || filter == null || result == null)
            {
                return null;
            }

            foreach (var element in dictonary)
            {
                if (filter.Invoke(element.Key))
                {
                    result.Add(element.Key);
                }
            }

            return result.ToArray();
        }

        public static bool IsNotNullAndTryGetValue<T, F>(this IDictionary<T, F> dictonary, T key, out F result)
        {
            result = default;
            return dictonary != null && dictonary.TryGetValue(key, out result);
        }

        public static bool TryGetKey<T, F>(this IDictionary<T, F> @this, F value, out T result)
        {
            if (@this == null)
            {
                result = default;
                return false;
            }

            foreach (var key in @this.Keys)
            {
                if (@this[key].Equals(value))
                {
                    result = key;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool ContainsValue<T, F>(this IDictionary<T, F> dictonary, F value)
        {
            if (dictonary == null)
            {
                return false;
            }

            var list = dictonary.Values;

            if (list == null)
            {
                return false;
            }

            return list.Contains(value);
        }
        #endregion
    }
}
