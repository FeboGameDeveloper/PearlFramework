using Pearl.ClockManager;
using Pearl.Debug;
using System;
using System.Linq;
using UnityEngine;

namespace Pearl.Tweens
{
    public enum TweenState { Active, Inactive, Pause, Complete, Wait }

    //Th class that manges the Tween
    public class Tween : IReset, IPause
    {
        #region Readonly Fields
        private readonly Timer _timer;
        #endregion

        #region Private Fields
        private float duration;
        private AnimationCurveInfo _curve;
        private ChangeModes _changeMode = ChangeModes.Time;

        private TypeReferenceEnum _pathReference = TypeReferenceEnum.Absolute;
        private bool _isInverse = false;
        private bool _isLoop = false;
        private bool _pingPong = false;
        private float _curveFactor = 0;

        private Func<Vector4> _getCurrentValue;
        private Vector4 _initValue;
        private Vector4 _originalInitValue;
        private Vector4[] _originalFinalValues;
        private Vector4[] _currentFinalValues;
        private Action<Vector4> _onSetAction;

        private Vector4 _pastValue;
        private bool _useCurve;
        private float _waitAtEndPath = 0f;

        private float _currentLenght = 0;
        private float _maxLenght = 0;
        private float _velocity = 0;
        private bool _angle = false;
        private TimeType _timetype = TimeType.Scaled;
        private float _error;
        private TweenState _currentState;
        private TweenState _oldState;
        #endregion

        #region Events
        public event Action<Tween, float> OnComplete;
        #endregion

