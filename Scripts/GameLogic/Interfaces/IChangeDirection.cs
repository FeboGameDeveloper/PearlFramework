using System;
using UnityEngine;

namespace Pearl
{
    public interface IChangeDirection
    {
        event Action<Vector3> OnChangeDirection;
    }
}
