using Pearl.Debug;
using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class PolynomialElement
    {
        public float coef;
        public float exponent;
    }


    //classe che rappresnta un polinomio
    public class Polynomial
    {
        private PolynomialElement[] elements;

        public Polynomial(params PolynomialElement[] elements)
        {
            this.elements = elements;
        }

        public float Evalutate(float x)
        {
            float result = 0;

            if (elements == null)
            {
                return result;
            }

            for (int i = 0; i < elements.Length; i++)
            {
                PolynomialElement element = elements[i];
                if (element != null)
                {
                    float coef = element.coef;
                    float exp = element.exponent;
                    try
                    {
                        result += coef * Mathf.Pow(x, exp);
                    }
                    catch (Exception e)
                    {
                        LogManager.LogWarning(e);
                    }
                }
            }

            return result;
        }
    }
}
