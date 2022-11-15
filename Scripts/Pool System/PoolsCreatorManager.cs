using System;
using UnityEngine;

namespace Pearl.Pools
{
    /// <summary>
    /// This class creates all the pools in the scene
    /// </summary>
    public class PoolsCreatorManager : MonoBehaviour
    {
        /// <summary>
        /// Auxiliary class that associates a pooled prefab with the maximum number of instantiations
        /// </summary>
        [Serializable]
        public struct ElementPool
        {
            #region Inspector Fields
            /// <summary>
            /// The pooled preab
            /// </summary>
            [SerializeField]
            private GameObject obj;
            /// <summary>
            /// The maximum number of instantiations
            /// </summary>
            [SerializeField]
            private uint numberElementsInPool;

            [SerializeField]
            private bool useMax;
            #endregion

            #region Properties
            public GameObject Obj { get { return obj; } }
            public uint NumberElementsInPool { get { return numberElementsInPool; } }

            public bool UseMax { get { return useMax; } }
            #endregion

            #region Constructors
            public ElementPool(GameObject obj, uint numberElementsInPool)
            {
                this.obj = obj;
                this.numberElementsInPool = numberElementsInPool;
                this.useMax = false;
            }

            public ElementPool(GameObject obj, uint numberElementsInPool, bool useMax)
            {
                this.obj = obj;
                this.numberElementsInPool = numberElementsInPool;
                this.useMax = useMax;
            }
            #endregion
        }


        #region Inspector Fields
        /// <summary>
        /// The prefab of each pool
        /// </summary>
        [SerializeField]
        private ElementPool[] prefabs = null;
        #endregion

        #region Unity CallBacks
        protected void Awake()
        {
            CreatePool();
        }
        #endregion

        #region Private Methods
        private void CreatePool()
        {
            if (prefabs != null)
            {
                foreach (var prefab in prefabs)
                {
                    PoolManager.InstantiatePool(prefab.Obj, prefab.UseMax, prefab.NumberElementsInPool);
                }
            }
        }
        #endregion
    }
}
