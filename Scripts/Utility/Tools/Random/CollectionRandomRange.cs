using Pearl.Testing;
using System.Collections.Generic;

namespace Pearl
{
    //Collezione dove il put retituisce un oggetto randomico (consumabile o meno)
    public class CollectionRandomRange<T>
    {
        private ICollection<T> _collection;
        private RandomRange _randomRange;

        public CollectionRandomRange(ICollection<T> collection)
        {
            Set(collection);
        }

        public int Count { get { return _randomRange.Count; } }

        public void Set(ICollection<T> collection)
        {
            _randomRange = new RandomRange(collection.Count - 1);
            _collection = new List<T>(collection);
        }

        public bool IsFinish()
        {
            return _randomRange != null ? _randomRange.IsFinish() : true;
        }


        public T Put(bool notRemove = false)
        {
            if (_randomRange == null || _collection == null)
            {
                LogManager.LogWarning("Error");
                return default;
            }

            if (!IsFinish())
            {
                int randomIndex = _randomRange.Put(notRemove);
                var rightItem = _collection.Get(randomIndex);
                return rightItem;
            }

            LogManager.LogWarning("The range is finished");
            return default;
        }

        public bool Remove(T item)
        {
            if (_randomRange == null || _collection == null)
            {
                LogManager.LogWarning("Error");
                return false;
            }

            int index = _collection.AtIndex(item);
            return _randomRange.RemoveElement(index);
        }
    }
}
