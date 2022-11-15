using Pearl.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pearl
{
    [DisallowMultipleComponent]
    public class MousePointerManager : PearlBehaviour, ISingleton
    {
        #region Inspector
        [SerializeField]
        private Camera gameCamera = null;
        [SerializeField]
        private DimensionsEnum dimension = DimensionsEnum.ThreeDimension;
        [SerializeField]
        private UpdateModes update = UpdateModes.Update;
        [SerializeField]
        private float doubleClick = 0.2f;
        #endregion

        #region Private fields
        private readonly List<PointerReader> _clickables = new();
        private readonly List<PointerReader> _trigger = new();
        private readonly List<PointerReader> _auxes = new();
        private readonly Dictionary<PointerReader, float> _doubleClicks = new();
        private readonly List<PointerReader> _pointerPressed = new();

        private InputAction _click;
        private Vector2 _position;
        #endregion

        #region Static Methods
        public static void ResetInputs()
        {
            if (Singleton<MousePointerManager>.GetIstance(out var mousePointerManager))
            {
                mousePointerManager._clickables.Clear();
                mousePointerManager._trigger.Clear();
                mousePointerManager._auxes.Clear();
                mousePointerManager._doubleClicks.Clear();
                mousePointerManager._pointerPressed.Clear();
            }
        }
        #endregion

        #region Unity Callbacks
        protected void Reset()
        {
            gameCamera = Camera.main;
        }

        protected override void Awake()
        {
            base.Awake();

            _click = new InputAction(binding: "<Mouse>/leftButton");

            _click.performed += ctx =>
            {
                if (gameCamera == null)
                {
                    return;
                }

                _auxes.Clear();

                if (dimension == DimensionsEnum.ThreeDimension)
                {
                    _position = PointerExtend.GetScreenPosition();
                    RaycastHit[] hits = Physics.RaycastAll(gameCamera.ScreenPointToRay(_position), Mathf.Infinity);

                    if (hits.IsAlmostSpecificCount())
                    {
                        foreach (var hit in hits)
                        {
                            if (hit.collider != null && hit.collider.TryGetComponent<PointerReader>(out var clickable))
                            {
                                _auxes.Add(clickable);
                            }
                        }

                        ClickableEvent();
                    }
                }
                else
                {
                    _position = PointerExtend.PointerWorldPosition(gameCamera);
                    RaycastHit2D[] hits2D = Physics2D.RaycastAll(_position, -Vector3.forward);

                    if (hits2D.IsAlmostSpecificCount())
                    {
                        foreach (var hit in hits2D)
                        {
                            if (hit.collider != null && hit.collider.TryGetComponent<PointerReader>(out var clickable))
                            {
                                _auxes.Add(clickable);
                            }
                        }

                        ClickableEvent();
                    }
                }
            };

            _click.canceled += ctx =>
            {
                OnClickDetach();
            };

            _click.Enable();
        }

        protected void Update()
        {
            if (update == UpdateModes.Update)
            {
                EveryFrame();
            }
        }

        protected void LateUpdate()
        {
            if (update == UpdateModes.LateUpdate)
            {
                EveryFrame();
            }
        }

        protected void FixedUpdate()
        {
            if (update == UpdateModes.FixedUpdate)
            {
                EveryFrame();
            }
        }
        #endregion

        #region Private Methods
        private void OnClickDetach()
        {
            foreach (var clickable in _clickables)
            {
                if (_pointerPressed.Exists((x) => x == clickable))
                {
                    clickable.OnClickDetach();
                    _pointerPressed.Remove(clickable);
                }
            }

            _clickables.Clear();
        }

        private void EveryFrame()
        {
            if (!InputManager.EnableInput)
            {
                return;
            }

            _auxes.Clear();

            if (gameCamera == null)
            {
                return;
            }


            for (int i = _doubleClicks.Count - 1; i >= 0; i--)
            {
                var aux = _doubleClicks.Keys.Get(i);
                _doubleClicks[aux] += TimeExtend.GetDeltaTime(TimeType.Unscaled, update);
                if (_doubleClicks[aux] > doubleClick)
                {
                    _doubleClicks.Remove(aux);
                }
            }

            if (dimension == DimensionsEnum.ThreeDimension)
            {
                _position = PointerExtend.GetScreenPosition();
                RaycastHit[] hits = Physics.RaycastAll(gameCamera.ScreenPointToRay(_position), Mathf.Infinity);

                if (hits == null)
                {
                    foreach (var element in _trigger)
                    {
                        element.OnPointerExit();
                    }

                    _trigger.Clear();
                }
                else
                {
                    foreach (var hit in hits)
                    {
                        if (hit.collider != null && hit.collider.TryGetComponent<PointerReader>(out var clickable))
                        {
                            _auxes.Add(clickable);
                        }
                    }

                    EnterOrExitTriggerMouse();
                }
            }
            else
            {
                _position = PointerExtend.PointerWorldPosition(gameCamera);
                RaycastHit2D[] hits2D = Physics2D.RaycastAll(_position, -Vector3.forward);

                if (hits2D == null)
                {
                    foreach (var element in _trigger)
                    {
                        element.OnPointerExit();
                    }

                    _trigger.Clear();
                }
                else
                {
                    foreach (var hit in hits2D)
                    {
                        if (hit.collider != null && hit.collider.TryGetComponent<PointerReader>(out var clickable))
                        {
                            _auxes.Add(clickable);
                        }
                    }

                    EnterOrExitTriggerMouse();
                }
            }
        }

        private void EnterOrExitTriggerMouse()
        {
            for (int i = _trigger.Count - 1; i >= 0; i--)
            {
                var element = _trigger[i];
                if (!_auxes.Exists((x) => x == element))
                {
                    element.OnPointerExit();
                    _trigger.Remove(element);
                }
            }

            SortedByPriority();

            bool forceExit = false;

            foreach (var aux in _auxes)
            {
                bool triggerExist = _trigger.Exists((x) => x == aux);

                if (forceExit)
                {
                    if (triggerExist)
                    {
                        aux.OnPointerExit();
                        _trigger.Remove(aux);
                    }
                }
                else
                {
                    if (!triggerExist)
                    {
                        aux.OnPointerEnter();
                        _trigger.Add(aux);
                    }
                }

                if (aux.Block)
                {
                    forceExit = true;
                }
            }
        }

        private void ClickableEvent()
        {
            SortedByPriority();

            foreach (var aux in _auxes)
            {
                if (_doubleClicks.ContainsKey(aux))
                {
                    aux.OnClickPress();
                    _pointerPressed.AddOnce(aux);
                    _doubleClicks.Remove(aux);
                }

                if (aux.UseDoubleClick)
                {
                    _doubleClicks.Update(aux, 0);
                }
                else
                {
                    aux.OnClickPress();
                    _pointerPressed.AddOnce(aux);
                }

                _clickables.Add(aux);

                if (aux.Block)
                {
                    break;
                }
            }
        }

        private void SortedByPriority()
        {
            _auxes.Sort(delegate (PointerReader x, PointerReader y)
            {
                return -x.Priority.CompareTo(y.Priority);
            });
        }
        #endregion
    }
}
