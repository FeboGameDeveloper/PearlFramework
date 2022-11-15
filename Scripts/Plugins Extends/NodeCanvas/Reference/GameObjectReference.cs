#if NODE_CANVAS

using UnityEngine;

namespace Pearl.NodeCanvas
{
    public class GameObjectReference : AbstractReference<GameObject>
    {
        public GameObject @GameObject
        {
            get
            {
                if (find == null || reference == null)
                {
                    return null;
                }


                if (!find.value)
                {
                    return reference.value;
                }
                else if (reference.value == null || !_init)
                {
                    reference.value = FindObj();
                }

                return reference.value;
            }
            set
            {
                reference = value;
            }
        }
    }
}
#endif
