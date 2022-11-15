using Pearl.Pools;
using UnityEngine;

namespace Pearl.Examples.PoolExample
{
    public class TestPools : MonoBehaviour
    {
        public GameObject prefab1;
        public GameObject prefab2;
        public Transform parent;
        public int numberObjects = 30;


        // Start is called before the first frame update
        void Start()
        {
            for (int i = 0; i < numberObjects; i++)
            {
                PoolManager.Instantiate(out _, prefab1, parent);
                PoolManager.Instantiate(out _, prefab2, parent);
            }

            Invoke("Remove", 5);
        }

        private void Remove()
        {
            PoolManager.RemovePool(prefab1);
        }
    }
}
