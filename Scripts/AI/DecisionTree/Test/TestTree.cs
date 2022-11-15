using System;

namespace Pearl.AI.DecisionTree
{
    public class TestTree : DecisionTree
    {
        #region Private Fields
        private int age = 10;
        private int beauty = 20;
        #endregion

        #region Properties
        public int Age
        {
            set
            {
                age = value;
                Execute();
            }
        }

        public int Beauty
        {
            set
            {
                beauty = value;
                Execute();
            }
        }
        #endregion

        #region NodeTree Functions
        private DecisionTreeNode NodeControlAge()
        {
            if (age > 50)
            {
                return ReturnNode("NodeControlBeauty");
            }
            else
            {
                return ReturnLeaves("LeafTalk");
            }
        }

        private DecisionTreeNode NodeControlBeauty()
        {
            if (beauty > 50)
            {
                return ReturnLeaves("LeafTalk");
            }
            else
            {
                return ReturnLeaves("LeafTalk");
            }
        }

        private AIAction LeafTalk()
        {
            return new Talk("Salvo");
        }
        #endregion

        #region Override Methods
        protected override Func<DecisionTreeNode> ReturnFunctionRoot()
        {
            return NodeControlAge;
        }

        protected override Func<DecisionTreeNode>[] ReturnFunctionsNodes()
        {
            return new Func<DecisionTreeNode>[] { NodeControlBeauty };
        }

        protected override Func<AIAction>[] ReturnActionsLeaves()
        {
            return new Func<AIAction>[] { LeafTalk };
        }
        #endregion 
    }
}
