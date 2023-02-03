using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class SlotArray<T> : IEnumerable<T>
    {
        #region Private fields
        private readonly T[] _array;
        private int _currentIndex = -1;
        #endregion

        #region Construcotrs
        public SlotArray(int length)
        {
            _array = new T[length];
        }
        #endregion

        #region Property
        public T this[int index]
        {
            get { return Get(index); }
        }

        public int Length { get { return _currentIndex + 1; } }
        #endregion

        #region Public Methods
        public bool IsEmpty()
        {
            return _array != null || _currentIndex < 0;
        }

        public void Add(T element)
        {
            if (_array == null)
            {
                return;
            }

            int length = _array.Length;
            _currentIndex++;
            if (_currentIndex == length)
            {
                for (int i = 1; i < length; i++)
                {
                    _array[i - 1] = _array[i];
                }
                _currentIndex--;
            }

            _array[_currentIndex] = element;
        }

        public T Get()
        {
            if (_array == null)
            {
                return default;
            }

            return Get(_currentIndex);
        }

        public T Get(int index)
        {
            if (_array == null || _currentIndex < 0)
            {
                return default;
            }

            index = Mathf.Clamp(index, 0, _array.Length - 1);
            return _array[index];
        }

        public void Reset()
        {
            _currentIndex = -1;
        }
        #endregion

        #region Interfaces
        public override string ToString()
        {
            string result = string.Empty;
            for (int i = 0; i < Length; ++i)
            {
                result += _array[i] + "; ";
            }
            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i <= _currentIndex; ++i)
            {
                yield return _array[_currentIndex];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }

}