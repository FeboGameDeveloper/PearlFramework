using Pearl.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pearl
{
    public static class CollectionExtend
    {
        public static bool IsOneChildNull<T>(this ICollection<T> @this)
        {
            if (@this != null)
            {
                foreach (var element in @this)
                {
                    if (element == null)
                    {
                        return true;
                    }
                }
                return false;
            }

            return true;
        }

        public static bool IsIndexValue<TSource>(this ICollection<TSource> @this, int index)
        {
            return @this != null && index >= 0 && index < @this.Count;
        }


        public static void ClearAndAdd<TSource>(this ICollection<TSource> @this, TSource element)
        {
            @this.Clear();
            @this.Add(element);
        }

        public static bool IsEqualContainer<Type>(this ICollection<Type> @this, ICollection<Type> anotherList)
        {
            return @this.All(anotherList.Contains) && @this.Count == anotherList.Count;
        }

        public static bool IsContains<Type>(this ICollection<Type> @this, ICollection<Type> anotherList)
        {
            return @this.All(anotherList.Contains);
        }

        public static bool AreChildrenNotNull<T>(this ICollection<T> @this)
        {
            return !IsOneChildNull(@this);
        }

        public static bool ThereIsntNullInChilds<T>(this ICollection<T> @this) where T : class
        {
            return !@this.IsOneChildNull();
        }

        #region ConvertNumbersInVectors
        public static ICollection<T> ConvertNumbersAndVectors<T>(params float[] values)
        {
            if (values != null)
            {
                if (typeof(T) == typeof(float))
                {
                    return (ICollection<T>)(object)values;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    ICollection<Vector2> finalValues = new List<Vector2>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector2(values[i], 0));
                    }
                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    ICollection<Vector3> finalValues = new List<Vector3>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector3(values[i], 0, 0));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    ICollection<Vector4> finalValues = new List<Vector4>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector4(values[i], 0, 0, 0));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
            }
            return null;
        }

        public static ICollection<T> ConvertNumbersAndVectors<T>(params Vector2[] values)
        {
            if (values != null)
            {
                if (typeof(T) == typeof(float))
                {
                    ICollection<float> finalValues = new List<float>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(values[i].x);
                    }
                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    return (ICollection<T>)(object)values;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    ICollection<Vector3> finalValues = new List<Vector3>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector3(values[i].x, values[i].y, 0));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    ICollection<Vector4> finalValues = new List<Vector4>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector4(values[i].x, values[i].y, 0, 0));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
            }
            return null;
        }

        public static ICollection<T> ConvertNumbersAndVectors<T>(params Vector3[] values)
        {
            if (values != null)
            {
                if (typeof(T) == typeof(float))
                {
                    ICollection<float> finalValues = new List<float>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(values[i].x);
                    }
                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    ICollection<Vector2> finalValues = new List<Vector2>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector2(values[i].x, values[i].y));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    return (ICollection<T>)(object)values;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    ICollection<Vector4> finalValues = new List<Vector4>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector4(values[i].x, values[i].y, values[i].z, 0));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
            }
            return null;
        }

        public static ICollection<T> ConvertNumbersAndVectors<T>(params Vector4[] values)
        {
            if (values != null)
            {
                if (typeof(T) == typeof(float))
                {
                    ICollection<float> finalValues = new List<float>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(values[i].x);
                    }
                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    ICollection<Vector2> finalValues = new List<Vector2>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector2(values[i].x, values[i].y));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    ICollection<Vector3> finalValues = new List<Vector3>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector3(values[i].x, values[i].y, values[i].z));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    return (ICollection<T>)(object)values;
                }
            }
            return null;
        }

        public static ICollection<T> ConvertNumbersAndVectors<T>(params Color[] values)
        {
            if (values != null)
            {
                if (typeof(T) == typeof(float))
                {
                    ICollection<float> finalValues = new List<float>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(values[i].r);
                    }
                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    ICollection<Vector2> finalValues = new List<Vector2>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector2(values[i].r, values[i].g));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    ICollection<Vector3> finalValues = new List<Vector3>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector3(values[i].r, values[i].g, values[i].b));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    ICollection<Vector4> finalValues = new List<Vector4>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        finalValues.Add(new Vector4(values[i].r, values[i].g, values[i].b, values[i].a));
                    }

                    return (ICollection<T>)(object)finalValues;
                }
            }
            return null;
        }

        #endregion

        public static string PrintElements<T>(this ICollection<T> @this)
        {
            string result = "{";
            if (@this != null)
            {
                foreach (var element in @this)
                {
                    result += element.ToString() + ", ";

                }
            }

            if (result.Length >= 2)
            {
                result = result.Remove(result.Length - 2);
            }

            result += "}";
            return result;
        }

        public static ICollection<TResult> CreateCollection<TSource, TResult>(this ICollection<TSource> @this, Func<TSource, TResult> func)
        {
            if (@this == null || func == null)
            {
                return default;
            }

            ICollection<TResult> aux = new List<TResult>();

            foreach (var item in @this)
            {
                TResult result = func.Invoke(item);
                aux.Add(result);
            }

            return aux;
        }

        public static ICollection<T> CreateCollection<T>(params T[] elements)
        {
            ICollection<T> newList = new List<T>();
            if (elements == null || elements.Length == 0)
            {
                return newList;
            }
            else
            {
                for (int i = 0; i < elements.Length; i++)
                {
                    newList.Add(elements[i]);
                }

                return newList;
            }
        }

        public static ICollection<TSource> FilterCollection<TSource>(this ICollection<TSource> @this, Func<TSource, bool> filter)
        {
            if (@this == null || filter == null)
            {
                return default;
            }

            ICollection<TSource> result = new List<TSource>();

            foreach (var item in @this)
            {
                if (filter.Invoke(item))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public static ICollection<T> GetRandomSubPart<T>(this ICollection<T> @this, int subElement, params T[] exception)
        {
            List<T> result = new();

            var newCollection = @this.ReturnNewCollectionsWithoutElements(exception);
            subElement = Mathf.Clamp(subElement, 0, newCollection.Count);

            CollectionRandomRange<T> randomRange = new CollectionRandomRange<T>(newCollection);

            for (int i = 0; i < subElement; i++)
            {
                result.Add(randomRange.Put());
            }

            return result;
        }

        public static ICollection<T> ReturnNewCollectionsWithoutElements<T>(this ICollection<T> @this, params T[] exception)
        {
            List<T> exceptionList = new(exception);
            List<T> cloneList = new(@this);
            for (int i = 0; i < cloneList.Count; i++)
            {
                T value = cloneList.Get(i);
                if (exceptionList.Contains(value))
                {
                    cloneList.Remove(value);
                    i--;
                }
            }

            return cloneList;
        }

        public static int GetIndexAvoiedProgresion<T>(this ICollection<T> @this, Func<T, int> getIndex, int defaultResult)
        {
            if (getIndex == null || @this == null)
            {
                Debug.LogManager.LogWarning("GetIndexAvoiedProgresion error");
                return default;
            }

            for (int i = 0; i < @this.Count; i++)
            {
                bool iExist = false;
                for (int j = 0; j < @this.Count; j++)
                {
                    if (getIndex.Invoke(@this.Get(j)) == i)
                    {
                        iExist = true;
                        break;
                    }
                }

                if (!iExist)
                {
                    return i;
                }
            }

            return defaultResult;
        }

        public static TypeSibilling GetTypeSybilling<T>(this ICollection<T> @this, int index)
        {
            if (@this == null)
            {
                LogManager.LogWarning("The Collection is null");
                return TypeSibilling.SpecificIndex;
            }

            if (index < 0 || index >= @this.Count)
            {
                LogManager.LogWarning("The index is invalid");
                return TypeSibilling.SpecificIndex;
            }

            if (index == 0)
            {
                return TypeSibilling.First;
            }
            else if (index == @this.Count - 1)
            {
                return TypeSibilling.Last;
            }
            else
            {
                return TypeSibilling.SpecificIndex;
            }
        }

        public static bool IsAlmostSpecificCount<T>(this ICollection<T> @this, int minIndex = 0)
        {
            return @this != null && @this.Count > minIndex && minIndex >= 0;
        }

        public static bool IsEmpty<T>(this ICollection<T> @this)
        {
            return @this == null || @this.Count <= 0;
        }

        public static T Get<T>(this ICollection<T> list, int index)
        {
            if (list != null && index < list.Count && index >= 0)
            {
                T rightItem = default;
                int i = 0;
                foreach (T item in list)
                {
                    if (i == index)
                    {
                        rightItem = item;
                        break;
                    }
                    i++;
                }
                return rightItem;
            }
            LogManager.LogWarning("The index is invalid");
            return default;
        }

        public static void AddOnce<T>(this ICollection<T> @this, T key)
        {
            if (@this != null && key != null && !@this.Contains(key))
            {
                @this.Add(key);
            }
        }

        public static int AtIndex<T>(this ICollection<T> @this, T key)
        {
            int index = 0;

            if (@this != null && key != null)
            {
                foreach (T item in @this)
                {
                    if (key.Equals(item))
                    {
                        break;
                    }
                    index++;
                }
            }

            return index;
        }

        public static void AddOnce<T>(this ICollection<T> @this, params T[] keys)
        {
            if (keys != null)
            {
                foreach (T key in keys)
                {
                    AddOnce(@this, key);
                }
            }
        }

        public static void Remove<T>(this ICollection<T> @this, params T[] keys)
        {
            if (@this != null && keys != null)
            {
                foreach (T key in keys)
                {
                    @this.Remove(key);
                }
            }
        }
    }
}
