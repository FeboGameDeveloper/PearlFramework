using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl.Events
{
    [Serializable]
    public class SimpleEvent : UnityEvent { }

    [Serializable]
    public class StringEvent : UnityEvent<string> { }

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    [Serializable]
    public class IntEvent : UnityEvent<int> { }

    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    [Serializable]
    public class VectorEvent : UnityEvent<Vector2> { }


    [Serializable]
    public class TrackingSimpleEvent : TrackingUnityEvent { }

    [Serializable]
    public class TrackingStringEvent : TrackingUnityEvent<string> { }

    [Serializable]
    public class TrackingGameObjectEvent : TrackingUnityEvent<GameObject> { }

    [Serializable]
    public class TrackingFloatEvent : TrackingUnityEvent<float> { }

    [Serializable]
    public class TrackingIntEvent : TrackingUnityEvent<int> { }

    [Serializable]
    public class TrackingBoolEvent : TrackingUnityEvent<bool> { }

    [Serializable]
    public class TrackingVectorEvent : TrackingUnityEvent<Vector2> { }
}
