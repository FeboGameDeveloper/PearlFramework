namespace Pearl.Debug
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class DebugScreenAttribute : System.Attribute
    {
        private string _debugCategory;
        private string _debugName;

        public string DebugCategory { get { return _debugCategory; } }
        public string DebugName { get { return _debugName; } }

        public DebugScreenAttribute(string debugCategory, string debugName)
        {
            this._debugCategory = debugCategory;
            this._debugName = debugName;
        }
    }
}
