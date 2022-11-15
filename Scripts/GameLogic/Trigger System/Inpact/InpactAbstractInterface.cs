using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public abstract class InpactAbstractInterface<T> : MonoBehaviour
    {
        [SerializeField]
        private ColliderResponseManager colliderResponseManager = null;

        [SerializeField]
        protected bool forFastObjects = false;

        [SerializeField]
        protected bool disable = false;

        protected List<Tuple<T, GameObject>> _activeObjs = new List<Tuple<T, GameObject>>();

        protected virtual void Reset()
        {
            colliderResponseManager = GetComponent<ColliderResponseManager>();
        }

        protected virtual void Awake()
        {
            if (colliderResponseManager)
            {
                colliderResponseManager.OnDisableResponse += ForceExit;
            }
        }

        private void OnDisable()
        {
            ForceExit();
        }

        protected void OnEnter(T element, GameObject obj)
        {
            if (!disable && element != null && _activeObjs != null)
            {
                if (CollisionEvent(obj, TriggerType.Enter))
                {
                    _activeObjs.Add(new Tuple<T, GameObject>(element, obj));
                }
                else
                {
                    _activeObjs.Remove(new Tuple<T, GameObject>(element, obj));
                }
            }
        }

        protected void OnExit(T element, GameObject obj)
        {
            if (!disable && CollisionEvent(obj, TriggerType.Exit))
            {
                _activeObjs.Remove(new Tuple<T, GameObject>(element, obj));
            }
        }


        private bool CollisionEvent(GameObject obj, TriggerType type)
        {
            if (colliderResponseManager)
            {
                if (type == TriggerType.Enter)
                {
                    return colliderResponseManager.Inpact(obj);
                }
                else
                {
                    return colliderResponseManager.ExitInpact(obj);
                }
            }
            return false;
        }

        private void ForceExit()
        {
            foreach (var obj in _activeObjs)
            {
                colliderResponseManager.ExitInpact(obj.Item2, true);
            }

            _activeObjs.Clear();
        }
    }
}
