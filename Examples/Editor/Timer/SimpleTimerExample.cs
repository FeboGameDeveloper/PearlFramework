using Pearl.ClockManager;
using UnityEngine;

namespace Pearl.Examples.ClockExamples
{
    public class SimpleTimerExample : MonoBehaviour
    {
        [SerializeField]
        private float timerValue = 2;

        [InspectorButton("Pause")]
        [SerializeField]
        private bool pauseButton;

        [TextArea]
        public string text;

        private SimpleTimer timer;


        // Start is called before the first frame update
        protected void Start()
        {
            timer = new SimpleTimer(timerValue, TimeType.Scaled);
        }

        // Update is called once per frame
        protected void Update()
        {
            if (timer.IsFinish())
            {
                text = "FINISH TIMER";
            }
        }

        protected void Pause()
        {
            timer.Pause(!timer.IsPause);
        }
    }

}