using Pearl.ClockManager;
using UnityEngine;

namespace Pearl.Examples.ClockExamples
{
    public class ClockExample : MonoBehaviour
    {
        [InspectorButton("Pause")]
        [SerializeField]
        private bool pauseButton;

        [TextArea]
        public string text;

        private Clock clock;


        // Start is called before the first frame update
        protected void Start()
        {
            clock = new Clock(true);
        }

        // Update is called once per frame
        protected void Update()
        {
            text = "CLOCK: " + clock.Time;
        }

        protected void Pause()
        {
            clock.Pause(!clock.IsPause);
        }
    }
}
