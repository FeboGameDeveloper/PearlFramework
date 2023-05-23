using System.Collections;
using System.Collections.Generic;

namespace Pearl
{
    public class IndexManager
    {
        private int _nextIndex = int.MinValue;
        private readonly List<int> _listIndexFree = new();

        public int GetIndex()
        {
            int aux;
            if (_listIndexFree.Count == 0)
            {
                aux = _nextIndex;
                _nextIndex++;
            }
            else
            {
                aux = _listIndexFree[0];
                _listIndexFree.RemoveAt(0);
            }

            return aux;
        }

        public void FreeIndex(in int index)
        {
            if (_nextIndex == int.MinValue)
            {
                return;
            }

            int currentIndex = _nextIndex - 1;

            if (index == currentIndex)
            {
                _nextIndex--;
            }
            else if (index < currentIndex && !_listIndexFree.Contains(index))
            {
                _listIndexFree.Add(index);
                _listIndexFree.Sort();
            }
        }
    }
}
