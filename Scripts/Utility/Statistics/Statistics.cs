using Pearl.Debug;
using System;
using UnityEngine;

namespace Pearl
{
    //funzioni statistiche
    public class Statistics
    {
        public static float Average(params float[] numbers)
        {
            if (numbers.IsAlmostSpecificCount())
            {
                float total = 0;
                foreach (float number in numbers)
                {
                    total += number;
                }

                return total / numbers.Length;
            }

            LogManager.LogWarning("The average is wrong");
            return 0f;
        }

        public static float Median(params float[] numbers)
        {
            if (numbers.IsAlmostSpecificCount())
            {
                Array.Sort(numbers);
                float Length = numbers.Length;

                if (MathfExtend.IsOdd(numbers.Length))
                {
                    return numbers[(int)((Length + 1) / 2)];
                }
                else
                {
                    float aux1 = numbers[(int)(Length / 2)];
                    float aux2 = numbers[(int)((Length / 2) + 1)];

                    return Average(aux1, aux2);
                }
            }

            LogManager.LogWarning("The median is wrong");
            return 0f;
        }

        public static float Variance(params float[] numbers)
        {
            if (numbers.IsAlmostSpecificCount())
            {
                float average = Average(numbers);
                float sum = 0;
                for (int i = 0; i < numbers.Length; i++)
                {
                    sum += Mathf.Pow(numbers[i] - average, 2);
                }

                return sum / numbers.Length;
            }

            LogManager.LogWarning("The variance is wrong");
            return 0f;
        }
    }
}
