using System;
using UnityEngine;

namespace Pearl
{
    [DisallowMultipleComponent]
    public sealed class PearlObject : MonoBehaviour
    {
        #region Inspector Fields
        [Header("Don't destroy load")]
        /// <summary>
        //  If the Boolean is true, the GameObject will be not destroy at load
        /// </summary>
        [SerializeField]
        private bool dontDestroyLoad = false;

        /// <summary>
        //  If the Boolean is true, the GameObject will be unique and each of its clone will be eliminated in the scene
        /// </summary>
        [ConditionalField("@dontDestroyLoad")]
        [SerializeField]
        private bool isUnique = false;
        #endregion

        #region Event
        public event Action DestroyHandler;
        public event Action<GameObject> DisactiveHandler;
        #endregion

        #region Unity Callbacks
        private void Awake()
        {
            if (isUnique && AmIClone())
            {
                GameObject.DestroyImmediate(gameObject);
                return;
            }

            if (dontDestroyLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            DestroyHandler?.Invoke();
        }

        private void OnDisable()
        {
            DisactiveHandler?.Invoke(gameObject);
        }
        #endregion

        #region Private
        private bool AmIClone()
        {
            PearlObject[] objs = GameObject.FindObjectsOfType<PearlObject>();

            if (objs == null)
            {
                return true;
            }

            objs = Array.FindAll(objs, x => x.name == name);
            return objs == null || objs.Length > 1;
        }
        #endregion
    }
}
