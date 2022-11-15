#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

namespace Pearl.NodeCanvas
{
    [Serializable]
    public class PositionData
    {
        public BBParameter<bool> useTransform;
        [Conditional("useTransform", 0)]
        public BBParameter<Vector3> position;
        [Conditional("useTransform", 1)]
        public BBParameter<Transform> transform;

        public Vector3 Get()
        {
            if (useTransform != null && useTransform.value && transform != null && transform.value)
            {
                return transform.value.position;
            }
            else
            {
                return position != null ? position.value : default;
            }
        }
    }
}

#endif
