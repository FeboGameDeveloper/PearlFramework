using Pearl.Events;
using UnityEngine;

namespace Pearl.UI
{
    public class StampNumberUI : MonoBehaviour
    {
        [SerializeField]
        private TextManager textManager = null;
        [SerializeField]
        private TypeNumber typeNumber = TypeNumber.Float;
        [SerializeField]
        private bool useRange = false;
        [ConditionalField("@useRange")]
        [SerializeField]
        private Range rangeInput = null;
        [ConditionalField("@useRange")]
        [SerializeField]
        private Range rangeOutput = null;

        [SerializeField]
        private string ev = null;

        private void OnEnable()
        {
            if (typeNumber == TypeNumber.Float)
            {
                PearlEventsManager.AddAction<float>(ev, WriteNumber);
            }
            else
            {
                PearlEventsManager.AddAction<int>(ev, WriteNumber);
            }
        }

        private void OnDisable()
        {
            if (typeNumber == TypeNumber.Float)
            {
                PearlEventsManager.RemoveAction<float>(ev, WriteNumber);
            }
            else
            {
                PearlEventsManager.RemoveAction<int>(ev, WriteNumber);
            }
        }

        public void WriteNumber(float newValue)
        {
            string valueString = ConvertInString(newValue);

            if (textManager)
            {
                textManager.SetText(valueString);
            }
        }

        public void WriteNumber(int newValue)
        {
            string valueString = ConvertInString(newValue);

            if (textManager)
            {
                textManager.SetText(valueString);
            }
        }


        private string ConvertInString(float newValue)
        {
            if (useRange)
            {
                newValue = MathfExtend.ChangeRange(newValue, rangeInput, rangeOutput);
                newValue = Mathf.FloorToInt(newValue);
            }

            return newValue.ToString();
        }
    }
}
