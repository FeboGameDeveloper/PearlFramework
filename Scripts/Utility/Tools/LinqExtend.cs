using System;
using System.Collections.Generic;
using System.Linq;

namespace Pearl
{
    public static class LinqExtend
    {
        public static bool Find<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, out TSource result)
        {
            result = default;
            try
            {
                result = source.Single<TSource>(predicate);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            return source.ComparerBy(selector, comparer, ComparerEnum.Minor);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            return source.ComparerBy(selector, comparer, ComparerEnum.Major);
        }

        private static TSource ComparerBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer, ComparerEnum comparerEnum)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");

            if (comparer == null)
            {
                comparer = Comparer<TKey>.Default;
            }

            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var value = sourceIterator.Current;
                var valueKey = selector(value);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if ((comparerEnum == ComparerEnum.Minor && comparer.Compare(candidateProjected, valueKey) < 0) ||
                        (comparerEnum == ComparerEnum.Major && comparer.Compare(candidateProjected, valueKey) > 0))
                    {
                        value = candidate;
                        valueKey = candidateProjected;
                    }
                }
                return value;
            }
        }
    }
}
