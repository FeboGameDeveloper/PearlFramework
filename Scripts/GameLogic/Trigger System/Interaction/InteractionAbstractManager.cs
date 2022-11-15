using UnityEngine;

namespace Pearl
{
    public abstract class InteractionAbstractManager : MonoBehaviour
    {
        [SerializeField]
        protected Transform originRay = null;

        [SerializeField]
        protected Vector3 halfEextens = Vector3.one;

        [SerializeField]
        protected float maxDistance = 1;

        [SerializeField]
        protected LayerMask layerMask = 1;

        public abstract void Interact(string comand);

        protected void Interact(in GameObject obj, in string comand)
        {
            if (obj.TryGetComponent<InteractEvents>(out var interactScript))
            {
                interactScript.Interact(comand);
            }
        }
    }
}
