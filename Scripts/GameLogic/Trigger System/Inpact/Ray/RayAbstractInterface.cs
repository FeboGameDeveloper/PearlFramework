using UnityEngine;

namespace Pearl
{
    public abstract class RayAbstractInterface<T> : InpactAbstractInterface<T>
    {
        [SerializeField]
        protected Transform originRay = null;

        [SerializeField]
        protected Vector3 halfEextens = Vector3.one;

        [SerializeField]
        protected float maxDistance = 1;

        [SerializeField]
        protected LayerMask layerMask = 1;


        // Update is called once per frame
        protected virtual void Update()
        {
            if (forFastObjects)
            {
                EveryFrame();
            }
        }

        protected void FixedUpdate()
        {
            if (!forFastObjects)
            {
                EveryFrame();
            }
        }

        protected abstract void EveryFrame();
    }
}
