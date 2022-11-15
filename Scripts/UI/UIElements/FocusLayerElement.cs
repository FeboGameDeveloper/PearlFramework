using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    [DisallowMultipleComponent]
    public class FocusLayerElement : PearlBehaviour
    {
        #region Inspector fields
        [SerializeField]
        private string focusGroup = null;
        [SerializeField]
        private int layerFocus = 0;
        [SerializeField]
        private bool setLayerFocusOnEnable = false;
        [SerializeField]
        private bool deleteStorageGroupAtDestroy = false;

        [SerializeField]
        private bool clickForNewFocus = false;
        [SerializeField, ConditionalField("@clickForNewFocus")]
        private bool useFocusGroup = false;
        [SerializeField, ConditionalField("@clickForNewFocus && !@useFocusGroup")]
        private GameObject newFocusOnClick = null;
        [SerializeField, ConditionalField("@clickForNewFocus && @useFocusGroup")]
        private string newFocusGroup = null;
        #endregion

        #region Property
        public string FocusGroup { get { return focusGroup; } set { focusGroup = value; } }
        #endregion

        #region UnityCallbacks
        protected override void OnEnable()
        {
            FocusManager.OnChangeLayer += OnChangeLayer;

            if (gameObject.TryGetComponent<IPearlSelectable>(out var component))
            {
                component.OnSelected += OnSelected;
            }

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (clickForNewFocus && gameObject.TryGetComponent<Button>(out var button))
            {
                button.onClick.RemoveListener(SetNewFocus);
            }

            if (gameObject.TryGetComponent<IPearlSelectable>(out var component))
            {
                component.OnSelected -= OnSelected;
            }

            FocusManager.OnChangeLayer -= OnChangeLayer;
            PearlInvoke.StopTimer<int>(OnChangeLayer);
        }

        protected override void Start()
        {
            base.Start();

            PearlInvoke.WaitForMethod<int>(FocusManager.waitForInitFocus, OnChangeLayer, FocusManager.CurrentLayerFocus, TimeType.Unscaled);
        }

        protected override void OnEnableAfterStart()
        {
            base.OnEnableAfterStart();

            if (clickForNewFocus)
            {
                if (gameObject.TryGetComponent<Button>(out var button))
                {
                    button.onClick.AddListener(SetNewFocus);
                }
            }

            if (setLayerFocusOnEnable)
            {
                ChangeLayerInFocusManager();
            }
            else
            {
                OnChangeLayer(FocusManager.CurrentLayerFocus);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (deleteStorageGroupAtDestroy)
            {
                FocusManager.Clear(focusGroup);
            }
        }
        #endregion

        #region Public Methods
        public bool IsRightLayer()
        {
            return FocusManager.CurrentLayerFocus == layerFocus;
        }

        public void SetLayerFocus(in int newLayerFocus)
        {
            SetLayerFocus(newLayerFocus, false);
        }

        public void SetLayerFocus(in int newLayerFocus, in bool forceFocusManager)
        {
            this.layerFocus = newLayerFocus;
            if (forceFocusManager)
            {
                ChangeLayerInFocusManager();
            }
        }

        public void SetUpLayerFocus(in bool forceFocusManager)
        {
            int newLayerFocus = FocusManager.CurrentLayerFocus + 1;
            SetLayerFocus(newLayerFocus, forceFocusManager);
        }

        public void ChangeLayerInFocusManager()
        {
            bool isRightLayer = IsRightLayer();
            if (!isRightLayer)
            {
                FocusManager.ChangeLayer(layerFocus);
            }
        }
        #endregion

        #region Private Methods
        private void SetNewFocus()
        {
            if (clickForNewFocus)
            {
                if (useFocusGroup)
                {
                    FocusManager.SetFocus(newFocusGroup, true, false);
                }
                else
                {
                    FocusManager.SetFocus(newFocusOnClick, true, false);
                }
            }
        }

        private void OnSelected()
        {
            FocusManager.Save();
        }

        private void OnChangeLayer(int currentLayerFocus)
        {
            bool isRightLayer = currentLayerFocus == layerFocus;

            IFocusLayer[] components = gameObject.GetComponents<IFocusLayer>();
            if (components != null)
            {
                foreach (var component in components)
                {
                    if (component != null)
                    {
                        component.SetFocusLayer(isRightLayer);
                    }
                }
            }
        }
        #endregion
    }
}
