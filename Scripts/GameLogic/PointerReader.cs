using Pearl.Events;
using UnityEngine;

namespace Pearl
{
    [DisallowMultipleComponent]
    public class PointerReader : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        private bool isBlock = false;
        [SerializeField]
        private int priority = 0;
        [SerializeField]
        private bool useDoubleClick = false;
        [SerializeField]
        private bool interrupt = false;

        [SerializeField]
        private TrackingSimpleEvent PressEvent;
        [SerializeField]
        private TrackingSimpleEvent DetachEvent;
        [SerializeField]
        private TrackingSimpleEvent EnterEvent;
        [SerializeField]
        private TrackingSimpleEvent ExitEvent;

        #endregion

        #region Property

        public bool Block { get { return isBlock; } }
        public int Priority { get { return priority; } }
        public bool UseDoubleClick { get { return useDoubleClick; } }
        public bool Interrupt { get { return interrupt; } }

        #endregion

        #region Public Methods

        public void OnPointerEnter()
        {
            EnterEvent?.Invoke();
        }

        public void OnPointerExit()
        {
            ExitEvent?.Invoke();
        }

        public void OnClickPress()
        {
            PressEvent?.Invoke();
        }

        public void OnClickDetach()
        {
            DetachEvent?.Invoke();
        }

        #endregion
    }
}
