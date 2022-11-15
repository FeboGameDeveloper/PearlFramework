using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class ComponentReference<Reference> : AbstractReference<Reference> where Reference : Component
    {
        public Reference @Component
        {
            get
            {
                if (!find)
                {
                    return reference;
                }
                else if (reference.IsNull() || !_init)
                {
                    var obj = FindObj();
                    reference = obj != null ? obj.GetComponent<Reference>() : default;
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