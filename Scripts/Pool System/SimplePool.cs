using Pearl.Testing;
using System;
using System.Collections.Generic;

namespace Pearl
{
    public class SimplePool<T> where T : class, IReset
    {
        #region Private fields
        private readonly List<T> _poolListActive = new();
        private readonly List<T> _poolListDisactive = new();
        private readonly bool _useMax;
        #endregion

        #region Constructors
        public SimplePool(in bool useMax, in uint max = 10, params object[] parms)
        {
            _useMax = useMax;

            for (int i = 0; i < max; i++)
            {
                _poolListDisactive.Add(CreateInstance(parms));
            }
        }
        #endregion

        #region Public methods
        public T Get(params object[] vars)
        {
            if (_poolListActive == null || _poolListDisactive == null)
            {
                return default;
            }

            if (_poolListDisactive.Count <= 0)
            {
                if (!_useMax)
                {
                    var instance = CreateInstance(vars);
                    _poolListDisactive.Add(instance);
                }
                else if (_poolListActive.IsAlmostSpecificCount())
                {
                    Remove(_poolListActive[0]);
                }
            }

            if (_poolListDisactive.RemoveAt(0, out T obj))
            {
                _poolListActive.Add(obj);
                obj.ResetElement();
                return obj;
            }

            return default;
        }

        public bool Remove(in T obj)
        {
            if (obj == null || _poolListActive == null || _poolListDisactive == null)
            {
                return false;
            }

            bool success = _poolListActive.Remove(obj);
            if (success)
            {
                obj.DisableElement();
                _poolListDisactive.Add(obj);
            }
            else
            {
                LogManager.LogWarning("There isn't obj in ActiveList");
            }
            return success;
        }

        public bool Remove(in Predicate<T> predicate)
        {
            if (_poolListActive == null)
            {
                return false;
            }


            List<T> listObj = _poolListActive.FindAll(predicate);

            if (listObj.IsAlmostSpecificCount())
            {
                foreach (var obj in listObj)
                {
                    Remove(obj);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region Private methods
        private T CreateInstance(params object[] parms)
        {
            return ReflectionExtend.CreateInstance<T>(parms);
        }
        #endregion
    }
}
