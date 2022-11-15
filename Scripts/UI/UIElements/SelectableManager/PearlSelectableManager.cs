namespace Pearl.UI
{
    public class PearlSelectableManager : SelectableManagerNative
    {
        #region UnityCallbacks
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();

            if (selectable != null && selectable is IPearlSelectable pearlSelectable)
            {
                pearlSelectable.OnSelected += OnSelect;
                pearlSelectable.OnDeselected += OnDeselect;
                pearlSelectable.OnHighlighted += OnHighlight;
                pearlSelectable.OnNotHighlighted += OnNotHighlight;
                pearlSelectable.OnPressed += OnPress;
                pearlSelectable.OnUp += OnUp;
            }
        }

        protected void OnDestroy()
        {
            if (selectable != null && selectable is IPearlSelectable pearlSelectable)
            {
                pearlSelectable.OnSelected -= OnSelect;
                pearlSelectable.OnDeselected -= OnDeselect;
                pearlSelectable.OnHighlighted -= OnHighlight;
                pearlSelectable.OnNotHighlighted -= OnNotHighlight;
                pearlSelectable.OnPressed -= OnPress;
                pearlSelectable.OnUp -= OnUp;
            }
        }
        #endregion
    }
}