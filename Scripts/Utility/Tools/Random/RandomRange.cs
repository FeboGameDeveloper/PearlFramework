using Pearl.Testing;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    //Lista di interi dove il put retituisce un intero randomico (consumabile o meno)
    public class RandomRange
    {
        private readonly Dictionary<int, int> _dictionary = new Dictionary<int, int>();
        private int maxIndex;

        public RandomRange(int min, int max)
        {
            Set(min, max);
        }

        public RandomRange(int max)
        {
            Set(0, max);
        }

        public RandomRange()
        {
        }

        public int Count { get { return _dictionary.Count; } }

        public void Set(int min, int max)
        {
            if (_dictionary != null)
            {
                for (int i = min, j = 0; i <= max; i++, j++)
                {
                    _dictionary.Add(j, i);
                }
                this.maxIndex = _dictionary.Count - 1;
            }
        }

        public void AddElement(int value)
        {
            if (_dictionary != null)
            {
                maxIndex++;
                _dictionary.Add(maxIndex, value);
            }
        }

        public bool IsFinish()
        {
            return maxIndex < 0;
        }

        public int Put(bool notRemove = false)
        {
            if (_dictionary == null)
            {
                LogManager.LogWarning("Error");
                return default;
            }

            if (!IsFinish())
            {
                int randomIndex = Random.Range(0, maxIndex + 1);
                int value = _dictionary[randomIndex];
                if (!notRemove)
                {
                    RemoveElement(value);
                }
                return value;
            }
            LogManager.LogWarning("The range is finished");
            return -1;
        }

        public bool RemoveElement(int value)
        {
            bool result = false;
            if (_dictionary != null && _dictionary.TryGetKey(value, out int key))
            {
                result = _dictionary.Remove(key);

                if (maxIndex > 0 && key != maxIndex)
                {
                    int valueInMaxIndex = _dictionary[maxIndex];
                    result = _dictionary.Remove(maxIndex);
                    _dictionary.Add(key, valueInMaxIndex);
                }
                maxIndex--;
            }
            return result;
        }
    }
}
