using Pearl.Events;
using UnityEngine;

namespace Pearl.Tweens
{
    public abstract class TweenComponent<Type, Container> : MonoBehaviour, ITween where Container : Component
    {
        #region Inspector fields
        [SerializeField]
        protected ComponentReference<Container> container;
        [SerializeField]
        protected Type[] values;
        [SerializeField]
        protected FunctionEnum functionEnum;
        [SerializeField]
        protected ChangeModes mode;
        [ConditionalField("@mode == Time")]
        [SerializeField]
        protected float time;
        [ConditionalField("@mode == Time")]
        [SerializeField]
        protected TimeType timeType = TimeType.Scaled;
        [ConditionalField("@mode == Velocity")]
        [SerializeField]
        protected float velocity;
        [SerializeField]
        protected bool isRelative;
        [SerializeField]
        protected bool isLoop;
        [ConditionalField("@isLoop")]
        [SerializeField]
        protected float waitAtEndPath = 0f;
        [SerializeField]
        protected bool pingPong;
        [SerializeField]
        protected bool isInverse;
        [SerializeField]
        protected bool isAutoKill;
        [SerializeField]
        protected float curveFactor = 0;
        [SerializeField]
        protected bool init;
        [SerializeField]
        protected bool useInitValue = false;
        [SerializeField]
        [ConditionalField("@useInitValue")]
        protected Type initValue = default;

        [SerializeField]
        private FloatEvent OnCompleteTween;
        #endregion

        #region Protected fields
        protected TweenContainer _tween;
        protected float _valueForTween;
        #endregion

        #region UnityCallbacks
        protected void Awake()
        {
            _valueForTween = mode == ChangeModes.Time ? time : velocity;
            if (useInitValue)
            {
                SetValue(initValue);
            }
            CreateTween();

            if (_tween != null)
            {
                _tween.OnComplete += OnComplete;
            }
        }

        protected void Start()
        {
            if (init)
            {
                Play();
            }
        }

        private void OnValidate()
        {
            container ??= new();
            container.Component = GetComponent<Container>();
        }

        protected void OnDisable()
        {
            _tween?.Kill();
        }
        #endregion

        #region Public Methods
        public void Pause(bool onPause)
        {
            _tween?.Pause(onPause);
        }

        public void Force(float percent)
        {
            _tween?.ForceTween(percent);
        }

        public void Stop()
        {
            _tween?.Stop();
        }

        public void ResetTween()
        {
            _tween?.Reset();
        }

        public void ForceFinish()
        {
            _tween?.ForceFinish();
        }

        public void Kill()
        {
            _tween?.Kill();
        }

        // Update is called once per frame
        public virtual void Play(bool setInit = false)
        {
            if (_tween != null)
            {
                _tween.PathReference = isRelative ? TypeReferenceEnum.Relative : TypeReferenceEnum.Absolute;
                _tween.IsLoop = isLoop;
                _tween.Function = functionEnum;
                _tween.PingPong = pingPong;
                _tween.IsInverse = isInverse;
                _tween.WaitAtEndPath = waitAtEndPath;
                _tween.Duration = time;
                _tween.Velocity = velocity;
                _tween.CurveFactor = curveFactor;
                _tween.Play(setInit, timeType);
            }
        }
        #endregion

        #region private methods
        private void OnComplete(TweenContainer tween, float error)
        {
            OnCompleteTween?.Invoke(error);
        }
        #endregion

        #region abstract methods
        protected abstract void CreateTween();

        public abstract void SetValue(Type type);
        #endregion
    }

}
