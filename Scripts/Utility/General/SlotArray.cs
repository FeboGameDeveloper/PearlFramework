using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    //Array con una certa capienza. Se viene aggiunto un elemento, l'array o si riempie o viene traslato a destra.
    //Il primo elemento è sempre qullo più giovane.
    //L'ultimo elemento è il più vecchio.

    public class SlotArray<T> : IEnumerable<T>
    {
        private T[] array;
        private int _currentIndex = -1;

        public SlotArray(int length)
        {
            array = new T[length];
        }

        public bool IsEmpty()
        {
            return array != null || _currentIndex < 0;
        }


        public void Add(T element)
        {
            if (array == null)
            {
                return;
            }

            int length = array.Length;
            _currentIndex++;
            if (_currentIndex == length)
            {
                for (int i = 1; i < length; i++)
                {
                    array[i - 1] = array[i];
                }
                _currentIndex--;
            }

            array[_currentIndex] = element;
        }

        public T Remove()
        {
            if (array == null || _currentIndex < 0)
            {
                return default;
            }

            array[_currentIndex] = default;
            _currentIndex--;
            _currentIndex = Mathf.Clamp(_currentIndex, 0, array.Length - 1);
            return array[_currentIndex];
        }

        public T Get()
        {
            if (array == null)
            {
                return default;
            }

            return Get(_currentIndex);
        }

        public T Get(int index)
        {
            if (array == null || _currentIndex < 0)
            {
                return default;
            }

            index = Mathf.Clamp(index, 0, array.Length - 1);
            return array[index];
        }

        public void Set(int index)
        {
            index = Mathf.Clamp(index, -1, array.Length);

            if (index < _currentIndex)
            {
                while (index != _currentIndex)
                {
                    Remove();
                }
            }
        }

        public void Reset()
        {
            Set(-1);
        }

        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i <= _currentIndex; ++i)
            {
                result += array[i] + "; ";
            }
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i <= _currentIndex; ++i)
            {
                yield return array[_currentIndex];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}