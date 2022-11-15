using Pearl.Tweens;
using UnityEngine;

namespace Pearl.Examples.TweenExample
{
    public class ExampleTween : MonoBehaviour
    {
        [InterfaceType(typeof(ITween))]
        public Object tweenObj;

        private ITween tween;

        [Range(0, 1)]
        public float puntator;

        [InspectorButton("Stop")]
        public bool stop;

        [InspectorButton("Pause")]
        public bool pause;

        [InspectorButton("Unpause")]
        public bool unpause;

        [InspectorButton("ResetTween")]
        public bool resetTween;

        [InspectorButton("ForceFinish")]
        public bool forceFinish;

        [InspectorButton("Play")]
        public bool play;

        [InspectorButton("Force")]
        public bool force;



        private void Start()
        {
            tween = (ITween)tweenObj;
        }

        public void Stop()
        {
            tween.Stop();
        }

        public void Force()
        {
            tween.Force(puntator);
        }

        public void Pause()
        {
            tween.Pause(true);
        }

        public void Unpause()
        {
            tween.Pause(false);
        }

        public void ResetTween()
        {
            tween.ResetTween();
        }

        public void ForceFinish()
        {
            tween.ForceFinish();
        }

        public void Play()
        {
            tween.Play();
        }
    }
}