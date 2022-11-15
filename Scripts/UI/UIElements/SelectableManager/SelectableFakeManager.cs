namespace Pearl.UI
{
    public class SelectableFakeManager : SelectableManagerNative
    {
        #region Public Methods
        public void SetHighlight()
        {
            OnHighlight();
        }

        public void SetPress()
        {
            OnPress();
        }

        public void SetNotHighlight()
        {
            OnNotHighlight();
        }

        public void SetSelect()
        {
            OnSelect();
        }

        public void SetDeselect()
        {
            OnDeselect();
        }

        public void SetPointerUp()
        {
            OnUp();
        }
        #endregion
    }
}