using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Storage
{
    public class StorageComponent : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private bool initLoadVars = false;

        [SerializeField]
        private bool encrypted = false;

        [SerializeField]
        private string id = string.Empty;

        [SerializeField]
        private List<PearlBehaviour> componentsForStorage = null;
        #endregion

        #region Unity Callbacks
        protected void Awake()
        {
            if (initLoadVars)
            {
                LoadAll(StorageTypeEnum.Light);
            }
        }

        protected void OnDestroy()
        {
            if (initLoadVars)
            {
                foreach (var component in componentsForStorage)
                {
                    StorageManager.Remove(component, true, id, encrypted);
                }
            }
        }
        #endregion

        #region Public Methods
        public void SaveAll(StorageTypeEnum typeStorage = StorageTypeEnum.Light)
        {
            foreach (var component in componentsForStorage)
            {
                StorageManager.Save(component, typeStorage, id, encrypted);
            }
        }

        public void LoadAll(StorageTypeEnum typeStorage = StorageTypeEnum.Light)
        {
            foreach (var component in componentsForStorage)
            {
                StorageManager.Load(component, typeStorage, id, encrypted);
            }
        }
        #endregion
    }
}