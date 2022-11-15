using Pearl.FrameRate;
using Pearl.UI;
using UnityEngine;

namespace Pearl
{
    public class DebugManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private StringBoolDictionary debugViews = null;
        [SerializeField]
        private FrameRateManager frameRateManager = null;
        [SerializeField]
        private GameObject frameRatePrefab = null;

        [SerializeField]
        private GameObject tunning = null;

        public const string debugFPS = "debugFPS";
        public const string debugInScreen = "debugScreen";
        public const string consoleInGameString = "consoleInGame";

        public static bool GetIstance(out DebugManager result)
        {
            return Singleton<DebugManager>.GetIstance(out result);
        }

        public static bool GetActiveDebug(in string debugString)
        {
            if (GetIstance(out var debugManager))
            {
                return debugManager != null && debugManager.GetActiveDebugPrivate(debugString);
            }
            return false;
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

#if STOMPYROBOT
            if (tunning)
            {
                GameObjectExtend.CreateGameObject(tunning, out _, transform, true);
            }
#endif

            CreateDebugElements();
        }

        public void SetActiveDebug(in string debugString, bool value)
        {
            if (debugViews != null)
            {
                debugViews.Update(debugString, value);
            }
        }

        private bool GetActiveDebugPrivate(in string debugString)
        {
            debugViews.IsNotNullAndTryGetValue(debugString, out bool valueDebug);
            return valueDebug;
        }

        private void CreateDebugElements()
        {
            if (debugViews != null)
            {
                if (debugViews.TryGetValue(debugFPS, out bool isActive))
                {
                    if (isActive)
                    {
                        GameObjectExtend.CreateUIlement(frameRatePrefab, out _, CanvasTipology.Debug);
                    }

                    if (frameRateManager)
                    {
                        frameRateManager.enabled = isActive;
                    }
                }

                if (debugViews.TryGetValue(debugInScreen, out isActive))
                {
                    if (frameRateManager)
                    {
                        frameRateManager.enabled = isActive;
                    }
                }
            }
        }
    }
}
