using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Pearl.UI
{
    [DisallowMultipleComponent]
    public class GenericPage : PearlBehaviour
    {
        #region Serializable Fields

        [Header("Focus")]
        [SerializeField]
        protected bool useHistoricalFocus = true;

        [SerializeField, ConditionalField("@useHistoricalFocus")]
        protected bool useAnotherFocusGroup = false;

        [SerializeField, ConditionalField("@useHistoricalFocus && @useAnotherFocusGroup")]
        protected string focusGroupOnEnable = string.Empty;

        [SerializeField]
        protected bool useInitFocus = true;

        [SerializeField, ConditionalField("@useInitFocus")]
        protected GameObject firstObjectFocusable = null;

        [SerializeField]
        protected bool useNamePageForFocusGroup = true;

        [Header("Navbar")]
        [SerializeField]
        private string labelForBack = String.Empty;
        #endregion

        #region Private Fields
        protected string mode = "";
        #endregion

        #region Property
        public string Mode
        {
            set
            {
                mode = value;
                RefreshPage();
            }
        }

        public string FocusGroupOnEnable { set { focusGroupOnEnable = value; } }
        #endregion

        #region Unity Callbacks
        protected override void Start()
        {
            base.Start();

            if (useNamePageForFocusGroup)
            {
                var focusLayersElement = gameObject.GetChildrenInHierarchy<FocusLayerElement>();
                foreach (var element in focusLayersElement)
                {
                    if (element != null)
                    {
                        element.FocusGroup = gameObject.name;
                    }
                }
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            FocusManager.Clear(gameObject.name);
        }

        protected override void OnEnableAfterStart()
        {
            if (useHistoricalFocus && FocusManager.IsThereHistory(gameObject.name))
            {
                string focusGroup = useAnotherFocusGroup ? focusGroupOnEnable : gameObject.name;
                FocusManager.SetFocus(focusGroup, true, true, transform);
            }
            else if (useInitFocus)
            {
                FocusManager.SetFocus(firstObjectFocusable, true);
            }
        }
        #endregion

        #region Static
        public static void ChangeLangauge(string newLanguage)
        {
            ChangeLangauge(EnumExtend.ParseEnum<SystemLanguage>(newLanguage));
        }

        public static void ChangeLangauge(SystemLanguage newLanguage)
        {
#if LOCALIZATION
            LocalizationManager.Language = newLanguage;
#endif
        }

        public void Quit()
        {
            GameManager.Quit();
        }

        public void RepeatScene()
        {
            SceneSystemManager.RepeasScene();
        }

        public void ChangePage()
        {
            ChangePage(null);
        }

        public void ChangePage(string label)
        {
            if (string.IsNullOrEmpty(label))
            {
                GameManager.CheckTransitions(true);
            }
            else
            {
                GameManager.CheckTransitionsAfterChangeLabel(label);
            }
        }
        #endregion

        #region Public Methods
        public virtual void RefreshPage()
        {

        }

        #region Optionals
        public virtual void SaveOption()
        {
            GameManager.SaveOption();
        }

        public virtual void Pause(bool pause)
        {
            LevelManager.CallPause(pause);
        }
        #endregion

        #region FSM
        public virtual void Back()
        {
            FocusManager.Clear(gameObject.name);

            FocusManager.SetFocusNull();
            ChangePage(labelForBack);
        }
        #endregion

        #endregion
    }
}