#if SPLINE

using Pearl.ClockManager;
using Pearl.Events;
using Unity.Mathematics;
using UnityEngine;

namespace Pearl.Paths
{
    public class PathManager : MonoBehaviour, IPause
    {
        public enum PathState { Active, Inactive, Pause, Wait }

#region Public fields

        [Header("General setting")]

        [SerializeField]
        [Tooltip("the path reference")]
        private PearlSpline path = null;
        [SerializeField]
        [Tooltip("the type translation")]
        private TypeTranslation typeTranslation = TypeTranslation.Transform;
        [SerializeField]
        [Tooltip("the transform that uses the path")]
        private ComponentReference<Transform> referenceTranform = null;

        [Header("Duration")]

        [SerializeField]
        [Tooltip("Use the time or velocity for move.")]
        private ChangeModes changeMode = ChangeModes.Velocity;
        [SerializeField]
        [ConditionalField("@changeMode == Time")]
        [Tooltip("The time it takes to finish the route.")]
        private float time = 3f;
        [SerializeField]
        [ConditionalField("@changeMode == Time")]
        private TimeType timeType = TimeType.Scaled;
        [ConditionalField("@changeMode == Velocity")]
        [SerializeField]
        [Tooltip("The speed of the object to travel the curve.")]
        private float velocity = 7;

        [Header("Curve")]

        [SerializeField]
        [Tooltip("Use a curve to file the path.")]
        private bool useCurve = false;
        [SerializeField]
        [ConditionalField("@useCurve")]
        [Tooltip("Various types of curves.")]
        private AnimationCurveInfo curve = default;

        [Header("Attributes")]

        [SerializeField]
        [Tooltip("The type of path: repeat th path after finish")]
        private bool isLoop = false;
        [SerializeField]
        [ConditionalField("@isLoop")]
        [Tooltip("At the end of each path before it starts, it resumes after x time.")]
        private float waitAtEndPath = 1f;
        [SerializeField]
        [Tooltip("The path starts from the beginning or from the end.")]
        private bool isInverse = false;
        [Tooltip("Is there the returned path?")]
        [SerializeField]
        private bool pingPong = false;
        [SerializeField]
        [Tooltip("The start of the path is absolute, that is, it is equal to the path. Or it is relative with respect to the position of the object.")]
        private bool isRelative = false;

        [Header("Start")]

        [SerializeField]
        [Range(0f, 1f)]
        [Tooltip("At what percentage of the path did I start the object?")]
        private float startT = 0f;
        [SerializeField]
        [Tooltip("The object automatically starts at the beginning.")]
        private bool initAtStart = false;

        [Header("Orientation")]

        [SerializeField]
        [Tooltip("Does the rotation of the object follow the trajectory of the path?")]
        private bool usePathOrientation = true;
        [SerializeField]
        [ConditionalField("@usePathOrientation")]
        [Tooltip("The starting angle of rotation")]
        private float initAngle = 0;

        [Header("Debug")]

        [SerializeField]
        [Tooltip("Debug mode: thanks to it you can see the trajectory of the object in the editor")]
        private bool debug = false;
        [InspectorButton("SetInitDebug")]
        [ConditionalField("@debug")]
        public bool setInitDebug;

        private Rigidbody _body;
        private Rigidbody2D _body2D;

        private void SetInitDebug()
        {
            _obj = referenceTranform.Component;
            _initPositionDebug = _obj.position;
        }

        [Header("Event")]

        [SerializeField]
        [Tooltip("When the path ends, do you have to activate some methods?")]
        private FloatEvent onFinish = null;

#endregion

#region Private fields

        private PathState _pathState = PathState.Inactive;
        private PathState _oldState = PathState.Inactive;
        private Timer _timer;
        private Transform _obj;
        private float _currentLength;
        private float _maxLength;
        private float _error;
        private float3 _initPositionDebug;
        private Vector3 _initPosition;
        private bool _currentInverse;

        private float3 _deltaPath;
        private float3 _initPath;
        private float _currentStartT;

#endregion

#region Properties
        public float CurrentPercent { get; private set; }

        public PearlSpline Path
        {
            get => path;
            set
            {
                if (path != null)
                {
                    path.OnChangePath -= OnChangePath;
                }

                path = value;

                if (path != null)
                {
                    path.OnChangePath += OnChangePath;
                    OnChangePath();
                }
            }
        }
        public float Velocity { get => velocity; set => velocity = value; }

        public float Time { get => time; set => time = value; }
#endregion

#region UnityCallbacks
        protected void Awake()
        {
            if (referenceTranform != null)
            {
                _obj = referenceTranform.Component;
            }

            _body = GetComponent<Rigidbody>();
            _body2D = GetComponent<Rigidbody2D>();

            if (path != null)
            {
                OnChangePath();
                path.OnChangePath += OnChangePath;
            }
            _timer = new Timer();
        }

        protected void OnValidate()
        {
            if (debug && !Application.isPlaying && path != null && referenceTranform != null)
            {
                _obj = referenceTranform.Component;
                Vector3 result;
                if (isRelative)
                {
                    path.GetPointReltive(out result, startT, _initPositionDebug);
                }
                else
                {
                    result = path.EvaluatePosition(startT);
                }

                _obj.position = result;
                SetDirection(startT);
            }
        }

        protected void Start()
        {
            if (initAtStart)
            {
                Play();
            }
        }

        protected void Reset()
        {
            path = GetComponent<PearlSpline>();
        }

        protected void Update()
        {
            if (_pathState == PathState.Active)
            {
                CurrentPercent = GetCurrenPercent();
                Navigate();
            }
        }
#endregion

#region Public Methods
        public void Play()
        {
            _currentStartT = startT;
            _pathState = PathState.Active;
            _currentInverse = isInverse;
            Execute();
        }

