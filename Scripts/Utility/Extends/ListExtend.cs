using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pearl
{
    public static class ListExtend
    {
        public static TSource GetHead<TSource>(this List<TSource> @this)
        {
            return @this != null ? @this[0] : default;
        }

        public static TSource GetTail<TSource>(this List<TSource> @this)
        {
            return @this != null ? @this[@this.Count - 1] : default;
        }

        public static void RemoveTail<TSource>(this List<TSource> @this)
        {
            @this?.RemoveAt(@this.Count - 1);
        }

        public static List<TSource> SortInverse<TSource>(this List<TSource> @this)
        {
            return @this?.OrderByDescending(x => x).ToList();
        }


        public static bool RemoveAt<TSource>(this List<TSource> list, int index, out TSource elementRemoved)
        {
            elementRemoved = default;
            if (list != null && list.Count > index)
            {
                elementRemoved = list[index];
                list.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void AddRange<TSource>(this List<TSource> @this, params IEnumerable<TSource>[] sources)
        {
            if (@this != null)
            {
                foreach (var source in sources)
                {
                    @this.AddRange(source);
                }
            }
        }

        public static void ComplexInsert<Type>(this List<Type> @this, int i, Type element)
        {
            if (@this == null)
            {
                return;
            }

            if (i > 0)
            {
                i = Mathf.Clamp(i, 0, @this.Count - 1);
                @this.Insert(i, element);
            }
            else
            {
                @this.Add(element);
            }
        }

        public static List<TResult> CreateList<TSource, TResult>(this List<TSource> @this, Func<TSource, TResult> func)
        {
            if (@this == null)
            {
                return null;
            }

            var collection = @this.CreateCollection(func);
            return collection?.Cast<TResult>().ToList();
        }

        public static List<T> CreateList<T>(params T[] elements)
        {
            var collection = CollectionExtend.CreateCollection(elements);
            return collection?.Cast<T>().ToList();
        }

        public static void AddInSort<T>(this List<T> @this, T newElement)
        {
            if (@this != null)
            {
                @this.Add(newElement);
                @this.Sort();
            }
        }

        public static List<TSource> FilterList<TSource>(this List<TSource> list, Func<TSource, bool> filter)
        {
            if (list == null)
            {
                return null;
            }

            var collection = list.FilterCollection(filter);
            return collection?.Cast<TSource>().ToList();
        }

        public static bool Remove<TSource>(this List<TSource> list, Predicate<TSource> match)
        {
            int index = list.FindIndex(match);
            if (index < 0)
            {
                return false;
            }

            list.RemoveAt(index);
            return true;
        }

        public static bool Find<TSource>(this List<TSource> list, Predicate<TSource> match, out TSource result)
        {
            foreach (var aux in list)
            {
                if (match.Invoke(aux))
                {
                    result = aux;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
