using UnityEngine;
using UnityEngine.InputSystem;

namespace Pearl.Input
{
    public static class PointerExtend
    {
        #region private fields
        private static readonly Pointer _currentDevice;
        #endregion

        #region Constructor
        static PointerExtend()
        {
            _currentDevice = UnityEngine.Device.SystemInfo.deviceType == DeviceType.Handheld ? UnityEngine.InputSystem.Touchscreen.current : Mouse.current;
        }
        #endregion

        #region Property
        public static Vector2 Delta
        {
            get
            {
                return _currentDevice != null && InputManager.EnableInput ? _currentDevice.delta.ReadValue() : Vector2.zero;
            }
        }

        public static bool EnablePointer
        {
            set
            {
                Cursor.visible = value;
                Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            }
        }
        #endregion

        #region Public methods
        public static Vector2 GetScreenPosition()
        {
            if (_currentDevice != null)
            {
                var position = _currentDevice.position;
                if (position != null)
                {
                    return position.ReadValue();
                }
            }

            return default;
        }

        public static bool PointerIsPressed()
        {
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                return Touchscreen.current.primaryTouch.isInProgress;
            }

            return Mouse.current.leftButton.isPressed;
        }

        public static Vector3 PointerWorldPosition(this Camera @this)
        {
            var position = GetScreenPosition();
            return @this != null ? @this.ScreenToWorldPoint(position) : default;
        }

        public static Vector3 PointerWorldPosition()
        {
            return PointerWorldPosition(Camera.main);
        }

        public static bool IsPointerOver(Bounds bounds, bool is2D = false)
        {
            return IsPointerOver(bounds, Camera.main, is2D);
        }

        public static bool IsPointerOver(Bounds bounds, Camera camera, bool is2D = false)
        {
            if (camera == null)
            {
                return false;
            }

            var position = camera.PointerWorldPosition();
            if (is2D)
            {
                position.z = bounds.center.z;
                return bounds.Contains(position);
            }
            else
            {
                var ray = camera.ScreenPointToRay(position);
                return bounds.IntersectRay(ray);
            }
        }
        #endregion
    }
}
