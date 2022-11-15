using System;
using System.Collections.Generic;


namespace Pearl.AI.DecisionTree
{
    public abstract class DecisionTree
    {
        #region Private Fields
        private readonly Dictionary<string, Node> nodes;
        private readonly Dictionary<string, Leaf> leaves;
        private readonly DecisionTreeNode root;
        #endregion

        #region Aux Fields
        private DecisionTreeNode nextNode;
        #endregion

        #region Constructors
        public DecisionTree()
        {
            nodes = new Dictionary<string, Node>();
            leaves = new Dictionary<string, Leaf>();
            root = CreateRoot(ReturnFunctionRoot());
            CreateNodes(ReturnFunctionsNodes());
            CreateLeaves(ReturnActionsLeaves());
            Execute();
        }
        #endregion

        #region Public Methods
        public void Execute()
        {
            if (root == null)
            {
                return;
            }

            nextNode = root.Execute();
            while (nextNode != null)
            {
                nextNode = nextNode.Execute();
            }
        }
        #endregion

        #region Protected Methods
        protected DecisionTreeNode ReturnNode(in string name)
        {
            if (name != null && nodes != null && nodes.TryGetValue(name, out Node node))
            {
                return node;
            }
            return null;
        }

        protected DecisionTreeNode ReturnLeaves(in string name)
        {
            if (name != null && nodes != null && leaves.TryGetValue(name, out Leaf leaf))
            {
                return leaf;
            }
            return null;
        }
        #endregion

        #region Private Methods
        private DecisionTreeNode CreateRoot(Func<DecisionTreeNode> function)
        {
            return new Node(function);
        }

        private void CreateNodes(params Func<DecisionTreeNode>[] funcs)
        {
            if (nodes != null && funcs != null)
            {
                for (int i = 0; i < funcs.Length; i++)
                {
                    var func = funcs[i];
                    if (func != null)
                    {
                        nodes.Add(func.Method.Name, new Node(func));
                    }
                }
            }
        }

        private void CreateLeaves(params Func<AIAction>[] actions)
        {
            if (nodes != null && actions != null)
            {
                for (int i = 0; i < actions.Length; i++)
                {
                    var action = actions[i];

                    if (action != null)
                    {
                        leaves.Add(action.Method.Name, new Leaf((AIAction)action.DynamicInvoke()));
                    }
                }
            }
        }
        #endregion

        #region Abstract Methods
        protected abstract Func<DecisionTreeNode>[] ReturnFunctionsNodes();

        protected abstract Func<AIAction>[] ReturnActionsLeaves();

        protected abstract Func<DecisionTreeNode> ReturnFunctionRoot();
        #endregion
    }
}
