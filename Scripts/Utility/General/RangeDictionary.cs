using System.Collections.Generic;

namespace Pearl
{
    public class RangeDictionary<T>
    {
        private readonly Dictionary<float, T> dict = new();

        public void Add(float round, T value)
        {
            dict.Add(round, value);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public void Remove(float round)
        {
            dict.Remove(round);
        }

        public T Find(float value)
        {
            var values = dict.Keys;

            if (values == null)
            {
                return default;
            }

            var arr = values.CreateArray(((x) => x));
            var key = MathfExtend.Nearest(value, arr);
            return dict[key];
        }

    }
}
