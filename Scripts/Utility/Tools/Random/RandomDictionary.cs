using Pearl.Testing;
using System.Collections.Generic;

namespace Pearl
{
    //Dizionario con chiavi generate automaticamente randomiche
    public class RandomDictionary<T>
    {
        private readonly Dictionary<int, T> _randomDictionary;
        private RandomRange _randomRange;

        public RandomDictionary(int maxKeys = 1000)
        {
            _randomRange = new RandomRange(maxKeys);
            _randomDictionary = new Dictionary<int, T>();
        }

        public int Add(T element)
        {
            int key = -1;
            if (_randomRange != null && _randomDictionary != null)
            {
                key = _randomRange.Put();
                _randomDictionary.Add(key, element);
            }
            return key;
        }

        public bool Remove(int key)
        {
            if (_randomRange == null || _randomDictionary == null)
            {
                return false;
            }

            _randomRange.AddElement(key);
            return _randomDictionary.Remove(key);
        }

        public T Get(int key)
        {
            if (_randomDictionary.IsNotNullAndTryGetValue(key, out T result))
            {
                return result;
            }

            LogManager.LogWarning("The key is wrong");
            return default;
        }
    }
}