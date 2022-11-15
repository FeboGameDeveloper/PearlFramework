#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.ClockManager;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class TranslateTask : ActionTask<Transform>
    {
        public BBParameter<float> timeForTranslate;
        public BBParameter<Vector3> vectorForTranslate;
        public BBParameter<Transform> transform;


        private Timer timer;

        protected override void OnExecute()
        {
            if (timeForTranslate != null)
            {
                timer = new Timer(timeForTranslate.value, true);
            }
        }

        protected override void OnUpdate()
        {
            if (transform != null && transform.value != null && vectorForTranslate != null)
            {
                transform.value.Translate(vectorForTranslate.value * Time.deltaTime);
            }


            if (timer != null && timer.IsFinish())
            {
                EndAction();
            }
        }
    }
}

#endif
