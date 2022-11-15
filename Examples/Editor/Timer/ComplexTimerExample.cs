using Pearl.ClockManager;
using UnityEngine;

namespace Pearl.Examples.ClockExamples
{
    public class ComplexTimerExample : MonoBehaviour
    {
        [SerializeField]
        private float timerValue = 2;

        [InspectorButton("Pause")]
        [SerializeField]
        private bool pauseButton;

        [TextArea]
        public string text;

        private TimerContainer timer;


        // Start is called before the first frame update
        protected void Start()
        {
            TimerContainer.CreateTimer(out timer, timerValue, true, false, OnFinish, TimeType.Scaled);
        }

        // Update is called once per frame
        protected void Update()
        {
            if (timer != null && timer.On)
            {
                text = "TIMER: " + timer.Time + "\n" +
                       "PERCENT: " + timer.TimeInPercent + "\n" +
                       "TIMER REVERSED: " + timer.TimeReversed + "\n" +
                       "TIMER REVERSED: " + timer.TimeInPercentReversed;
            }
        }

        private void OnFinish(TimerContainer container, float left, float right)
        {
            text = "FINISH TIMER WITH " + right + " DELAY";
        }

        protected void Pause()
        {
            timer.Pause(!timer.IsPause);
        }
    }
}
