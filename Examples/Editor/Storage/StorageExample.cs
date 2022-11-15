using Pearl.Storage;
using System;
using UnityEngine;

namespace Pearl.Examples.Storage
{
    [Serializable]
    public class ToStorage
    {
        public string s;
        public int n;
        public Vector3Storage v;
        public Axis3DEnum axe;
    }

    public class StorageExample : MonoBehaviour
    {

        [Storage("Test")]
        public ToStorage to;

        public string prefix = "salvo";
        public StorageTypeEnum typeStorage = StorageTypeEnum.Light;

        [InspectorButton("Save")]
        public bool save;

        [InspectorButton("Load")]
        public bool load;

        private void OnDestroy()
        {
            StorageManager.Remove(this, typeStorage != StorageTypeEnum.Hard, prefix, false);
        }

        public void Save()
        {
            StorageManager.Save(this, typeStorage, prefix, false);
        }

        public void Load()
        {
            StorageManager.Load(this, typeStorage, prefix, false);
        }
    }
}
