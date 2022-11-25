using Pearl.ClockManager;
using Pearl.Events;
using UnityEngine;

namespace Pearl
{
    public class TranslateObject : MonoBehaviour
    {
        #region Field Inspector
        [SerializeField]
        private bool initAtStart = false;

        [SerializeField]
        private bool destroyAtFinish = false;

        [SerializeField]
        private bool isTarget = false;

        [SerializeField]
        private bool originUpdate = false;

        [SerializeField]
        private Transform agentTransform = default;

        [SerializeField]
        private Transform originTransform = null;

        [SerializeField]
        [ConditionalField("@isTarget")]
        private Transform targetTransform;

        [SerializeField]
        [ConditionalField("!@isTarget")]
        private Vector3 translateVector = default;

        [SerializeField]
        private TimeType timeType = TimeType.Scaled;

        [SerializeField]
        private float timeLength = 1f;

        [SerializeField]
        private bool useCurve = false;

        [SerializeField]
        [ConditionalField("@useCurve")]
        private AnimationCurve curve = null;

        [SerializeField]
        private GameObjectEvent onFinish = null;

        #endregion

        #region Private Fields

        private bool init = false;
        private Vector3 _origin = default;
        private Vector3 _destnation = default;
        private Timer _timer;

        #endregion

        #region Proprieties

        public Transform ObjTransfom
        {
            set
            {
                originTransform = value;
                if (originTransform)
                {
                    _origin = originTransform.position;
                }
            }
        }

        public float TimeLength
        {
            set
            {
                timeLength = value;
            }
        }
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            _timer = new Timer(timeType);
        }

        private void Start()
        {
            if (originTransform && (!isTarget || targetTransform))
            {
                _origin = originTransform.position;
                _destnation = isTarget ? targetTransform.position : _origin + translateVector;
            }

            if (initAtStart)
            {
                Play();
            }
        }

        private void Update()
        {
            if (init && _timer != null && originTransform && (!useCurve || curve != null) && (!originUpdate || originTransform != null) && (!isTarget || targetTransform != null))
            {
                float percent = useCurve && curve != null ? curve.Evaluate(_timer.TimeInPercent) : _timer.TimeInPercent;

                Vector3 pointA = originUpdate ? originTransform.position : _origin;
                Vector3 pointb = isTarget ? targetTransform.position : _destnation;

                Vector3 currentPosition = Vector3.Lerp(pointA, pointb, percent);

                if (agentTransform)
                {
                    agentTransform.position = currentPosition;
                }

                if (_timer.IsFinish())
                {
                    onFinish.Invoke(gameObject);
                    ResetTranslation();
                    if (destroyAtFinish)
                    {
                        GameObjectExtend.DestroyExtend(gameObject);
                    }
                }

            }
        }

        private void Reset()
        {
            agentTransform = transform;
        }

        #endregion

        #region Public Methods
        public void Play(Transform target, Transform origin = null)
        {
            if (target != null)
            {
                targetTransform = target;
            }

            if (origin != null)
            {
                originTransform = origin;
            }

            init = true;
            _timer?.ResetOn(timeLength);
        }

        public void Play(float timeLength, Transform target, Transform origin = null)
        {
            this.timeLength = timeLength;
            Play(target, origin);
        }

        public void Play(float timeLength)
        {
            this.timeLength = timeLength;
            Play();
        }

        public void Play()
        {
            Play(null);
        }
        #endregion

        #region Private Methods
        private void ResetTranslation()
        {
            _timer.ResetOff();
            init = false;
        }

        #endregion
    }

}