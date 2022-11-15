using UnityEngine;

namespace Pearl
{
    public class InteractionManager2D : InteractionAbstractManager
    {
        public override void Interact(string comand)
        {
            var rayCasts = Physics2D.BoxCastAll(originRay.position, halfEextens * 2, 0, originRay.forward, maxDistance, layerMask);
            foreach (var ray in rayCasts)
            {
                var obj = ray.collider.gameObject;
                Interact(obj, comand);
            }
        }
    }
}