using System;

namespace Pearl.AI.DecisionTree
{
    public class Node : DecisionTreeNode
    {
        #region Private Fields
        private Func<DecisionTreeNode> func;
        #endregion

        #region Constructors
        public Node(Func<DecisionTreeNode> func)
        {
            this.func = func;
        }
        #endregion

        #region Public Methods
        public override DecisionTreeNode Execute()
        {
            if (func != null)
            {
                return func.Invoke();
            }
            return null;
        }
        #endregion
    }
}
