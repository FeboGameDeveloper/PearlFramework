using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    public class AutoDestruction : MonoBehaviour
    {
        [SerializeField]
        private float time;
        [SerializeField]
        private TimeType timeType;
        [SerializeField]
        private bool atStart;

        private bool _init;
        private TimerContainer _timer;

        // Start is called before the first frame update
        protected void Start()
        {
            TimerContainer.CreateTimer(out _timer, true, OnFinishTime, timeType);

            if (atStart)
            {
                InitDestroy();
            }
        }

        protected void OnDestroy()
        {
            _timer?.Kill();
        }

        public void InitDestroy()
        {
            if (!_init)
            {
                _init = true;
                _timer.ResetOn(time);
            }
        }

        private void OnFinishTime(TimerContainer container, float left, float right)
        {
            GameObjectExtend.DestroyExtend(gameObject);
        }
    }
}
