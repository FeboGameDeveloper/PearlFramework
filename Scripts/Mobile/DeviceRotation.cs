using Pearl.Testing;
using Pearl.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pearl.Mobile
{
    public static class DeviceRotation
    {
        #region private fields
        private static bool gyroInitialized = false;
        #endregion

        #region Property
        public static bool HasGyroscope { get { return SystemInfo.supportsGyroscope; } }
        #endregion

        #region Public Methods
        public static bool Get(out Quaternion result, bool isRaw = false)
        {
            if (!InputManager.EnableInput)
            {
                result = Quaternion.identity;
                return false;
            }

            if (!gyroInitialized)
            {
                InitGyro();
            }

            result = ReadGyroscopeRotation(isRaw);

            return HasGyroscope;
        }

        public static void InitGyro()
        {
            if (HasGyroscope)
            {
                InputSystem.EnableDevice(UnityEngine.InputSystem.AttitudeSensor.current);
                UnityEngine.InputSystem.AttitudeSensor.current.samplingFrequency = 60;
            }

            gyroInitialized = true;
        }
        #endregion

        #region Private methods
        private static Quaternion ReadGyroscopeRotation(bool isRaw = false)
        {
            if (HasGyroscope)
            {
                var attitude = UnityEngine.InputSystem.AttitudeSensor.current.attitude.ReadValue();
                return isRaw ? attitude : new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * attitude * new Quaternion(0, 0, 1, 0);
            }
            else
            {
                LogManager.LogWarning("System isn't support the gyroscope");
                return Quaternion.identity;
            }
        }
        #endregion
    }
}
