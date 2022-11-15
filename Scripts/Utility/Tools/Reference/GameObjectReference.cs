using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class GameObjectReference : AbstractReference<GameObject>
    {
        public GameObject @GameObject
        {
            get
            {
                if (!find)
                {
                    return reference;
                }
                else if (reference == null || !_init)
                {
                    reference = FindObj();
                }

                return reference;
            }
            set
            {
                reference = value;
            }
        }
    }
}

