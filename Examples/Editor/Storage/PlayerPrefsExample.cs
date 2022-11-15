using Pearl.Storage;
using UnityEngine;

namespace Pearl.Examples.Storage
{
    public class PlayerPrefsExample : MonoBehaviour, IStoragePlayerPrefs
    {
        private int a = 1;

        public int Test
        {
            get
            {
                return a;
            }
            set
            {
                a = value;
            }
        }

        [StoragePlayerPrefs()]
        public string b;


        // Start is called before the first frame update
        void Awake()
        {
            StoragePlayerPrefs.Load(this);
        }
    }

}