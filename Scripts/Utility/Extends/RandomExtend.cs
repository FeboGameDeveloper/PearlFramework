using Pearl.Testing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct StatisticalElement<T>
    {
        public T value;
        public float statisticValue;

        public StatisticalElement(T value, float statisticValue)
        {
            this.value = value;
            this.statisticValue = Mathf.Abs(statisticValue);
        }
    }

    public static class RandomExtend
    {
        public static StatisticalElement<T> GetStatisticalElement<T>(T element, float statistical)
        {
            return new StatisticalElement<T>(element, statistical);
        }

        public static bool GetRandomBool(in float percent)
        {
            float randomValue = UnityEngine.Random.value;
            return randomValue <= percent;
        }

        public static T GetRandomCollection<T, F>(T collection, int numberElements, params F[] exception) where T : ICollection<F>, new()
        {
            if (collection == null)
            {
                return collection;
            }

            var randomRange = new CollectionRandomRange<F>(collection);

            if (exception != null)
            {
                foreach (var item in exception)
                {
                    randomRange.Remove(item);
                }
            }

            T result = new();
            numberElements = Mathf.Clamp(numberElements, 0, collection.Count);
            if (randomRange != null)
            {
                for (int i = 0; i < numberElements; i++)
                {
                    result.Add(randomRange.Put());
                }
            }
            return result;
        }

        private static T GetRandomElement<T>(in ICollection<T> values, Vector2[] rangeStatistics)
        {
            if (values.IsEmpty() || rangeStatistics.Length != values.Count)
            {
                LogManager.LogWarning("The array is null or empty");
                return default;
            }

            if (values.Count <= 0)
            {
                LogManager.LogWarning("The array is empty");
            }

            float randomValue = UnityEngine.Random.value;

            for (int i = 0; i < rangeStatistics.Length; i++)
            {
                var element = rangeStatistics[i];
                if (randomValue > element.x && randomValue <= element.y)
                {
                    return values.Get(i);
                }
            }


            return default;
        }

        public static T GetRandomElement<T>(in ICollection<T> values, List<T> exception)
        {
            return GetRandomElement(values, exception.ToArray());
        }

        public static T GetRandomElement<T>(in ICollection<T> values, params T[] exception)
        {
            if (values == null || values.Count < 0)
            {
                LogManager.LogWarning("The array is null or empty");
                return default;
            }

            ICollection<T> aux = new List<T>(values);
            aux = aux.ReturnNewCollectionsWithoutElements<T>(exception);

            if (!aux.IsAlmostSpecificCount())
            {
                LogManager.LogWarning("The array is null or empty");
                return default;
            }

            float value = 1f / aux.Count;
            Vector2[] rangeStatistics = new Vector2[aux.Count];

            float currentRange = 0;
            float oldRange = 0;
            for (int i = 0; i < rangeStatistics.Length; i++)
            {
                currentRange += value;
                rangeStatistics[i] = new Vector2(oldRange, currentRange);
                oldRange = currentRange;
            }

            return GetRandomElement(aux, rangeStatistics);
        }

        public static T GetRandomElement<T>(in ICollection<StatisticalElement<T>> values, params T[] exception)
        {
            if (values == null || values.Count < 0)
            {
                LogManager.LogWarning("The array is null or empty");
                return default;
            }

            List<StatisticalElement<T>> valuesCopy = new(values);

            if (exception.IsAlmostSpecificCount())
            {
                List<T> exceptionList = new(exception);
                for (int i = valuesCopy.Count - 1; i < 0; i--)
                {
                    if (exceptionList.Contains(valuesCopy[i].value))
                    {
                        valuesCopy.RemoveAt(i);
                    }
                }

                if (valuesCopy == null || valuesCopy.Count < 0)
                {
                    LogManager.LogWarning("The array is null or empty");
                    return default;
                }
            }

            ICollection<T> aux = valuesCopy.CreateCollection((x) => x.value);

            float maxStatitics = 0;
            foreach (var element in valuesCopy)
            {
                maxStatitics += element.statisticValue;
            }

            Vector2[] rangeStatistics = new Vector2[valuesCopy.Count];

            float currentRange = 0;
            float oldRange = -0.1f;
            for (int i = 0; i < rangeStatistics.Length; i++)
            {
                currentRange += valuesCopy[i].statisticValue / maxStatitics;
                rangeStatistics[i] = new Vector2(oldRange, currentRange);
                oldRange = currentRange;
            }

            return GetRandomElement<T>(aux, rangeStatistics);
        }

        public static int RandomRangeInt(Range range)
        {
            return UnityEngine.Random.Range((int)range.Min, (int)range.Max + 1);
        }

        public static float RandomRangeFloat(Range range)
        {
            return UnityEngine.Random.Range(range.Min, range.Max);
        }

        public static int RandomVectorInt(Vector2 range)
        {
            return UnityEngine.Random.Range((int)range.x, (int)range.y + 1);
        }

        public static float RandomVectorFloat(Vector2 range)
        {
            return UnityEngine.Random.Range(range.x, range.y);
        }

        public static float GetRandomAngle()
        {
            return UnityEngine.Random.Range(0f, 360f);
        }
    }
}
