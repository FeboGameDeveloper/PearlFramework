using Pearl.Testing;
using Pearl.Multitags;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Pools
{
    /// <summary>
    /// This class manages every pool of each prefab
    /// </summary>
    public class PoolManager : MonoBehaviour
    {
        #region static

        #region public Fields
        public const string poolElement = "elementPool";
        #endregion

        #region Private Fields
        /// <summary>
        /// This list contains all the pools
        /// </summary>
        private readonly static Dictionary<string, PoolManager> listPool = new();
        private const string poolContainerName = "poolContainer";
        private const string poolContainerStaticName = "staticPoolContainer";
        #endregion

        #region Public Methods

        #region Instantiate
        /// <summary>
        /// Set specific Pool for obj
        /// </summary>
        /// <param name = "prefab"> The prefab that will contain the pool</param>
        /// <param name = "max"> The maximum number of the components of the pool</param>
        public static PoolManager InstantiatePool(GameObject prefab, bool useMax, uint max = 10, bool dontDestoryAtLoad = false)
        {
            if (prefab != null && listPool != null)
            {
                if (listPool.ContainsKey(prefab.name))
                {
                    return null;
                }

                GameObject container = FindContainer(dontDestoryAtLoad);
                PoolManager pool = GameObjectExtend.CreateGameObject<PoolManager>(prefab.name + "Pool", container.transform);

                if (pool == null)
                {
                    return null;
                }

                pool.Set(prefab, max, useMax);

                if (container != null)
                {
                    listPool.Add(prefab.name, pool);
                    return pool;
                }
            }
            return null;
        }

        public static bool Instantiate<T>(out T result, GameObject prefab, Transform parent = null, bool worldPositionStays = false) where T : Component
        {
            return Instantiate<T>(out result, prefab, Vector3.zero, Quaternion.identity, parent, worldPositionStays);
        }

        public static bool Instantiate<T>(out T result, GameObject prefab, Vector3 position, Transform parent = null, bool worldPositionStays = false) where T : Component
        {
            return Instantiate<T>(out result, prefab, position, Quaternion.identity, parent, worldPositionStays);
        }

        public static bool Instantiate<T>(out T result, GameObject prefab, Vector3 position, Quaternion quat, Transform parent = null, bool worldPositionStays = false) where T : Component
        {
            result = Instantiate(out var aux, prefab, position, quat, parent, worldPositionStays) ? aux.GetComponent<T>() : null;
            return result != null;
        }

        public static bool Instantiate(out GameObject result, GameObject prefab, Transform parent = null, bool worldPositionStays = false)
        {
            return Instantiate(out result, prefab, Vector3.zero, Quaternion.identity, parent, worldPositionStays);
        }

        /// <summary>
        /// Recycles a element for specific Pool
        /// </summary>
        /// <param name = "prefab"> The prefab for pool</param>
        /// <param name = "position"> The position where the prefab will spawn</param>
        /// <param name = "rotation"> The rotation where the prefab will spawn</param>
        /// <param name = "parent"> The parent of new object</param>
        public static bool Instantiate(out GameObject result, GameObject prefab, Vector3 position, Quaternion quat, Transform parent = null, bool worldPositionStays = false)
        {
            result = null;

            if (prefab == null || listPool == null)
            {
                return false;
            }

            if (!listPool.TryGetValue(prefab.name, out PoolManager pool))
            {
                pool = InstantiatePool(prefab, false);
            }

            if (pool != null)
            {
                result = pool.InstantiateObject(position, quat, parent, worldPositionStays);
            }
            return result != null;
        }
        #endregion

        #region Remove
        /// <summary>
        /// Destroy/Disable for specific Pool
        /// </summary>
        /// <param name = "obj">The specific pool that will be destroyed</param>
        public static void Remove(GameObject obj)
        {
            if (obj != null && listPool != null && listPool.TryGetValue(obj.name, out PoolManager pool))
            {
                pool.Disable(obj);
            }
        }

        /// <summary>
        /// Remove specific Pool for obj
        /// </summary>
        /// <param name = "prefab"> The prefab that contain the pool</param>
        public static void RemovePool(GameObject prefab)
        {
            if (prefab != null && listPool != null && listPool.TryGetValue(prefab.name, out PoolManager pool))
            {
                pool.AllDisable(true);
                GameObject.Destroy(pool.gameObject);
                listPool.Remove(prefab.name);
            }
        }

        /// <summary>
        /// Remove all Pools
        /// </summary>
        public static void RemoveAllPool()
        {
            if (listPool == null)
            {
                return;
            }

            for (int i = listPool.Count - 1; i >= 0; i--)
            {
                PoolManager pool = listPool.Values.Get(i);
                RemovePool(pool.Prefab);
            }

        }
        #endregion

        #endregion

        #region Private Methods
        private static GameObject FindContainer(bool dontDestoryAtLoad = false)
        {
            string namePool = dontDestoryAtLoad ? poolContainerStaticName : poolContainerName;
            GameObject container = MultiTagsManager.FindGameObjectWithMultiTags(true, namePool);
            if (!container)
            {
                container = new(namePool, typeof(MultitagsComponent));
                if (container)
                {
                    if (dontDestoryAtLoad)
                    {
                        GameObject.DontDestroyOnLoad(container);
                    }
                    container.AddTags(namePool);
                }
            }

            return container;
        }
        #endregion

        #endregion

        #region noStatic

        #region Private Fields
        /// <summary>
        /// The list contains the disable components
        /// </summary>
        private readonly List<GameObject> listDisable = new();
        /// <summary>
        /// The list contains the enable components
        /// </summary>
        private readonly List<GameObject> listAble = new();
        /// <summary>
        /// The prefab component of the pool
        /// </summary>
        private GameObject _prefab;
        /// <summary>
        /// The max number of object of that prefab
        /// </summary>
        private uint max;
        private bool _useMax = false;
        #endregion

        #region Auxiliar Fields
        /// <summary>
        /// An auxiliary gameobject 
        /// </summary>
        private GameObject auxGameobject;
        #endregion

        #region Properties
        public GameObject Prefab { get { return _prefab; } }
        #endregion

        #region UnityCallbacks
        protected void OnDestroy()
        {
            PoolManager.RemovePool(Prefab);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Setting variables and create pool
        /// </summary>
        /// <param name = "pool">The Gameobject that contains all specific components on scene</param>
        /// <param name = "prefab"> The object that represents the prefab</param>
        /// <param name = "max"> The maximum number of the components of the pool</param>
        private void Set(GameObject prefab, uint max, bool useMax = false)
        {
            this.max = max;
            this._useMax = useMax;
            this._prefab = prefab;

            if (prefab != null)
            {
                for (int i = 0; i < this.max; i++)
                {
                    IstantiateNewObject();
                }
            }
        }

        /// <summary>
        /// Disable all GameObjects
        /// </summary>
        private void AllDisable(bool containerDestroyed = false)
        {
            if (listAble == null)
            {
                return;
            }

            for (int i = listAble.Count - 1; i >= 0; i--)
            {
                Disable(listAble[i], containerDestroyed);
            }
        }

        /// <summary>
        /// Create or Recycle GameObject
        /// </summary>
        /// <param name = "position"> The position where the prefab will spawn</param>
        /// <param name = "rotation"> The rotation where the prefab will spawn</param>
        /// <param name = "parent"> The parent of new object</param>
        private GameObject InstantiateObject(Vector3 position, Quaternion rotation, Transform parent = null, bool worldPositionStays = false)
        {
            long aux = listAble.Count + listDisable.Count;
            if (listAble != null && listAble.Count == aux && !_useMax)
            {
                IstantiateNewObject();
                aux -= this.max;
                LogManager.Log("There are " + aux + " more " + _prefab.name + " elements");
            }

            return RecycleObject(position, rotation, parent, worldPositionStays);
        }


        /// <summary>
        /// Disable GameObject
        /// </summary>
        /// <param name = "obj"> The gameobject that must be disabled</param>
        private void Disable(GameObject obj, bool containerDestroyed = false)
        {
            if (obj != null && listDisable != null && listAble != null)
            {
                if (!containerDestroyed)
                {
                    obj.transform.SetParent(gameObject.transform, false);
                }

                obj.SetActive(false);
                listDisable.Add(obj);
                listAble.Remove(obj);


                foreach (Component component in obj.GetComponents(typeof(Component)))
                {
                    if (component is IReset reset)
                    {
                        reset.DisableElement();
                    }
                }

            }
        }

        /// <summary>
        /// Recycle GameObject
        /// </summary>
        /// <param name = "position"> The position where the prefab will spawn</param>
        /// <param name = "rotation"> The rotation where the prefab will spawn</param>
        private GameObject RecycleObject(Vector3 pos, Quaternion rotation, Transform parent = null, bool worldPositionStays = false)
        {
            if (listDisable != null && listAble != null)
            {
                if (listDisable.Count <= 0 && listAble.Count > 0)
                {
                    Disable(listAble[0]);
                }

                if (listDisable.Count > 0)
                {
                    auxGameobject = listDisable[0];
                    auxGameobject = Enable(auxGameobject, pos, rotation, parent, worldPositionStays);

                    auxGameobject.SetActive(true);
                    listDisable.Remove(auxGameobject);
                    listAble.Add(auxGameobject);

                    return auxGameobject;
                }
            }
            return null;
        }

        /// <summary>
        /// Sect attributes for the recycled object
        /// </summary>
        /// <param name = "aux"> The recycled object</param>
        /// <param name = "position"> The position where the prefab will spawn</param>
        /// <param name = "rotation"> The rotation where the prefab will spawn</param>
        private GameObject Enable(GameObject aux, Vector3 position, Quaternion quat, Transform parent = null, bool worldPositionStays = false)
        {
            if (aux != null)
            {
                foreach (Component component in aux.GetComponents(typeof(Component)))
                {
                    if (component is IReset reset)
                    {
                        reset.ResetElement();
                    }
                }

                aux.transform.SetPositionAndRotation(position, quat);

                if (parent != null)
                {
                    aux.transform.SetParent(parent, worldPositionStays);
                }

                if (_prefab)
                {
                    aux.transform.localScale = _prefab.transform.localScale;
                }

            }
            return aux;
        }

        private void IstantiateNewObject()
        {
            GameObjectExtend.CreateGameObject(_prefab, out auxGameobject, _prefab.name, Vector3.zero, Quaternion.identity, gameObject.transform);
            auxGameobject.AddTags(PoolManager.poolElement);
            Disable(auxGameobject);
        }
        #endregion

        #endregion
    }
}


