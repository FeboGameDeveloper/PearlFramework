#if STOMPYROBOT

using System.ComponentModel;
using UnityWebGLOOrientationDetction;

namespace Pearl.Debug
{
    public class WebGLDeviceTunning : TunningPage
    {
        [Category("WebGL")] // Options will be grouped by category
        public void FullScreen()
        {
            MobileOrientationDetector.FullScreen();
        }

        [Category("WebGL")] // Options will be grouped by category
        public void ExitFullScreen()
        {
            MobileOrientationDetector.ExitFullScreen();
        }

        [Category("WebGL")] // Options will be grouped by category
        public void ScreenLock()
        {
            MobileOrientationDetector.ScreenLock();
            MobileOrientationDetector.Init();
        }

        [Category("WebGL")] // Options will be grouped by category
        public void ScreenUnlock()
        {
            MobileOrientationDetector.ScreenUnlock();
        }


        [Category("WebGL")] // Options will be grouped by category
        public void DeviceInit()
        {
            MobileOrientationDetector.Init();
        }

        protected override void LoadVarsPrivate()
        {
        }

        [Category("WebGL")] // Options will be grouped by category
        public int Angle
        {
            get
            {
                return MobileOrientationDetector.GetOrientation();
            }
        }
    }
}

#endif
