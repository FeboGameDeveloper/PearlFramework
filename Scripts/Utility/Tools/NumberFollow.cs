using UnityEngine;

namespace Pearl
{
    // La classe permette ad un numero di arrivare ad un'altro numero tramite un progressione polimoniale
    public class NumberFollow
    {
        private float currentNumber;
        private readonly Polynomial polynomial;

        public NumberFollow(PolynomialElement[] elements)
        {
            currentNumber = 0;
            polynomial = new Polynomial(elements);
        }


        public float Follow(float newNumber)
        {
            float x = newNumber - currentNumber;

            if (x == 0)
            {
                return currentNumber;
            }

            int sign = MathfExtend.Sign(x);
            x = Mathf.Abs(x);

            float y = Mathf.Abs(polynomial.Evalutate(x));

            currentNumber = MathfExtend.NotPassTheFrontier(currentNumber, y * sign, newNumber);
            return currentNumber;
        }

    }

}