using Pearl.Input;
using System;
using UnityEngine;

namespace Pearl.Editor
{
    [CreateAssetMenu(fileName = "ButtonTextForInput", menuName = "Pearl/Input/ButtonTextForInput", order = 1)]
    public class ButtonTextForCommandScriptableObject : ScriptableObject
    {
        [Serializable]
        public class ButtonTextForDevice
        {
            [SerializeField]
            private InputDeviceEnumstringDictionary stringForDevice = null;

            public string GetText(InputDeviceEnum inputDevice)
            {
                if (stringForDevice.IsNotNullAndTryGetValue(inputDevice, out string text))
                {
                    return text;
                }
                return null;
            }
        }


        [SerializeField]
        private StringButtonTextForDeviceDictionary buttonTextForInputEvent = null;

        public string GetText(in string inputEvent)
        {
            if (inputEvent != null && buttonTextForInputEvent.IsNotNullAndTryGetValue(inputEvent, out ButtonTextForDevice buttonTextForDevice))
            {
                var manager = InputManager.Input;
                InputDeviceEnum device = manager ? manager.CurrentInputDevice : InputDeviceEnum.Null;
                return buttonTextForDevice.GetText(device);
            }
            return null;
        }
    }
}
