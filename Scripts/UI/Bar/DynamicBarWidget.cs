using UnityEngine;

namespace Pearl.UI
{
    public class DynamicBarWidget : PearlBehaviour
    {
        #region Inspector fields
        [SerializeField]
        private RectTransform rectTransform = null;
        #endregion

        #region Privte fields
        private float initWidth;
        #endregion

        #region Unity callbacks
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (rectTransform != null)
            {
                initWidth = rectTransform.sizeDelta.x;
            }
        }
        #endregion

        #region Public Methods
        public void ResetSize()
        {
            SetSize(0);
        }

        public void SetSize(float percent)
        {
            if (rectTransform != null)
            {
                float deltaFill = MathfExtend.ChangeRange01(percent, Mathf.Abs(initWidth));
                Vector2 newOffsetMax = new Vector2(initWidth + deltaFill, rectTransform.offsetMax.y);
                rectTransform.offsetMax = newOffsetMax;
            }
        }
        #endregion
    }
}
