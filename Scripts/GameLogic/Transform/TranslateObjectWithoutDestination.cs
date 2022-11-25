using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    public class TranslateObjectWithoutDestination : MonoBehaviour
    {
        [SerializeField]
        private bool useThisAgent = true;
        [SerializeField]
        private bool useTime = true;

        [SerializeField]
        private bool destoryAtFinish = true;

        [ConditionalField("!@useThisAgent")]
        [SerializeField]
        private Transform agent;

        [ConditionalField("@useTime")]
        [SerializeField]
        private float time = 2f;

        [SerializeField]
        private bool initAtStart = false;
        [SerializeField]
        private Vector3 translateVector = default;
        [SerializeField]
        private float speed = 1f;

        private bool _play = false;
        private Timer _timer;

        private void Awake()
        {
            _timer = new Timer(time, false);

            if (useThisAgent)
            {
                agent = transform;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (initAtStart)
            {
                Play();
            }
        }

        private void Update()
        {
            if (_play)
            {
                if (agent)
                {
                    agent.Translate(translateVector * speed * Time.deltaTime);
                }

                if (useTime && _timer != null && _timer.IsFinish())
                {
                    Stop();
                }
            }
        }

        public void Play()
        {
            _play = true;

            if (useTime)
            {
                _timer.ResetOn();
            }
        }

        public void Play(Vector3 translateVector)
        {
            this.translateVector = translateVector;
            Play();
        }

        public void Stop()
        {
            _play = false;
            _timer.ResetOff();
            if (destoryAtFinish)
            {
                GameObjectExtend.DestroyExtend(gameObject);
            }
        }
    }

}