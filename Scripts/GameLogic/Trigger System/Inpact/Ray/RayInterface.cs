using Pearl;
using System;
using UnityEngine;

public class RayInterface : RayAbstractInterface<Collider>
{
    protected override void EveryFrame()
    {
        var rayCasts = Physics.BoxCastAll(originRay.position, halfEextens, originRay.forward, Quaternion.identity, maxDistance, layerMask);
        foreach (var ray in rayCasts)
        {
            OnEnter(ray.collider, ray.collider.gameObject);
        }


        for (int i = _activeObjs.Count - 1; i >= 0; i--)
        {
            var active = _activeObjs[i];
            if (!Array.Exists(rayCasts, (RaycastHit r) => r.collider == active.Item1))
            {
                OnExit(active.Item1, active.Item2);
            }
        }
    }
}
