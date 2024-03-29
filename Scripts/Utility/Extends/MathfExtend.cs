﻿using Pearl.Debug;
using System;
using System.Globalization;
using UnityEngine;

namespace Pearl
{
    public enum OperationEnum { Sum, Less, Null }

    /// <summary>
    /// A class that extend Mathf class
    /// </summary>
    public static class MathfExtend
    {
        #region Public Methods

        #region float
        public static float Sqr(this float f)
        {
            return f * f;
        }

        public static float SqrRt(this float f)
        {
            return Mathf.Sqrt(f);
        }

        public static bool ApproxZero(this float f)
        {
            return Mathf.Approximately(0, f);
        }

        public static bool Approx(this float f, float f2)
        {
            return Mathf.Approximately(f, f2);
        }

        /// <summary>
        /// The method returns the sign of a number
        /// </summary>
        /// <param name = "value"> The value that will be studied to obtain the sign</param>
        public static int Sign(float value)
        {
            return value > 0 ? 1 : (value < 0 ? -1 : 0);
        }

        public static float Clamp0(float value, float max)
        {
            return Mathf.Clamp(value, 0, max);
        }

        public static bool MajorOrEquals(this float a, float b)
        {
            float result = a - b;
            return result > 0 || result.ApproxZero();
        }

        public static bool MinorOrEquals(this float a, float b)
        {
            float result = a - b;
            return result < 0 || result.ApproxZero();
        }
        #endregion

        public static float Nearest(float value, params float[] values)
        {
            float distance = Mathf.Infinity;
            float result = 0;

            if (values != null)
            {
                foreach (var v in values)
                {
                    float aux = Mathf.Abs(value - v);
                    if (aux < distance)
                    {
                        distance = aux;
                        result = v;
                    }
                }
            }
            return result;
        }
        public static float Lerp(float a, float b, float t, FunctionEnum function)
        {
            var func = FunctionDefinition.GetFunction(function);
            if (func != null)
            {
                float tChanged = func.Invoke(t);
                return Mathf.Lerp(a, b, tChanged);
            }
            return 0;
        }

        public static float Proportion(float numberA, float numberB, float numberC)
        {
            return numberB * numberC / numberA;
        }

        public static bool IsEven(int number)
        {
            return number % 2 == 0;
        }

        public static bool IsOdd(int number)
        {
            return number % 2 != 0;
        }

        public static Func<float, int> GetRound(RoundsEnum roundType)
        {
            return roundType switch
            {
                RoundsEnum.Ceil => Mathf.CeilToInt,
                RoundsEnum.Floor => Mathf.FloorToInt,
                RoundsEnum.Round => Mathf.RoundToInt,
                _ => Mathf.RoundToInt,
            };
        }

        public static int GetRound(float number, RoundsEnum roundType)
        {
            return roundType switch
            {
                RoundsEnum.Ceil => Mathf.CeilToInt(number),
                RoundsEnum.Floor => Mathf.FloorToInt(number),
                RoundsEnum.Round => Mathf.RoundToInt(number),
                _ => Mathf.RoundToInt(number),
            };
        }

        public static float Round(float value, int digits)
        {
            float mult = Mathf.Pow(10.0f, (float)digits);
            return Mathf.Round(value * mult) / mult;
        }

        public static float Truncate(this float number, int sectionDecimal)
        {
            int elevation = 10;
            for (int i = 1; i < sectionDecimal; i++)
            {
                elevation *= 10;
            }

            return Mathf.Floor(number * elevation) / elevation;
        }

        public static float ConvertToRadians(float angle)
        {
            return (Mathf.PI / 180) * angle;
        }

        /// <summary>
        /// Returns the value from the old range to the new range. The value preserves the proportions(ex: 0.5 in the range between[0, 1] becomes 1.5 in the range [1, 2])
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "originalRange">The original range for the value </param>
        /// <param name = "newRange">The new range for the value </param>
        public static float ChangeRange(float value, Range originalRange, Range newRange)
        {
            if (originalRange != null && newRange != null)
            {
                return ChangeRange(value, originalRange.Min, originalRange.Max, newRange.Min, newRange.Max);
            }
            return 0;
        }

