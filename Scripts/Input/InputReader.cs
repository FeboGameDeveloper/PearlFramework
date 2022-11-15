using Pearl.Events;
using Pearl.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class InputReader : PearlBehaviour
    {
        #region Auxiliars Classes
        [Serializable]
        public class InputInfo
        {
            public string nameAction;
            public int numberPlayer = 0;
            public InputType output;

            [ConditionalField("@output == Button")]
            public StateButton stateButton;
            [ConditionalField("@output != Button")]
            public bool raw;
            [ConditionalField("@output != Button")]
            public bool ignoreZeroValue = false;

            [ConditionalField("@output == Button")]
            public SimpleEvent action;
            [ConditionalField("@output == Float")]
            public FloatEvent floatAction;
            [ConditionalField("@output == Vector2")]
            public VectorEvent vectorAction;

            public Action auxAction;
        }
        #endregion

        #region Inspector Fields
        [SerializeField]
        private InputInfo[] inputInfos;
        #endregion

        #region Private Fields
        private List<InputInfo> _inputsNoVoid = new();
        #endregion

        #region Unity Callbacks
        // Start is called before the first frame update
        protected override void OnEnableAfterStart()
        {
            AnalizeVoidInput(ActionEvent.Add);

            for (int i = 0; i < inputInfos.Length; i++)
            {
                var inputInfo = inputInfos[i];

                if (inputInfo.output != InputType.Button)
                {
                    _inputsNoVoid.Add(inputInfo);
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            AnalizeVoidInput(ActionEvent.Remove);
        }

        // Update is called once per frame
        private void Update()
        {
            if (_inputsNoVoid == null)
            {
                return;
            }

            for (int i = 0; i < _inputsNoVoid.Count; i++)
            {
                var inputInfo = _inputsNoVoid[i];
                var inputInterface = InputManager.Players[inputInfo.numberPlayer];

                if (inputInterface == null)
                {
                    continue;
                }

                if (inputInfo.output == InputType.Float)
                {
                    var valueFloat = inputInterface.GetAxis(inputInfo.nameAction, inputInfo.raw);
                    if (!inputInfo.ignoreZeroValue || valueFloat != 0)
                    {
                        inputInfo.floatAction?.Invoke(valueFloat);
                    }
                }
                else if (inputInfo.output == InputType.Vector2)
                {
                    var valueVector = inputInterface.GetVectorAxis(inputInfo.nameAction, inputInfo.raw);
                    if (!inputInfo.ignoreZeroValue || valueVector != Vector2.zero)
                    {
                        inputInfo.vectorAction?.Invoke(valueVector);
                    }
                }
            }
        }
        #endregion

        #region Private Fields
        private void AnalizeVoidInput(ActionEvent actionEvent)
        {
            for (int i = 0; i < inputInfos.Length; i++)
            {
                var inputInfo = inputInfos[i];

                if (inputInfo.output == InputType.Button)
                {
                    var inputInterface = InputManager.Players[inputInfo.numberPlayer];

                    if (inputInterface == null)
                    {
                        continue;
                    }

                    if (actionEvent == ActionEvent.Add)
                    {
                        inputInfo.auxAction = () => inputInfo.action?.Invoke();
                    }

                    inputInterface.PerformedHandle(inputInfo.nameAction, inputInfo.auxAction, actionEvent, inputInfo.stateButton);
                }
            }
        }
        #endregion
    }
}

