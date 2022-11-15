using System;

namespace Pearl
{
    //Calcola dinamicamente la varianza
    public class DynamicVariance
    {
        #region Private Methods
        private double average;
        private int index;
        private double variance;
        private double M2;
        #endregion

        #region Aux Fields
        private double auxFloat;
        #endregion

        #region Properties
        public double Average { get { return average; } }
        public double Variance { get { return variance; } }
        public double StandardDeviation { get { return Math.Sqrt(variance); } }
        #endregion

        #region Constructors
        public DynamicVariance()
        {
            Reset();
        }
        #endregion

        #region Public Methods
        //I use Welford's Online algorithm
        public void AddElementInPopolation(double newElement)
        {
            auxFloat = newElement - average;
            average += (auxFloat / ++index);
            M2 += auxFloat * (newElement - average);
            variance = index != 1 ? M2 / (index - 1) : 0;
        }

        public void Reset()
        {
            variance = 0;
            M2 = 0;
            index = 0;
            average = 0;
        }
        #endregion
    }
}