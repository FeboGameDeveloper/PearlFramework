using System;

namespace Pearl
{

    //calcola dinamicamente la media.
    [Serializable]
    public class DynamicAverage
    {
        #region Private Methods
        private int index;
        private float average;
        #endregion

        #region Properties
        public float Average { get { return average; } }
        #endregion

        #region Constructors
        public DynamicAverage() : this(0, 0f)
        {
        }

        public DynamicAverage(int indexParam, float averageParam)
        {
            this.index = indexParam;
            this.average = averageParam;
        }
        #endregion

        #region Public Methods
        public void AddElementInPopolation(float newElement)
        {
            average += ((newElement - average) / ++index);
        }

        public void Reset()
        {
            index = 0;
            average = 0;
        }
        #endregion
    }
}
