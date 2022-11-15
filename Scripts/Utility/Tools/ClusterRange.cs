using UnityEngine;

namespace Pearl
{
    //Dividere un intervallo di numeri in tot intervalli
    public class ClusterRange
    {
        private float _delta;
        private float _maxValue;

        public ClusterRange()
        {
        }

        public ClusterRange(float maxValue, int intervalsNumber)
        {
            Reset(maxValue, intervalsNumber);
        }

        public void Reset(float maxValue, int intervalsNumber)
        {
            _maxValue = maxValue;
            _delta = maxValue / (intervalsNumber - 1);
        }

        //Stabilire l'intevllo in cui èposizionato il numero
        public int GetNumberInterval(float currentValue, bool inverse = false)
        {
            if (_delta == 0)
            {
                return 0;
            }

            currentValue = Mathf.Clamp(currentValue, 0, _maxValue);

            if (inverse)
            {
                currentValue = _maxValue - currentValue;
            }

            return (int)(currentValue / _delta);
        }

    }
}
