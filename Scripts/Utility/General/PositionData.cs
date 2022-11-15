using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct PositionData
    {
        public bool useTransform;
        [ConditionalField("!@useTransform")]
        public Vector3 position;
        [ConditionalField("@useTransform")]
        public Transform transform;

        public Vector3 Get()
        {
            if (useTransform && transform)
            {
                return transform.position;
            }
            else
            {
                return position;
            }
        }
    }
}
