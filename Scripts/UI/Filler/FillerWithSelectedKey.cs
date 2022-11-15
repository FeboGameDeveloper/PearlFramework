namespace Pearl.UI
{
    public abstract class FillerWithSelectedKey<F, Z> : Filler<F>
    {
        protected Z selectedKey;

        public FillerWithSelectedKey(Z selectedKey)
        {
            this.selectedKey = selectedKey;
        }
    }
}
