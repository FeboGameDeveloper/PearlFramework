using Pearl.ClockManager;
using UnityEngine;

namespace Pearl.Examples.ClockExamples
{
    public class TimerExample : MonoBehaviour
    {
        [SerializeField]
        private float timerValue = 2;

        [InspectorButton("Pause")]
        [SerializeField]
        private bool pauseButton;

        [TextArea]
        public string text;

        private Timer timer;


        // Start is called before the first frame update
        protected void Start()
        {
            timer = new Timer(timerValue, true);
        }

        // Update is called once per frame
        protected void Update()
        {
            if (timer.IsFinish())
            {
                text = "FINISH TIMER";
            }
            else
            {
                text = "TIMER: " + timer.Time + "\n" +
                       "PERCENT: " + timer.TimeInPercent + "\n" +
                       "TIMER REVERSED: " + timer.TimeReversed + "\n" +
                       "TIMER REVERSED PERCENT: " + timer.TimeInPercentReversed;
            }
        }

        protected void Pause()
        {
            timer.Pause(!timer.IsPause);
        }
    }
}