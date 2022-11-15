using System;
using UnityEngine;

namespace Pearl.Input
{
    [Serializable]
    public class ReaderNumericInput
    {
        #region Field Inspector
        [SerializeField]
        private string inputString = "Move";
        [SerializeField]
        private float minTime = 0;
        [SerializeField]
        private bool isVector = false;
        [ConditionalField("@isVector")]
        public Axis2DEnum axisEnum;

        private float _deltaTime = Mathf.Infinity;
        private float _oldTime;
        #endregion

        #region Property
        public float MinTime { get { return minTime; } set { minTime = value; } }
        public bool IsVector { get { return isVector; } set { isVector = value; } }
        public string InputString { get { return inputString; } set { inputString = value; } }
        public Axis2DEnum AxisEnum { get { return axisEnum; } set { axisEnum = value; } }

        #endregion

        #region Constructor
        public ReaderNumericInput(string inputString)
        {
            this.inputString = inputString;
            this._oldTime = Time.unscaledTime;
        }

        public ReaderNumericInput(string inputString, Axis2DEnum axisEnum) : this(inputString)
        {
            this.isVector = true;
            this.axisEnum = axisEnum;
        }

        public ReaderNumericInput(string inputString, float minTime) : this(inputString)
        {
            this.minTime = minTime;
        }

        public ReaderNumericInput(string inputString, Axis2DEnum axisEnum, float minTime) : this(inputString, axisEnum)
        {
            this.minTime = minTime;
        }
        #endregion

        #region Public Method
        public void ResetDeltaTime()
        {
            _oldTime = Time.unscaledTime;
        }

        public void GetValue(Action<Vector2> callback, bool raw = false)
        {
            GetValue(callback, 0, raw);
        }

        public void GetValue(Action<Vector2> callback, int player, bool raw = false)
        {
            var manager = InputManager.Get(player);
            _deltaTime = Time.unscaledTime - _oldTime;
            Vector2 value = manager.GetVectorAxis(inputString, raw);
            if (value != Vector2.zero && _deltaTime >= minTime)
            {
                ResetDeltaTime();
                callback?.Invoke(value);
            }
        }

        public void GetValue(Action<float> callback, bool raw = false)
        {
            GetValue(callback, 0, raw);
        }

        public void GetValue(Action<float> callback, int player, bool raw = false)
        {
            var manager = InputManager.Get(player);
            _deltaTime = Time.unscaledTime - _oldTime;

            float value;
            if (isVector)
            {
                Vector2 vector = manager.GetVectorAxis(inputString, raw);
                value = axisEnum == Axis2DEnum.X ? vector.x : vector.y;
            }
            else
            {
                value = manager.GetAxis(inputString, raw);
            }

            if (value != 0 && _deltaTime >= minTime)
            {
                ResetDeltaTime();
                callback?.Invoke(value);
            }
        }
        #endregion
    }
}
