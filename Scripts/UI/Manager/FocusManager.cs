using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class FocusManager : MonoBehaviour
    {
        #region Inpector field
#pragma warning disable 169, 414
        [SerializeField, ReadOnly]
        private int _layerFocus = 0;
#pragma warning restore 169, 414
        #endregion

        public const float waitForInitFocus = 0.01f;

        public const int maxElementInList = 10;

        #region Private Fields
        private static readonly Dictionary<string, List<string>> storageFocusGroup = new();
        private static GameObject _focusSaved;
        private static int _currentLayerFocus = 0;
        private static GameObject _auxFocus = null;
        private static GameObject preSelected = null;
        #endregion

        #region Events
        public static event Action<int> OnChangeLayer;
        public static event Action<GameObject> OnNewFocus;
        #endregion

        #region Static

        #region Property
        public static int CurrentLayerFocus { get { return _currentLayerFocus; } }
        #endregion

        #region Public methods

        #region Layer Focus

        public static void ChangeLayer(int layerFocus)
        {
            _currentLayerFocus = layerFocus;
            OnChangeLayer?.Invoke(_currentLayerFocus);

#if UNITY_EDITOR
            var focusManager = GameObject.FindObjectOfType<FocusManager>();
            if (focusManager != null)
            {
                focusManager._layerFocus = layerFocus;
            }
#endif
        }

        #endregion

        #region Storage Focus
        public static void PreserveCurrentFocus()
        {
            _focusSaved = GetFocus();
        }

        public static void RestoreFocusSaved()
        {
            SetFocus(_focusSaved, true);
        }

        public static void Save()
        {
            string currentFocusGroup = GetCurrentFocusGroup();
            if (!string.IsNullOrEmpty(currentFocusGroup))
            {
                AddStorageFocusGroup(currentFocusGroup);
            }

            var stack = GetCurrentList();
            if (stack != null)
            {
                GameObject aux = GetFocus();
                if (aux != null)
                {
                    if (stack.Count >= 10)
                    {
                        stack.RemoveAt(0);
                    }
                    stack.Add(aux.name);
                }
            }
        }

        public static bool IsThereHistory()
        {
            var list = GetCurrentList();
            if (list != null)
            {
                return list.Count >= 1;
            }
            return false;
        }

        public static bool IsThereHistory(string focusGroup)
        {
            if (storageFocusGroup != null && storageFocusGroup.TryGetValue(focusGroup, out var list))
            {
                return list.Count >= 1;
            }
            return false;
        }

        public static void Clear()
        {
            var list = GetCurrentList();
            if (list != null)
            {
                list.Remove();
            }
        }

        public static void Clear(string focusGroup)
        {
            if (focusGroup != null && storageFocusGroup.TryGetValue(focusGroup, out var list))
            {
                list.Remove();
            }
        }

        public static void ClearAll()
        {
            if (storageFocusGroup != null)
            {
                foreach (var list in storageFocusGroup.Values)
                {
                    if (list != null)
                    {
                        list.Remove();
                    }
                }
            }
        }
        #endregion

        #region Setter Focus
        public static void SetFocusNull()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem)
            {
                eventSystem.SetSelectedGameObject(eventSystem.gameObject);
            }
        }

        public static void SetFocus(string focusGroup, bool useTime, Transform container = null)
        {
            SetFocus(focusGroup, true, useTime, container);
        }

        public static void SetFocus(string focusGroup, bool forceLayer, bool useTime, Transform container = null)
        {
            GameObject focus = Load(focusGroup, container);
            SetFocus(focus, forceLayer, useTime);
        }

        public static void SetFocus(GameObject focus, bool useTime = false)
        {
            SetFocus(focus, true, useTime);
        }

        public static void SetFocus(GameObject focus, bool forceLayer, bool useTime = false)
        {
            if (focus != null)
            {
                Selectable childSelectable = focus.GetComponentInChildren<Selectable>();
                SetFocus(childSelectable, forceLayer, useTime);
            }
        }

        public static void SetFocus(Selectable focus, bool useTime = false)
        {
            SetFocus(focus, true, useTime);
        }

        public static void SetFocus(Selectable focus, bool forceLayer, bool useTime = false)
        {
            PearlInvoke.StopTimer<bool>(SetFocus);

            if (focus != null)
            {
                _auxFocus = focus.gameObject;

                float wait = useTime ? waitForInitFocus : 0;

                if (wait == 0)
                {
                    SetFocus(forceLayer);
                }
                else
                {
                    PearlInvoke.WaitForMethod<bool>(wait, SetFocus, forceLayer, TimeType.Unscaled);
                }
            }
        }
        #endregion

        #region Getter Focus
        public static GameObject GetFocus()
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem)
            {
                return eventSystem.currentSelectedGameObject;
            }
            return null;
        }
        #endregion

        #endregion

        #region Private methods

        private static GameObject Load(string focusGroup, Transform contaniner = null)
        {
            Selectable[] selectables = contaniner != null ? contaniner.GetComponentsInHierarchy<Selectable>()?.ToArray() :
                GameObject.FindObjectsOfType<Selectable>();

            if (storageFocusGroup.TryGetValue(focusGroup, out var list))
            {
                string nameObj = list.Count != 0 ? list[^1] : null;
                if (nameObj != null)
                {
                    foreach (var selectable in selectables)
                    {
                        if (nameObj == selectable.name)
                        {
                            return selectable.gameObject;
                        }
                    }
                }
            }
            return null;
        }

        private static void SetFocus(bool forceLayer)
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                return;
            }

            if (forceLayer && _auxFocus.TryGetComponent<FocusLayerElement>(out var element))
            {
                element.ChangeLayerInFocusManager();
            }

            eventSystem.SetSelectedGameObject(_auxFocus);
        }

        private static List<string> GetCurrentList()
        {
            string currentFocusGroup = GetCurrentFocusGroup();
            if (!string.IsNullOrEmpty(currentFocusGroup) && storageFocusGroup.TryGetValue(currentFocusGroup, out var list))
            {
                return list;
            }
            return null;
        }

        private static void AddStorageFocusGroup(string focusGroup)
        {
            if (focusGroup != null && storageFocusGroup != null && !storageFocusGroup.ContainsKey(focusGroup))
            {
                storageFocusGroup.Add(focusGroup, new List<string>());
            }
        }

        private static string GetCurrentFocusGroup()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem)
            {
                var obj = eventSystem.currentSelectedGameObject;
                if (obj && obj.TryGetComponent<FocusLayerElement>(out var focusElement))
                {
                    return focusElement.FocusGroup;
                }
            }
            return null;
        }
        #endregion

        #endregion

        #region NoStatic

        private void FixedUpdate()
        {
            CheckNewFocus();
        }

        private void CheckNewFocus()
        {
            EventSystem eventSystem = EventSystem.current;
            if (eventSystem == null)
            {
                return;
            }
            GameObject current = eventSystem.currentSelectedGameObject;
            if (current != null && preSelected != current)
            {
                preSelected = eventSystem.currentSelectedGameObject;
                OnNewFocus?.Invoke(preSelected);
            }
        }

        #endregion
    }
}