        public static float ChangeRange(float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            float t = Mathf.InverseLerp(fromMin, fromMax, value);
            return Mathf.Lerp(toMin, toMax, t);
        }

        /// <summary>
        /// Returns the value from the old range [0, oldMax] to the new range. The value preserves the proportions(ex: 0.5 in the range between[0, 1] becomes 1.5 in the range [1, 2])
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "oldMax">The uppper limit of original range for the value </param>
        /// <param name = "newRange">The new range for the value </param>
        public static float ChangeRange(float value, float oldMax, Range newRange)
        {
            if (oldMax >= 0 && newRange != null)
            {
                return ChangeRange(value, 0, oldMax, newRange.Min, newRange.Max);
            }
            return 0f;
        }

        /// <summary>
        /// Returns the value from the old range [0, oldMax] to the new range [0, newMax]. The value preserves the proportions(ex: 0.5 in the range between[0, 1] becomes 1.5 in the range [1, 2])
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "oldMax">The uppper limit of original range for the value </param>
        /// <param name = "newMax">The upper limit of new range for the value </param>
        public static float ChangeRange(float value, float oldMax, float newMax)
        {
            if (oldMax >= 0 && newMax >= 0)
            {
                return ChangeRange(value, 0, oldMax, 0, newMax);
            }
            return 0f;
        }

        public static float ChangeRange01(float value, float newMin, float newMax)
        {
            return ChangeRange(value, 0, 1, newMin, newMax);
        }

        /// <summary>
        /// Returns the value from the old range [0, 1] to the new range [0, newMax]. The value preserves the proportions(ex: 0.5 in the range between[0, 1] becomes 1.5 in the range [1, 2])
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        ///  <param name = "newMax">The upper limit of new range for the value </param>
        public static float ChangeRange01(float value, float newMax)
        {
            if (newMax >= 0)
            {
                return ChangeRange(value, 0, 1, 0, newMax);
            }
            return 0f;
        }

        /// <summary>
        /// Returns the value from the old range [0, 1] to the new range [newMax.x, newMax.y]. 
        /// The value preserves the proportions(ex: 0.5 in the range between[0, 1] becomes 1.5 in the range [1, 2]
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "newRange">The new range for the value </param>
        public static float ChangeRange01(float value, Range newRange)
        {
            if (newRange != null)
            {
                return ChangeRange(value, 0, 1, newRange.Min, newRange.Max);
            }
            return 0f;
        }

        /// <summary>
        /// Returns the percent [0, 1] respect a specific range
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "originalRange">The original range for the value </param>
        public static float Percent(float value, Range originalRange)
        {
            if (originalRange != null)
            {
                return ChangeRange(value, originalRange.Min, originalRange.Max, 0, 1);
            }
            return 0;
        }

        public static float Percent(float value, float min, float max)
        {
            if (min <= max)
            {
                return ChangeRange(value, min, max, 0, 1);
            }
            return 0;
        }

        /// <summary>
        /// Returns the percent [0, 1] respect a specific range [0, max]
        /// </summary>
        /// <param name = "value"> The value that will edited</param>
        /// <param name = "originalRange">The original range for the value </param>
        public static float Percent(float value, float max)
        {
            if (max >= 0)
            {
                return ChangeRange(value, 0, max, 0, 1);
            }
            return 0;
        }

        public static bool IsInteger(float x)
        {
            return Math.Abs(x % 1) <= (Double.Epsilon * 100);
        }

        public static float NotPassTheFrontier(float value, float add, float frontier)
        {
            if (value == frontier)
            {
                return frontier;
            }

            bool isPositive = value < frontier;
            float result = value + add;
            if (isPositive)
            {
                return result > frontier ? frontier : result;
            }
            else
            {
                return result < frontier ? frontier : result;
            }
        }