        public void Stop()
        {
            if (_pathState == PathState.Inactive || _pathState == PathState.Pause)
            {
                return;
            }

            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            _pathState = PathState.Inactive;
        }

        public void ResetPath()
        {
            if (_pathState == PathState.Inactive || _pathState == PathState.Pause)
            {
                return;
            }

            CurrentPercent = pingPong ? (_currentInverse == isInverse ? 0 : 1) : 0;
            Navigate();
            Stop();
        }

        public void Force(float t)
        {
            if (_pathState == PathState.Inactive || _pathState == PathState.Pause || _timer == null)
            {
                return;
            }

            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            if (pingPong)
            {
                if (t < 0.5f)
                {
                    _currentInverse = isInverse;
                    t *= 2;
                }
                else
                {
                    _currentInverse = !isInverse;
                    t = (t - 0.5f) * 2;
                }
            }

            _currentStartT = t;

            if (changeMode == ChangeModes.Time)
            {
                float auxTime = pingPong ? time * 0.5f : time;
                _timer.ResetOn(auxTime, timeType, (_currentStartT * auxTime));
            }
            else
            {
                _currentLength = _currentStartT * _maxLength;
            }

            _currentStartT = 0;
            _pathState = PathState.Active;

            CurrentPercent = GetCurrenPercent();
            Navigate();
        }

        public void ForceFinish()
        {
            if (_pathState == PathState.Inactive || _pathState == PathState.Pause)
            {
                return;
            }

            CurrentPercent = pingPong ? (_currentInverse == isInverse ? 0 : 1) : 1;
            Navigate();
            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            Finish();
        }

#endregion

#region Private Method
        private void OnChangePath()
        {
            if (path != null)
            {
                _maxLength = path.CalculateLength();
                _initPath = path.EvaluatePosition(0);
                _deltaPath = path.EvaluatePosition(1) - _initPath;
            }
        }

        private void Execute(float error = 0)
        {
            if (_obj == null || _timer == null)
            {
                return;
            }

            if (isRelative)
            {
                _initPosition = _currentInverse ? (float3)_obj.position - _deltaPath : _obj.position;
            }

            if (changeMode == ChangeModes.Time)
            {
                float auxTime = pingPong ? time * 0.5f : time;
                _timer.ResetOn(auxTime, timeType, error + (_currentStartT * auxTime));
            }
            else
            {
                _currentLength = velocity * error * UnityEngine.Time.deltaTime + _currentStartT * _maxLength;
            }

            _currentStartT = 0;
        }

        private void Navigate()
        {
            if (_obj == null || path == null)
            {
                return;
            }

            float f = _currentInverse ? 1 - CurrentPercent : CurrentPercent;

            Vector3 newPosition;
            if (isRelative)
            {
                path.GetPointReltive(out newPosition, f, _initPosition);
            }
            else
            {
                newPosition = path.EvaluatePosition(f);
            }

            if (typeTranslation == TypeTranslation.Transform)
            {
                _obj.position = newPosition;
            }
            else if (typeTranslation == TypeTranslation.Rigidbody2D)
            {
                _body2D.MovePosition(newPosition);
            }
            else
            {
                _body.MovePosition(newPosition);
            }


            SetDirection(f);

            if (CurrentPercent >= 1)
            {
                IsFinish();
            }
        }


        private void IsFinish()
        {
            if (pingPong && _currentInverse == isInverse)
            {
                _currentInverse = !isInverse;
                Execute();
            }
            else if (isLoop)
            {
                _pathState = PathState.Wait;

                if (waitAtEndPath > 0)
                {
                    PearlInvoke.WaitAccurateForMethod(waitAtEndPath - _error, OnFinishWait);
                }
                else
                {
                    OnFinishWait(0, Mathf.Abs(_error));
                }
            }
            else
            {
                Finish();
            }
        }

        private void Finish()
        {
            if (_pathState != PathState.Inactive)
            {
                onFinish?.Invoke(_error);
                _pathState = PathState.Inactive;
            }
        }

        private void OnFinishWait(float errorLeft, float errorRight)
        {
            _currentInverse = isInverse;
            _pathState = PathState.Active;
            Execute(errorRight);
        }


        private float GetCurrenPercent()
        {
            if (_timer == null)
            {
                return default;
            }

            float percent;
            if (changeMode == ChangeModes.Time)
            {
                _timer.IsFinish(out _, out _error);
                percent = _timer.TimeInPercent;
            }
            else
            {
                _currentLength += velocity * UnityEngine.Time.deltaTime;
                percent = Mathf.Clamp01(_currentLength / _maxLength);

                _error = _currentLength - _maxLength;
                _error /= velocity;
            }

            if (useCurve && curve != null)
            {
                percent = curve.Evaluate(percent);
            }

            return percent;
        }

        private void SetDirection(float percent)
        {
            if (usePathOrientation)
            {
                _obj.rotation = QuaternionExtend.CalculateRotation2D((Vector3)path.EvaluateTangent(percent), initAngle);
            }
        }
#endregion

#region Interface Methods
        public void Pause(bool onPause)
        {
            if ((_pathState == PathState.Pause && onPause) || (_pathState != PathState.Pause && !onPause) || _pathState == PathState.Inactive)
            {
                return;
            }

            if (onPause)
            {
                _oldState = _pathState;
            }

            _pathState = onPause ? PathState.Pause : _oldState;
            _timer.Pause(onPause);
            PearlInvoke.PauseTimer<float, float>(OnFinishWait, onPause);
        }
#endregion
    }
}

#endif
