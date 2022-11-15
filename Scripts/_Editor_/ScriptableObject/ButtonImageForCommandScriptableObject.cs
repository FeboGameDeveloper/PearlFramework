using Pearl.Input;
using System;
using UnityEngine;

namespace Pearl.Editor
{
    [CreateAssetMenu(fileName = "ButtonImageForInput", menuName = "Pearl/Input/ButtonImageForInput", order = 1)]
    public class ButtonImageForCommandScriptableObject : ScriptableObject
    {
        [Serializable]
        public class ButtonImageForDevice
        {
            [SerializeField]
            private InputDeviceEnumImageDictionary imageForDevice = null;

            public Sprite GetSprite()
            {
                var manager = InputManager.Input;
                return manager ? GetSprite(manager.CurrentInputDevice) : null;
            }

            public Sprite GetSprite(InputDeviceEnum inputDevice)
            {
                if (imageForDevice.IsNotNullAndTryGetValue(inputDevice, out Sprite correctImage))
                {
                    return correctImage;
                }
                return null;
            }
        }


        [SerializeField]
        private StringButtonImageForDeviceDictionary buttonImageForInputEvent = null;

        public Sprite GetSprite(in string inputEvent)
        {
            if (buttonImageForInputEvent != null && inputEvent != null)
            {
                if (inputEvent != null && buttonImageForInputEvent.TryGetValue(inputEvent, out ButtonImageForDevice buttonImageForDevice))
                {
                    var manager = InputManager.Input;
                    InputDeviceEnum device = manager ? manager.CurrentInputDevice : InputDeviceEnum.Null;
                    return buttonImageForDevice.GetSprite(device);
                }
            }
            return null;
        }
    }
}
