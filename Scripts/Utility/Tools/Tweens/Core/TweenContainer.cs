using Pearl.Debug;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Tweens
{
    /// <summary>
    /// This class manages the tween: it performs updates and notifies the user when they expire.
    /// </summary>
    public class TweenContainer : IReset, IPause
    {
        #region Static

        #region Private Fields
        private readonly static SimplePool<TweenContainer> _containerPool = new(false);
        private readonly static List<TweenContainer> _tweenActiveList = new();
        #endregion

        static TweenContainer()
        {
            GameManager.OnUpate += Update;
        }

        #region Static Methods
        //to create a tween: To do after awake
        public static TweenContainer CreateTween(Func<Vector4> currentValue, float timeOrVelocity, Action<Vector4> setAction, bool isAutoKill, AnimationCurveInfo curve, ChangeModes changeMode, params Vector4[] newValue)
        {
            TweenContainer tweenContainer = _containerPool.Get();

            if (tweenContainer != null)
            {
                tweenContainer.CreateTweenContainer(currentValue, timeOrVelocity, setAction, isAutoKill, curve, changeMode, newValue);
                return tweenContainer;
            }
            else
            {
                LogManager.LogWarning("Creation tween failed");
                return null;
            }
        }
        #endregion

        #region Private Methods
        private static void Update()
        {
            if (_tweenActiveList == null)
            {
                return;
            }

            for (int i = _tweenActiveList.Count - 1; i >= 0; i--)
            {
                _tweenActiveList[i]?.Evalutate();
            }
        }

        private static void DisactiveTween(TweenContainer tweenContainer)
        {
            _tweenActiveList.Remove(tweenContainer);
        }

        private static void Kill(TweenContainer tweenContainer)
        {
            if (tweenContainer != null)
            {
                _tweenActiveList.Remove(tweenContainer);
                _containerPool.Remove(tweenContainer);
            }
        }

        private static void Play(TweenContainer tweenContainer)
        {
            _tweenActiveList.AddOnce(tweenContainer);
        }
        #endregion

        #endregion

        #region No Static

        #region Events

        public Action<TweenContainer> OnKill;
        public Action<TweenContainer, float> OnComplete;

        #endregion

        #region Private fields

        private readonly Tween _tween;
        private bool _isAutoKill;

        #endregion

        #region Constructor

        public TweenContainer()
        {
            _tween = new Tween();
        }

        #endregion

        #region Propiety

        public TypeReferenceEnum PathReference
        {
            get { return _tween.PathReference; }
            set { _tween.PathReference = value; }
        }

        public TweenState CurrentState
        {
            get { return _tween.CurrentState; }
        }

        public bool IsLoop
        {
            get { return _tween.IsLoop; }
            set { _tween.IsLoop = value; }
        }

        public bool PingPong
        {
            get { return _tween.PingPong; }
            set { _tween.PingPong = value; }
        }

        public bool Angle
        {
            get { return _tween.Angle; }
            set { _tween.Angle = value; }
        }

        public bool IsKill
        {
            get; private set;
        }

        public Vector4[] FinalValues
        {
            get { return _tween.FinalValues; }
            set { _tween.FinalValues = value; }
        }

        public bool IsInverse
        {
            get { return _tween.IsInverse; }
            set { _tween.IsInverse = value; }
        }

        public bool IsAutoKill
        {
            get { return _isAutoKill; }
            set { _isAutoKill = value; }
        }

        public Vector4 GetCurrentValue
        {
            get
            {
                return _tween.GetCurrentValue;
            }
        }

        public float TimePercentCurrent
        {
            get
            {
                return _tween.TimePercentCurrent;
            }
        }

        public FunctionEnum Function
        {
            get { return _tween.Function; }
            set { _tween.Function = value; }
        }
        public float Duration
        {
            get { return _tween.Duration; }
            set { _tween.Duration = value; }
        }

        public float WaitAtEndPath
        {
            get { return _tween.WaitAtEndPath; }
            set { _tween.WaitAtEndPath = value; }
        }

        public float Velocity
        {
            get { return _tween.Velocity; }
            set { _tween.Velocity = value; }
        }

        public float CurveFactor
        {
            get { return _tween.CurveFactor; }
            set { _tween.CurveFactor = value; }
        }

        public Vector4 InitValue { get { return _tween.InitValue; } set { _tween.InitValue = value; } }

        #endregion

        #region Public Methods
        public void SetInitValue(in Vector4 value, in bool initValue = true)
        {
            if (IsKill)
            {
                return;
            }

            _tween?.SetInitValue(value, initValue);
        }

        public void ForceTween(in float value, in bool percent = true)
        {
            if (IsKill)
            {
                return;
            }

            _tween?.ForceTween(value, percent);
        }

        public void Play(in bool setInit = false, in TimeType timeType = TimeType.Scaled)
        {
            if (IsKill)
            {
                return;
            }

            if (_tween != null)
            {
                if (_tween.Play(setInit, timeType))
                {
                    Play(this);
                }
            }
        }

        public void Reset()
        {
            if (IsKill)
            {
                return;
            }

            if (_tween != null)
            {
                _tween.Reset();
                DisactiveTween(this);
            }
        }

        public void ForceFinish()
        {
            if (IsKill)
            {
                return;
            }

            if (_tween != null)
            {
                _tween.ForceFinish();
            }
        }

        public void Stop()
        {
            if (IsKill)
            {
                return;
            }

            if (_tween != null)
            {
                _tween.Stop();
            }
        }

        public void Pause(bool onPause)
        {
            if (IsKill)
            {
                return;
            }

            _tween?.Pause(onPause);
        }

        public void Kill()
        {
            if (!IsKill)
            {
                OnKill?.Invoke(this);
                Kill(this);
                OnKill = null;
                OnComplete = null;
                IsKill = true;
            }
        }

        private void OnCompleteTween(Tween tween, float error)
        {
            OnComplete?.Invoke(this, error);

            if (IsAutoKill)
            {
                Kill();
            }
            else
            {
                DisactiveTween(this);
            }
        }

        #endregion

        #region Private Methods
        private void Evalutate()
        {
            if (IsKill)
            {
                return;
            }

            _tween.Evalutate(Time.deltaTime);
            _tween.IsFinish();
        }

        private void CreateTweenContainer(in Func<Vector4> currentValue, float timeOrVelocity, in Action<Vector4> setAction, in bool isAutoKill, in AnimationCurveInfo curve, in ChangeModes changeMode, params Vector4[] newValue)
        {
            IsKill = false;
            IsAutoKill = isAutoKill;
            _tween.CreateTween(currentValue, timeOrVelocity, setAction, curve, changeMode, newValue);
            _tween.OnComplete += OnCompleteTween;
        }

        #endregion

        #region IReset
        public void ResetElement()
        {
            _tween?.ResetElement();
        }

        public void DisableElement()
        {
            OnKill = null;
            _tween?.DisableElement();
        }
        #endregion

        #endregion
    }
}