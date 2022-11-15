using UnityEngine;

namespace Pearl.AI.DecisionTree
{
    public class TestAgent : MonoBehaviour
    {
        private TestTree tree;

        public int Age { set { tree.Age = value; } }

        public int Beauty { set { tree.Beauty = value; } }

        // Start is called before the first frame update --
        void Start()
        {
            tree = new TestTree();
        }
    }
}
