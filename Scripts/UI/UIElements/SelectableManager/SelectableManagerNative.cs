using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pearl.UI
{
    public enum SelectableState { Highlighted, Pressed, Selected, Deselected, }

    public abstract class SelectableManagerNative : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        protected Selectable selectable = null;
        [SerializeField]
        protected bool isFocusWhenHighlighted = true;

        [Header("FSM Animations")]
        [SerializeField]
        protected bool useFSM = true;
        [SerializeField, ConditionalField("@useFSM"), InterfaceType(typeof(IFSM))]
        protected Component FSMObject = null;
        [SerializeField]
        [ConditionalField("@useFSM")]
        protected string selectableLabel = "selectable";
        [SerializeField]
        [ConditionalField("@useFSM")]
        protected string deselectableLabel = "deselectable";
        [SerializeField]
        [ConditionalField("@useFSM")]
        protected string disableLabel = "disable";
        [SerializeField]
        [ConditionalField("@useFSM")]
        protected string enableLabel = "enable";
        #endregion

        #region Events
        public UnityEvent OnSelected;
        public UnityEvent OnDeselected;
        public UnityEvent OnHighlighted;
        public UnityEvent OnNotHighlighted;
        public UnityEvent OnPressed;
        public UnityEvent OnPointerUp;
        #endregion

        #region Private fields
        private SelectableState selectableState = SelectableState.Deselected;
        private IFSM FSM;
        #endregion

        #region Property
        public SelectableState SelectableState { get { return selectableState; } }
        #endregion

        #region Unity Callbacks
        protected virtual void Awake()
        {
            if (FSMObject != null)
            {
                FSM = (IFSM)FSMObject;
            }
        }

        protected virtual void OnEnable()
        {
            if (useFSM && FSM != null)
            {
                FSM.ChangeLabel(enableLabel);
                FSM.CheckTransitions(true);
            }
        }

        protected virtual void OnDisable()
        {
            if (selectable != null && selectable.gameObject == FocusManager.GetFocus())
            {
                FocusManager.SetFocusNull();
                OnDeselect();
            }

            if (useFSM && FSM != null)
            {
                FSM.ChangeLabel(disableLabel);
                FSM.CheckTransitions(true);
            }
        }

        protected void Reset()
        {
            selectable = GetComponent<Selectable>();
            FSMObject = (Component)GetComponent<IFSM>();
        }
        #endregion

        #region Protected Methods
        protected void OnHighlight()
        {
            if (selectable && selectable.interactable)
            {
                selectableState = SelectableState.Highlighted;
                if (isFocusWhenHighlighted && selectable)
                {
                    FocusManager.SetFocus(selectable.gameObject, true);
                }
                else
                {
                    OnHighlighted?.Invoke();
                }
            }
        }

        protected void OnNotHighlight()
        {
            if (selectable && selectable.interactable)
            {
                if (!isFocusWhenHighlighted)
                {
                    OnNotHighlighted?.Invoke();
                }
            }
        }

        protected void OnPress()
        {
            OnPressed?.Invoke();
        }

        protected void OnUp()
        {
            OnPointerUp?.Invoke();
        }

        protected void OnSelect()
        {
            if (selectable && selectable.interactable)
            {
                selectableState = SelectableState.Selected;
                OnSelected?.Invoke();

                if (useFSM && FSM != null)
                {
                    FSM.ChangeLabel(selectableLabel);
                    FSM.CheckTransitions(true);
                }
            }
        }

        protected void OnDeselect()
        {
            selectableState = SelectableState.Deselected;
            OnDeselected?.Invoke();

            if (useFSM && FSM != null)
            {
                FSM.ChangeLabel(deselectableLabel);
                FSM.CheckTransitions(true);
            }
        }
        #endregion
    }
}
