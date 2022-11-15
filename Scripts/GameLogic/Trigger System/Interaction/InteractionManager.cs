using UnityEngine;

namespace Pearl
{
    public class InteractionManager : InteractionAbstractManager
    {
        public override void Interact(string comand)
        {
            var rayCasts = Physics.BoxCastAll(originRay.position, halfEextens, originRay.forward, Quaternion.identity, maxDistance, layerMask);
            foreach (var ray in rayCasts)
            {
                var obj = ray.collider.gameObject;
                Interact(obj, comand);
            }
        }
    }
}
