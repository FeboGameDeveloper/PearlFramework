using Pearl.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TypeReferences;
using UnityEngine;

namespace Pearl.Testing.ScreenVars
{
    #region Struct
    public struct DebugVar
    {
        public MemberComplexInfo member;
        public string name;

        public DebugVar(string name, MemberComplexInfo member)
        {
            this.name = name;
            this.member = member;
        }
    }
    #endregion

    public class DebugScreenManager : PearlBehaviour, ISingleton
    {
        #region Inspector fields
        [SerializeField]
        private StringBoolDictionary debugView = null;
        [SerializeField]
        private GameObject debugScreenPrefab = null;
        [SerializeField]
        private GameObject debugScreenElementPrefab = null;
        [SerializeField]
        [ClassImplements(typeof(DebugScreenVarsNative))]
        protected ClassTypeReference[] initDebugScreenPages = default;
        #endregion

        #region Private Methods
        private Transform _debugScreenParent;
        private readonly List<Type> _activeTypes = new();
        private readonly List<DebugScreenElement> _activeElements = new();
        private bool _isOpen = false;
        #endregion

        #region Static Methods
        public static bool GetIstance(out DebugScreenManager result)
        {
            return Singleton<DebugScreenManager>.GetIstance(out result);
        }

        public static void AddPage(Type page)
        {
            if (GetIstance(out var debugScreenManager))
            {
                debugScreenManager.AddPageIntern(page);
            }
        }

        public static void OpenDebugScreen(bool open)
        {
            if (GetIstance(out var debugScreenManager))
            {
                debugScreenManager.OpenDebugScreenIntern(open);
            }
        }

        public static void RemovePage(Type page)
        {
            if (GetIstance(out var debugScreenManager))
            {
                debugScreenManager.RemovePageIntern(page);
            }
        }
        #endregion

        #region Unity Callbacks
        protected override void Start()
        {
            base.Start();

            if (DebugManager.GetActiveDebug("debugScreen"))
            {
                OpenDebugScreenIntern(true);
            }
        }

        private void OnValidate() => UnityEditor.EditorApplication.delayCall += OnValidatePrivate;

        private void OnValidatePrivate()
        {
            UnityEditor.EditorApplication.delayCall -= OnValidatePrivate;
            if (this == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Type[] types = _activeTypes.ToArray();
                ClearDebugScreen();

                foreach (var type in types)
                {
                    AddPage(type);
                }
            }
        }

        #endregion

        #region Private methods
        private void OpenDebugScreenIntern(bool open)
        {
            if (open && !_isOpen)
            {
                _isOpen = true;
                InitDebugScreen();
            }
            else if (!open && _isOpen)
            {
                _isOpen = false;
                ClearDebugScreen();
                GameObject.Destroy(_debugScreenParent.gameObject);
            }
        }

        private void InitDebugScreen()
        {
            GameObjectExtend.CreateUIlement(debugScreenPrefab, out _debugScreenParent, canvasTipology: CanvasTipology.Debug);
            if (_debugScreenParent != null)
            {
                _debugScreenParent.DestroyAllChild();
            }

            foreach (var page in initDebugScreenPages)
            {
                AddPage(page.Type);
            }
        }

        private void AddPageIntern(Type page)
        {
            if (page == null && _activeTypes == null 
                && _activeElements == null && _activeTypes.Exists((x) => x == page)
                && !page.IsSubclassOf(typeof(DebugScreenVarsNative)))
            {
                return;
            }

            _activeTypes.Add(page);
            object obj = Activator.CreateInstance(page);

            var members = obj.GetType().GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            MemberComplexInfo[] membersInfos = new MemberComplexInfo[members.Length];
            for (int i = 0; i < members.Length; i++)
            {
                membersInfos[i] = (MemberComplexInfo)members[i].Invoke(obj, null);
            }

            for (int i = 0; i < members.Length; i++)
            {
                var name = ReflectionExtend.GetValueAttribute(members[i], (DebugScreenAttribute attr) =>
                {
                    return attr.DebugName;
                });

                var category = ReflectionExtend.GetValueAttribute(members[i], (DebugScreenAttribute attr) =>
                {
                    return attr.DebugCategory;
                });

                if (category != null && debugView.IsNotNullAndTryGetValue(category, out bool result) && !result)
                {
                    continue;
                }


                GameObjectExtend.CreateGameObject<DebugScreenElement>(debugScreenElementPrefab, out DebugScreenElement debugElement, "Element", default, default, _debugScreenParent);
                if (debugElement != null)
                {
                    _activeElements.Add(debugElement);
                    debugElement.SetField(membersInfos[i], page, name);
                }
            }

        }

        private void RemovePageIntern(Type page)
        {
            _activeTypes.Remove(page);
            var indexs = _activeElements.FindAllIndex(i => _activeElements[i].IndexType == page).SortInverse();
            foreach (int index in indexs)
            {
                GameObject.Destroy(_activeElements[index].gameObject);
                _activeElements.RemoveAt(index);
            }
        }


        private void ClearDebugScreen()
        {
            if (_debugScreenParent != null)
            {
                _debugScreenParent.DestroyAllChild();
            }
            _activeTypes.Clear();
            _activeElements.Clear();
        }
        #endregion
    }
}
