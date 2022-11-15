using UnityEngine;

namespace Pearl.AI
{
    public abstract class AbstractAction<T> : MonoBehaviour
    {
        public abstract void Execute(T parmeter);

        public abstract void Stop();
    }

    public abstract class AbstractAction : MonoBehaviour
    {
        public abstract void Execute();

        public abstract void Stop();
    }
}