#if STOMPYROBOT

using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.Debug
{
    public class TunningManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        [ClassImplements(typeof(TunningPage))]
        protected ClassTypeReference[] initTunningPages = default;
        [SerializeField]
        [ClassImplements(typeof(TunningPage))]
        protected ClassTypeReference[] tunningPages = default;

        [SerializeField]
        protected bool useTunningVars = false;

        private readonly Dictionary<Type, TunningPage> _tunningPageObjects = new();

        private readonly List<TunningPage> _activeTunningPages = new();

        private const string _SRDebuggerString = "SRDebugger";

        public static bool GetIstance(out TunningManager result)
        {
            return Singleton<TunningManager>.GetIstance(out result);
        }

        public static bool UseTunningVars
        {
            get { return GetIstance(out var tunningManager) && tunningManager.useTunningVars; }
        }


        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (!DebugManager.GetActiveDebug(DebugManager.consoleInGameString))
            {
                return;
            }

            if (initTunningPages != null)
            {
                foreach (var tunningPage in initTunningPages)
                {
                    CretePage(tunningPage);
                }
            }

            if (tunningPages != null)
            {
                foreach (var tunningPage in tunningPages)
                {
                    CretePage(tunningPage);
                }
            }

            if (initTunningPages != null && _tunningPageObjects != null)
            {
                foreach (var tunningPage in initTunningPages)
                {
                    if (_tunningPageObjects.TryGetValue(tunningPage, out TunningPage page))
                    {
                        AddOption(page);
                    }
                }
            }

            if (Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform)
            {
                CretePage(typeof(WebGLDeviceTunning));
                AddOption(typeof(WebGLDeviceTunning));
            }
        }

        protected override void Start()
        {
            base.Start();

            GameObject SRDebugger = GameObject.Find(_SRDebuggerString);

            if (!DebugManager.GetActiveDebug(DebugManager.consoleInGameString))
            {
                SRDebug.Instance?.DestroyDebugPanel();
                GameObject.Destroy(SRDebugger);
                return;
            }
            else if (SRDebugger != null)
            {
                SRDebugger.transform.parent = transform;

                var button = SRDebugger.GetChildInHierarchy<Button>("SR_TapButton");
                if (button)
                {
                    button.navigation = default;
                }
            }
        }

        public static void AddOption(Type typeTunningPage)
        {
            if (!DebugManager.GetActiveDebug(DebugManager.consoleInGameString))
            {
                return;
            }

            if (GetIstance(out var tunningManager))
            {
                tunningManager.AddOptionInternal(typeTunningPage);
            }
        }

        public static void RemoveOption(Type typeTunningPage)
        {
            if (!DebugManager.GetActiveDebug(DebugManager.consoleInGameString))
            {
                return;
            }

            if (GetIstance(out var tunningManager))
            {
                tunningManager.RemoveOptionInternal(typeTunningPage);
            }
        }

        private void AddOptionInternal(Type typeTunningPage)
        {
            if (typeTunningPage.IsSubclassOf(typeof(TunningPage)) && _tunningPageObjects.IsNotNullAndTryGetValue(typeTunningPage, out TunningPage page) && page != null)
            {
                AddOption(page);
            }
        }

        private void RemoveOptionInternal(Type typeTunningPage)
        {
            if (typeTunningPage.IsSubclassOf(typeof(TunningPage)) && _tunningPageObjects.IsNotNullAndTryGetValue(typeTunningPage, out TunningPage page) && page != null)
            {
                RemoveOption(page);
            }
        }

        private void CretePage(Type option)
        {
            object obj = Activator.CreateInstance(option);
            if (obj is TunningPage page)
            {
                _tunningPageObjects?.Add(option, page);
            }
        }

        private void AddOption(TunningPage page)
        {
            ReflectionExtend.UseMethod(page, "Init");
            SRDebug.Instance?.AddOptionContainer(page);
            _activeTunningPages?.Add(page);
        }

        private void RemoveOption(TunningPage page)
        {
            SRDebug.Instance?.RemoveOptionContainer(page);
            _activeTunningPages?.Remove(page);
        }
    }
}

#endif
