#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class PulseTask : ActionTask<Transform>
    {
        public BBParameter<Vector2> rangeX = default;
        public BBParameter<Vector2> rangeY = default;
        public BBParameter<Vector2> rangeZ = default;

        public float deltaPulse;

        private Vector3 _originalScale;
        private Vector3 _destinatioScale;
        private Vector3 _initScale;
        private Timer _timer;
        private bool _bigPulse = false;

        protected override void OnExecute()
        {
            if (agent)
            {
                _originalScale = agent.localScale;
            }

            _destinatioScale = _originalScale;
            SetDestination();
            _timer = new Timer(deltaPulse, true);
        }

        protected override void OnStop()
        {
            if (agent)
            {
                agent.localScale = _originalScale;
            }
        }

        protected override void OnUpdate()
        {
            if (agent)
            {
                agent.localScale = Vector3.Lerp(_initScale, _destinatioScale, _timer.TimeInPercent);
            }


            if (_timer.IsFinish())
            {
                _bigPulse = !_bigPulse;
                SetDestination();
                _timer.ResetOn();
            }
        }

        private void SetDestination()
        {
            if (rangeX != null && rangeY != null && rangeZ != null)
            {
                _initScale = _destinatioScale;

                _destinatioScale = _originalScale;
                if (_bigPulse)
                {
                    _destinatioScale += new Vector3(rangeX.value.y, rangeY.value.y, rangeZ.value.y);
                }
                else
                {
                    _destinatioScale -= new Vector3(rangeX.value.x, rangeY.value.x, rangeZ.value.x);
                }
            }
        }
    }
}

#endif