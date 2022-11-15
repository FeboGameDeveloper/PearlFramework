using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pearl
{
    public static class ArrayExtend
    {
        public static Array ConvertArray<T>(T[] array, Type newType)
        {
            Array result = Array.CreateInstance(newType, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                result.SetValue(array[i], i);
            }
            return result;
        }

        public static TSource[] FilterArray<TSource>(this ICollection<TSource> @this, Func<TSource, bool> filter)
        {
            var list = CollectionExtend.FilterCollection(@this, filter);
            return list.ToArray();
        }

        public static T[] ConvertNumbersAndVectors<T>(params float[] values)
        {
            ICollection<T> collection = CollectionExtend.ConvertNumbersAndVectors<T>(values);
            if (collection != null && collection.Cast<T>().IsNotNull(out var enumerable))
            {
                return enumerable.ToArray();
            }
            return null;
        }

        public static T[] ConvertNumbersAndVectors<T>(params Vector2[] values)
        {
            ICollection<T> collection = CollectionExtend.ConvertNumbersAndVectors<T>(values);
            if (collection != null && collection.Cast<T>().IsNotNull(out var enumerable))
            {
                return enumerable.ToArray();
            }
            return null;
        }

        public static T[] ConvertNumbersAndVectors<T>(params Vector3[] values)
        {
            ICollection<T> collection = CollectionExtend.ConvertNumbersAndVectors<T>(values);
            if (collection != null && collection.Cast<T>().IsNotNull(out var enumerable))
            {
                return enumerable.ToArray();
            }
            return null;
        }

        public static T[] ConvertNumbersAndVectors<T>(params Vector4[] values)
        {
            ICollection<T> collection = CollectionExtend.ConvertNumbersAndVectors<T>(values);
            if (collection != null && collection.Cast<T>().IsNotNull(out var enumerable))
            {
                return enumerable.ToArray();
            }
            return null;
        }

        public static T[] ConvertNumbersAndVectors<T>(params Color[] values)
        {
            ICollection<T> collection = CollectionExtend.ConvertNumbersAndVectors<T>(values);
            if (collection != null && collection.Cast<T>().IsNotNull(out var enumerable))
            {
                return enumerable.ToArray();
            }
            return null;
        }


        public static T[] Clone<T>(this T[] @this)
        {
            if (@this != null)
            {
                return (T[])@this.Clone();
            }

            return null;
        }

        public static void InsertAndLeftShift<T>(this T[] @this, T element, int index)
        {
            for (int i = 0; i < @this.Length; i++)
            {
                var aux = @this[i];
                if (i != 0)
                {
                    @this[i - 1] = aux;
                }

                if (i == index)
                {
                    @this[i] = element;
                    break;
                }
            }
        }

        public static void InsertAndRightShift<T>(this T[] @this, T element, int index)
        {
            for (int i = @this.Length; i >= 0; i--)
            {
                if (i == index)
                {
                    @this[i] = element;
                    break;
                }

                if (i - 1 >= 0)
                {
                    @this[i] = @this[i - 1];
                }
            }
        }

        public static T[] CreateArray<T>(params T[] elements)
        {
            return elements;
        }

        public static TResult[] CreateArray<TSource, TResult>(this ICollection<TSource> @this, Func<TSource, TResult> func)
        {
            if (@this == null)
            {
                return null;
            }

            var collection = @this.CreateCollection(func);
            return collection?.Cast<TResult>().ToArray();
        }

        public static T[] SubArray<T>(this T[] @this, int offset, uint length)
        {
            if (@this != null && length > 0 && @this.Length > offset + (length - 1))
            {
                T[] result = new T[length];
                Array.Copy(@this, offset, result, 0, length);
                return result;
            }
            return null;
        }

        public static T[] ReturnNewArrayWithoutElements<T>(in T[] original, params T[] elements)
        {
            if (original != null && elements != null && elements.Length > 0)
            {
                List<T> valueList = new(original);
                foreach (T element in elements)
                {
                    if (element != null)
                    {
                        while (valueList.Remove(element)) { }
                    }
                }
                return valueList.ToArray();
            }
            return original;
        }

    }
}