        public static int ChangeInCircle(int value, int maxExclusive)
        {
            return ChangeInCircle(value, 1, 0, maxExclusive);
        }

        public static int ChangeInCircle(int value, int delta, int maxExclusive)
        {
            return ChangeInCircle(value, delta, 0, maxExclusive);
        }

        public static int ChangeInCircle(int value, int delta, int minInclusive, int maxExclusive)
        {
            int range = maxExclusive - minInclusive;

            value = (value - minInclusive) + delta;

            int newValue = value % range;
            if (newValue < 0)
            {
                newValue = range + newValue;
            }
            newValue += minInclusive;

            return newValue;
        }

        public static float ChangeInCircle(float value, float delta, float maxExclusive)
        {
            return ChangeInCircle(value, delta, maxExclusive);
        }

        public static bool ContainsInCircle(float value, float min, float max)
        {
            return (min <= max && (value >= min && value <= max)) ||
                (min > max && (value >= min || value <= max));
        }

        public static float ChangeInCircle(float value, float delta, float min, float max)
        {
            float range = max - min;

            value = (value - min) + delta;

            float newValue = value % range;
            if (newValue < 0)
            {
                newValue = range + newValue;
            }
            newValue += min;

            return newValue;
        }

        public static float ChangeValue(float originalValue, float newValue, ChangeTypeEnum changeTypeTransform, float minValue = float.MinValue, float maxValue = float.MaxValue)
        {
            //Mathf.DeltaAngle

            float value = changeTypeTransform == ChangeTypeEnum.Setting ? newValue : originalValue + newValue;
            return Mathf.Clamp(value, minValue, maxValue);
        }

        public static float Clamp(float value, Range range)
        {
            return Mathf.Clamp(value, range.Min, range.Max);
        }

        public static float Clamp(float value, Vector2 range)
        {
            return Mathf.Clamp(value, range.x, range.y);
        }

        public static float Lerp(Range range, float t, FunctionEnum function = FunctionEnum.Linear)
        {
            if (function == FunctionEnum.Linear)
            {
                return Mathf.Lerp(range.Min, range.Max, t);
            }
            else
            {
                return Lerp(range.Min, range.Max, t, function);
            }
        }

        public static bool IsContains(this float value, float min, float max, bool includeMin = true, bool includeMax = true)
        {
            if (includeMax && includeMin)
            {
                return value >= min && value <= max;
            }
            else if (!includeMax && !includeMin)
            {
                return value > min && value < max;
            }
            else if (includeMin)
            {
                return value >= min && value < max;
            }
            else
            {
                return value > min && value <= max;
            }
        }

        public static bool IsContains(this int value, int min, int max, bool includeMin = true, bool includeMax = true)
        {
            if (includeMax && includeMin)
            {
                return value >= min && value <= max;
            }
            else if (!includeMax && !includeMin)
            {
                return value > min && value < max;
            }
            else if (includeMin)
            {
                return value >= min && value < max;
            }
            else
            {
                return value > min && value <= max;
            }
        }

        public static bool IsContains(this float value, float max, bool includeMax = true)
        {
            if (includeMax)
            {
                return value >= 0 && value <= max;
            }
            else
            {
                return value >= 0 && value < max;
            }
        }

        public static bool IsContains(this int value, int max, bool includeMax = true)
        {
            if (includeMax)
            {
                return value >= 0 && value <= max;
            }
            else
            {
                return value >= 0 && value < max;
            }
        }

        public static float ParseFloat(in string value, CultureInfo cultureInfo = null)
        {
            float f = -1;

            try
            {
                f = float.Parse(value, cultureInfo);
            }
            catch (Exception e)
            {
                LogManager.LogWarning(e);
            }

            return f;
        }
        #endregion
    }
}
