using System;
using UnityEngine;

namespace Pearl
{
    public class RayInterface2D : RayAbstractInterface<Collider2D>
    {
        protected override void EveryFrame()
        {
            var rayCasts = Physics2D.BoxCastAll(originRay.position, halfEextens * 2, 0, originRay.forward, maxDistance, layerMask);
            foreach (var ray in rayCasts)
            {
                OnEnter(ray.collider, ray.collider.gameObject);
            }


            for (int i = _activeObjs.Count - 1; i >= 0; i--)
            {
                var active = _activeObjs[i];
                if (!Array.Exists(rayCasts, (RaycastHit2D r) => r.collider == active.Item1))
                {
                    OnExit(active.Item1, active.Item2);
                }
            }
        }
    }
}
