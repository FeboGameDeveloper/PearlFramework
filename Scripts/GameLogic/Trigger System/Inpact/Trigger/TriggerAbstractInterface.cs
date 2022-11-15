namespace Pearl
{
    public abstract class TriggerAbstractInterface<T> : InpactAbstractInterface<T>
    {
        protected virtual void Update()
        {
            if (forFastObjects)
            {
                for (int i = _activeObjs.Count - 1; i >= 0; i--)
                {
                    if (!IsTouching(_activeObjs[i].Item1))
                    {
                        OnExit(_activeObjs[i].Item1, _activeObjs[i].Item2);
                    }
                }
            }
        }

        protected abstract bool IsTouching(T element);
    }
}
