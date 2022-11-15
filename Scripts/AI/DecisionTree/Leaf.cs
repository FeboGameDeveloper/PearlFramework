namespace Pearl.AI.DecisionTree
{
    public class Leaf : DecisionTreeNode
    {
        #region Private Fields
        private AIAction action;
        #endregion

        #region Constructors
        public Leaf(AIAction action)
        {
            this.action = action;
        }
        #endregion

        #region Public Methods
        public override DecisionTreeNode Execute()
        {
            if (action != null)
            {
                action.Execute();
            }
            return null;
        }
        #endregion
    }
}