        #region Propieties
        public Vector4[] FinalValues
        {
            get { return _originalFinalValues; }
            set
            {
                _originalFinalValues = value;
                _currentFinalValues = _originalFinalValues.Clone<Vector4>();

                ChangePathForInverse();
                ChangePathForRelative();
                ChangePathForPingPong();

                _pastValue = Lerp(0);
            }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public float Velocity
        {
            get { return _velocity; }
            set { _velocity = value; }
        }

        public bool Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        public Vector4 GetCurrentValue
        {
            get
            {
                if (_getCurrentValue != null)
                {
                    return _getCurrentValue.Invoke();
                }

                LogManager.LogWarning("Error in GetCurrentValue");
                return default;
            }
        }

        public float TimePercentCurrent
        {
            get
            {
                if (_timer != null)
                {
                    return _timer.TimeInPercent;
                }

                LogManager.LogWarning("Error in TimePercentCurrent");
                return 0;
            }
        }

        public TypeReferenceEnum PathReference
        {
            get { return _pathReference; }
            set { _pathReference = value; }
        }

        public bool IsInverse
        {
            get { return _isInverse; }
            set { _isInverse = value; }
        }

        public bool IsLoop
        {
            get { return _isLoop; }
            set { _isLoop = value; }
        }

        public TweenState CurrentState
        {
            get { return _currentState; }
        }

        public bool PingPong
        {
            get { return _pingPong; }
            set { _pingPong = value; }
        }

        public float WaitAtEndPath
        {
            get { return _waitAtEndPath; }
            set { _waitAtEndPath = value; }
        }

        public float CurveFactor
        {
            get { return _curveFactor; }
            set { _curveFactor = value; }
        }

        public FunctionEnum Function
        {
            get { return _curve != null ? _curve.Function : FunctionEnum.Null; }
            set
            {
                if (_curve != null)
                {
                    _curve.Function = value;
                }
            }
        }

        public Vector4 InitValue { get { return _originalInitValue; } set { _originalInitValue = value; } }
        #endregion

        #region Constructors
        public Tween()
        {
            _timer = new Timer();
            _currentState = TweenState.Inactive;
        }
        #endregion

        #region Public Methods
        public bool CreateTween(in Func<Vector4> currentValue, in float timeOrVelocity, in Action<Vector4> onSetAction, in AnimationCurveInfo newCurve, in ChangeModes changeMode, params Vector4[] finalValues)
        {
            _currentState = TweenState.Inactive;
            _getCurrentValue = currentValue;
            _originalFinalValues = finalValues;
            _onSetAction = onSetAction;
            _curve = newCurve;
            _changeMode = changeMode;
            _useCurve = newCurve.Function != FunctionEnum.Linear;
            Pause(false);

            CreateInitValue();


            if (_changeMode == ChangeModes.Time)
            {
                duration = timeOrVelocity;
            }
            else
            {
                _velocity = timeOrVelocity;
            }

            _currentLenght = 0;

            return true;
        }

        public bool Play(in bool setInit = false, in TimeType timeType = TimeType.Scaled, in float preserved = 0)
        {
            if (_currentState == TweenState.Pause)
            {
                return false;
            }

            _currentState = TweenState.Active;

            if (setInit)
            {
                CreateInitValue();
            }

            _currentFinalValues = _originalFinalValues.Clone<Vector4>();
            _initValue = _originalInitValue;
            _timetype = timeType;

            ChangePathForInverse();
            ChangePathForRelative();
            ChangePathForPingPong();

            PearlInvoke.StopTimer<float, float>(OnFinishWait);

            float percent = 0;

            if (_changeMode == ChangeModes.Velocity)
            {
                CalculateLenght();

                _currentLenght = _velocity * preserved;
            }

            if (!_currentFinalValues.IsAlmostSpecificCount() || (duration == 0 && _changeMode == ChangeModes.Time) || (_maxLenght == 0 && _changeMode == ChangeModes.Velocity))
            {
                ForceFinish();
                return false;
            }

            if (_changeMode == ChangeModes.Time)
            {
                _timer.ResetOn(duration, _timetype, preserved);
                percent = _timer.TimeInPercent;
            }

            _pastValue = _getCurrentValue != null ? _getCurrentValue.Invoke() : _pastValue;

            if (preserved != 0)
            {
                Evalutate(0);
                if (IsFinish())
                {
                    return false;
                }
            }

            return true;
        }

        public void ForceTween(in float value, in bool usePercent = true)
        {
            if (_currentState == TweenState.Inactive || _currentState == TweenState.Pause)
            {
                return;
            }

            if (_changeMode == ChangeModes.Time)
            {
                if (usePercent)
                {
                    _timer.ForceTimeInPercent(value);
                }
                else
                {
                    _timer.ForceTime(value);
                }
            }
            else
            {
                _currentLenght = usePercent ? Mathf.Clamp01(value) * _maxLenght : Mathf.Clamp(value, 0, _maxLenght);
            }

            _currentState = TweenState.Active;
            PearlInvoke.StopTimer<float, float>(OnFinishWait);

            Evalutate(0);
            IsFinish();
        }

        public void SetInitValue(in Vector4 value, in bool initValue = true)
        {
            _onSetAction?.Invoke(GetVectorFromValue(value));

            if (initValue)
            {
                _initValue = value;
                _originalInitValue = _initValue;
                _pastValue = _initValue;
            }
        }

        public void ForceFinish()
        {
            if (_currentState == TweenState.Inactive || _currentState == TweenState.Pause)
            {
                return;
            }

            if (_currentFinalValues != null)
            {
                int aux = _currentFinalValues.Length - 1;
                if (_currentFinalValues.IsAlmostSpecificCount())
                {
                    _onSetAction?.Invoke(GetVectorFromValue(_currentFinalValues[aux]));
                }
                else
                {
                    LogManager.LogWarning("This tween is wrong");
                }
                Finish();
            }
        }

        public void Stop()
        {
            if (_currentState == TweenState.Inactive || _currentState == TweenState.Pause)
            {
                return;
            }

            _currentState = TweenState.Inactive;
            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            _timer.ResetOff();
        }

        public void Reset()
        {
            if (_currentState == TweenState.Inactive || _currentState == TweenState.Pause)
            {
                return;
            }

            _onSetAction?.Invoke(GetVectorFromValue(_originalInitValue));
            Stop();
        }

        public bool IsFinish()
        {
            if (_currentState == TweenState.Inactive || _currentState == TweenState.Pause || CurrentState == TweenState.Wait)
            {
                return false;
            }

            if (_currentState == TweenState.Complete)
            {
                return true;
            }

            bool isFinish = false;
            if (_changeMode == ChangeModes.Time)
            {
                isFinish = _timer.On && _timer.IsFinish(out _, out _error);
            }
            else
            {
                isFinish = _currentLenght >= _maxLenght;
                //trasformare distanza in tempo
                _error /= _velocity;
            }


            if (isFinish)
            {
                if (_isLoop)
                {
                    isFinish = false;

                    _currentState = TweenState.Wait;
                    var wait = _waitAtEndPath - _error;
                    if (wait > 0)
                    {
                        PearlInvoke.WaitAccurateForMethod(wait, OnFinishWait);
                    }
                    else
                    {
                        OnFinishWait(0, MathF.Abs(_error));
                    }
                }
                else
                {
                    Finish();
                }
            }
            return isFinish;
        }

        public void Evalutate(float delta)
        {
            Vector4 result = Lerp(delta);
            Vector4 currentValue = result - _pastValue;
            _pastValue = result;
            _onSetAction?.Invoke(currentValue);
        }
        #endregion

        #region Private Methods
        private Vector4 Lerp(float delta)
        {
            if (_currentState != TweenState.Active && _currentState != TweenState.Wait)
            {
                return default;
            }

            if ((!_timer.On && _changeMode == ChangeModes.Time) || _currentFinalValues == null)
            {
                return default;
            }

            float percent;
            if (_changeMode == ChangeModes.Time)
            {
                percent = _timer.TimeInPercent;
            }
            else
            {
                _currentLenght += _velocity * delta;
                _error = Mathf.Max(_currentLenght - _maxLenght, 0);
                _currentLenght = Mathf.Clamp(_currentLenght, 0, _maxLenght);
                percent = Mathf.InverseLerp(0, _maxLenght, _currentLenght);
            }

            if (_useCurve && _curve != null)
            {
                percent = _curve.Evaluate(percent);
            }

            float aux = _currentFinalValues.Length * percent;

            int rightIndex = Mathf.FloorToInt(aux);
            rightIndex = rightIndex == _currentFinalValues.Length ? rightIndex - 1 : rightIndex;

            if (!_currentFinalValues.IsAlmostSpecificCount(rightIndex))
            {
                return default;
            }

            Vector4 a = rightIndex == 0 ? _initValue : _currentFinalValues[rightIndex - 1];
            Vector4 b = _currentFinalValues[rightIndex];

            float rangePercent = 1f / _currentFinalValues.Length;
            float maxPercent = (rightIndex + 1) * rangePercent;

            float currentPercent = MathfExtend.Percent(percent, maxPercent - rangePercent, maxPercent);

            Vector4 result;
            if (_curveFactor != 0)
            {
                result = QuadraticBezier.GetPoint(a, b, _curveFactor, currentPercent);
            }
            else
            {
                result = _angle ? Vector4Extend.LerpAngle(a, b, currentPercent) : Vector4.Lerp(a, b, currentPercent);
            }

            return result;
        }

        private Vector4 GetVectorFromValue(in Vector4 value)
        {
            Vector4 currentValue = _getCurrentValue != null ? _getCurrentValue.Invoke() : Vector4.zero;
            return value - currentValue;
        }

        private void OnFinishWait(float leftAccurate, float rightAccurate)
        {
            if (_pathReference == TypeReferenceEnum.Relative)
            {
                CreateInitValue();
            }

            Play(false, _timetype, rightAccurate);
        }

        private void CalculateLenght()
        {
            _maxLenght = 0;
            if (_currentFinalValues.IsAlmostSpecificCount())
            {
                _maxLenght = Vector4.Distance(_initValue, _currentFinalValues[0]);
                for (int i = 1; i < _currentFinalValues.Length; i++)
                {
                    _maxLenght += Vector4.Distance(_currentFinalValues[i - 1], _currentFinalValues[i]);
                }
            }
        }

        private void ChangePathForInverse()
        {
            if (IsInverse)
            {
                Array.Reverse(_currentFinalValues);

                if (_pathReference == TypeReferenceEnum.Absolute)
                {
                    var aux = _initValue;
                    _initValue = _currentFinalValues[0];
                    _currentFinalValues.InsertAndLeftShift(aux, _currentFinalValues.Length - 1);
                }
            }
        }

        private void ChangePathForRelative()
        {
            if (_pathReference == TypeReferenceEnum.Relative && _originalFinalValues != null && _currentFinalValues != null)
            {
                for (int i = 0; i < _originalFinalValues.Length; i++)
                {
                    _currentFinalValues[i] = i == 0 ? _currentFinalValues[i] + _initValue : _currentFinalValues[i] + _currentFinalValues[i - 1];
                }
            }
        }

        private void ChangePathForPingPong()
        {
            if (PingPong)
            {
                Vector4[] aux = _currentFinalValues.Clone<Vector4>();
                Array.Resize(ref aux, aux.Length - 1);
                Array.Reverse(aux);
                Array.Resize(ref aux, aux.Length + 1);
                aux[^1] = _initValue;

                _currentFinalValues = _currentFinalValues.Concat(aux).ToArray();
            }
        }

        private void CreateInitValue()
        {
            if (_getCurrentValue != null)
            {
                _initValue = _getCurrentValue.Invoke();
            }
            _originalInitValue = _initValue;
            _pastValue = _initValue;
        }

        private void Finish()
        {
            _currentState = TweenState.Complete;
            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            _timer.ResetOff();
            OnComplete?.Invoke(this, _error);
        }
        #endregion

        #region Interface
        public void ResetElement()
        {

        }

        public void DisableElement()
        {
            OnComplete = null;
            PearlInvoke.StopTimer<float, float>(OnFinishWait);
            _isLoop = false;
            _pingPong = false;
            _waitAtEndPath = 0f;
            CurveFactor = 0;
            _isInverse = false;
            _error = 0;
            _currentState = TweenState.Inactive;
            _getCurrentValue = null;
            _originalFinalValues = null;
            _currentFinalValues = null;
            _onSetAction = null;
            _angle = false;
        }

        public void Pause(bool onPause)
        {
            if ((_currentState == TweenState.Pause && onPause) || (_currentState != TweenState.Pause && !onPause) || _currentState == TweenState.Inactive)
            {
                return;
            }

            if (onPause)
            {
                _oldState = _currentState;
            }

            _currentState = onPause ? TweenState.Pause : _oldState;
            _timer.Pause(onPause);
            PearlInvoke.PauseTimer<float, float>(OnFinishWait, onPause);
        }
        #endregion
    }
}