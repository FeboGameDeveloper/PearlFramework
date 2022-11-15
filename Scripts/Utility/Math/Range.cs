using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct ElementRange
    {
        [Tooltip("The value")]
        public float value;
        [Tooltip("include value in the range?")]
        public bool includeExtreme;

        public ElementRange(in ElementRange elementRange)
        {
            this.value = elementRange.value;
            this.includeExtreme = elementRange.includeExtreme;
        }

        public ElementRange(float value, bool includeExtreme)
        {
            this.value = value;
            this.includeExtreme = includeExtreme;
        }

        public ElementRange(float value)
        {
            this.value = value;
            this.includeExtreme = true;
        }

        public void Set(float value, bool includeExtreme = true)
        {
            this.value = value;
            this.includeExtreme = includeExtreme;
        }

        public void Set(in ElementRange elementRange)
        {
            Set(elementRange.value, elementRange.includeExtreme);
        }
    }

    /// <summary>
    /// A structure that represents a range of numbers
    /// </summary>
    [Serializable]
    public class Range
    {
        #region private Fields
        [SerializeField]
        private ElementRange minElement = default;
        [SerializeField]
        private ElementRange maxElement = default;
        #endregion

        public float Min
        {
            get
            {
                float value = minElement.value;
                return minElement.includeExtreme ? value : value + float.MinValue;
            }
            set
            {
                minElement.value = value;
            }
        }

        public float Max
        {
            get
            {
                float value = maxElement.value;
                return minElement.includeExtreme ? value : value - float.MinValue;
            }
            set
            {
                maxElement.value = value;
            }
        }

        public float LeftRange { get { return minElement.value; } }
        public float RightRange { get { return maxElement.value; } }

        public ElementRange MinElement { get { return minElement; } }
        public ElementRange MaxElement { get { return maxElement; } }

        #region Constructors
        public Range(ElementRange min, ElementRange max)
        {
            Set(min, max);
        }

        public Range(float min, ElementRange max)
        {
            Set(new ElementRange(min), max);
        }

        public Range(ElementRange min, float max)
        {
            Set(min, new ElementRange(max));
        }


        public Range(float min, float max)
        {
            Set(min, max);
        }

        public Range(Range range)
        {
            Set(range.minElement, range.maxElement);
        }

        public Range()
        {
            Set(0, 1);
        }
        #endregion

        #region Set
        public void Set(in ElementRange min, in ElementRange max)
        {
            Set(min.value, max.value, min.includeExtreme, max.includeExtreme);
        }

        public void Set(float min, float max, bool minIncludeExtreme = true, bool maxIncludeExtreme = true)
        {
            var aux = Math.Min(min, max);
            max = Math.Max(min, max);
            min = aux;

            minElement.Set(min, minIncludeExtreme);
            maxElement.Set(max, maxIncludeExtreme);
        }
        #endregion

        #region Contains
        public bool Contains(float value)
        {
            return !IsIncreased(value) && !IsDecreased(value);
        }

        public bool Contains(Range value)
        {
            return Contains(value.Min) && Contains(value.Max);
        }
        #endregion

        public float GetMediumPoint()
        {
            return (Max - Min) / 2f;
        }

        public bool IsIncreased(float x)
        {
            return maxElement.includeExtreme ? x > RightRange : x >= RightRange;
        }

        public bool IsDecreased(float x)
        {
            return minElement.includeExtreme ? x < LeftRange : x <= RightRange;
        }

        #region Override Methods
        public override bool Equals(object obj)
        {
            Range otherRange = (Range)obj;
            if (otherRange != null)
            {
                return otherRange.minElement.includeExtreme == this.minElement.includeExtreme &&
                    otherRange.maxElement.includeExtreme == this.maxElement.includeExtreme &&
                    otherRange.minElement.value == this.minElement.value &&
                    otherRange.maxElement.value == this.maxElement.value;
            }
            return false;
        }

        public override string ToString()
        {
            string minBrackets = minElement.includeExtreme ? "[" : "(";
            string maxBrackets = maxElement.includeExtreme ? "]" : ")";
            return minBrackets + minElement.value + "," + maxElement.value + maxBrackets;
        }

        public override int GetHashCode()
        {
            var hashCode = 1143683652;
            hashCode = hashCode * -1521134295 + minElement.GetHashCode();
            hashCode = hashCode * -1521134295 + maxElement.GetHashCode();
            hashCode = hashCode * -1521134295 + Min.GetHashCode();
            hashCode = hashCode * -1521134295 + Max.GetHashCode();
            return hashCode;
        }
        #endregion
    }
}