using Pearl.Input;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class PearlScrollRect : ScrollRect
    {
        [SerializeField]
        private bool useInputForHorizontalScrollbar = false;
        [SerializeField]
        private bool useInputForVerticalScrollbar = false;
        [SerializeField]
        [ConditionalField("@useInputForHorizontalScrollbar")]
        private ReaderNumericInput inputX = default;
        [SerializeField]
        [ConditionalField("@useInputForVerticalScrollbar")]
        private ReaderNumericInput inputY = default;
        [ConditionalField("@useInputForVerticalScrollbar || @useInputForHorizontalScrollbar")]
        private float speedInput = 0.1f;

        [SerializeField]
        private bool useFocus = false;
        [SerializeField]
        private bool focusInternal = false;
        [SerializeField]
        [ConditionalField("@focusInternal")]
        private bool useComplexElement = false;
        [SerializeField]
        [ConditionalField("@focusInternal")]
        private Range rangeForActvateInternal = null;

        private List<Selectable> selectables = new();
        private Range _rangeLeft = null;
        private Range _rangeRight = null;

        protected override void Awake()
        {
            base.Awake();

            speedInput = Mathf.Abs(speedInput);

            if (useFocus)
            {
                FocusManager.OnNewFocus += OnNewFocus;
            }

            if (focusInternal && rangeForActvateInternal != null)
            {
                _rangeLeft = new Range(rangeForActvateInternal.MinElement, 0);
                _rangeRight = new Range(0, rangeForActvateInternal.MaxElement);
            }

            CreteSelectables();
        }

        protected void Update()
        {
            if (inputX != null && useInputForHorizontalScrollbar)
            {
                inputX.GetValue((value) => AddNormalizedPosition(value * speedInput, 0));
            }

            if (inputY != null && useInputForVerticalScrollbar)
            {
                inputY.GetValue((value) => AddNormalizedPosition(value * speedInput, 1));
            }
        }

        protected void AddNormalizedPosition(float value, int axis)
        {
            float delta = axis == 0 ? value + horizontalNormalizedPosition : value + verticalNormalizedPosition;
            SetNormalizedPosition(delta, axis);
        }

        protected override void SetNormalizedPosition(float value, int axis)
        {
            //float delta = axis == 0 ? value - horizontalNormalizedPosition : value - verticalNormalizedPosition;

            base.SetNormalizedPosition(value, axis);

            //SetFocusAtIntrnalElement(axis, delta);
        }

        private void SetFocusAtIntrnalElement(int axis, float delta)
        {
            if (focusInternal)
            {
                bool positive = delta >= 0;

                if (selectables.IsAlmostSpecificCount() && delta != 0)
                {
                    foreach (var selectable in selectables)
                    {
                        var target = selectable.GetComponent<RectTransform>();
                        var container = viewport;
                        var bounds = container.TransformBoundsTo(target);
                        float boundsElement = axis == 0 ? bounds.center.x : bounds.center.y;

                        if ((positive && _rangeLeft.Contains(boundsElement)) || (!positive && _rangeRight.Contains(boundsElement)))
                        {
                            FocusManager.SetFocus(selectable, true);

                            if (verticalScrollbar && horizontalScrollbar)
                            {
                                selectable.SetNavigation(verticalScrollbar, verticalScrollbar, verticalScrollbar, verticalScrollbar);
                            }
                            else if (verticalScrollbar)
                            {
                                selectable.SetNavigation(verticalScrollbar, verticalScrollbar.navigation.selectOnRight, verticalScrollbar.navigation.selectOnLeft, verticalScrollbar);
                            }
                            else
                            {
                                selectable.SetNavigation(verticalScrollbar.navigation.selectOnUp, verticalScrollbar, verticalScrollbar, verticalScrollbar.navigation.selectOnDown);
                            }

                            break;
                        }
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (useFocus)
            {
                FocusManager.OnNewFocus -= OnNewFocus;
            }
        }

        public void ModifyContent()
        {
            CreteSelectables();
        }

        private void CreteSelectables()
        {
            if (focusInternal && selectables != null)
            {
                selectables.Clear();

                if (useComplexElement)
                {
                    var complexElementList = content.GetComponentsInHierarchy<IComplexSelectable>();

                    foreach (var complexElement in complexElementList)
                    {
                        selectables.Add(complexElement.GetSelectable());
                    }
                }
                else
                {
                    selectables = content.GetComponentsInHierarchy<Selectable>();
                }
            }
        }

        // Update is called once per frame
        private void OnNewFocus(GameObject newObj)
        {
            PearlInvoke.WaitForMethod<GameObject>(0.01f, ChangeScroll, newObj, TimeType.Unscaled);
        }

        private void ChangeScroll(GameObject newObj)
        {
            if (newObj != null && newObj.transform.IsChildOf(transform))
            {
                UIExtension.ScrollToCeneter(this, newObj.GetComponent<RectTransform>());
            }
        }
    }
}
