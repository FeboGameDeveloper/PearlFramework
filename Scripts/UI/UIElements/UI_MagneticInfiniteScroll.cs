using Pearl.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.UI
{
    public class UI_MagneticInfiniteScroll : PearlInfiniteScroll
    {
        public event Action<GameObject> OnScrollCurrentElement;

        public const float waitForContentSet = 0.05f;

        [Tooltip("The pointer to the pivot, the visual element for centering objects.")]
        [SerializeField]
        private RectTransform pivot = null;

        [Tooltip("A boolean that indicate if the scroll need initialization with specific index")]
        [SerializeField]
        private bool useInitIndex = false;

        [Tooltip("The pointer to the object container")]
        [SerializeField]
        private RectTransform content = null;
        [Tooltip("the maximum speed that allows you to activate the magnet to center on the pivot")]
        [SerializeField]
        private float maxSpeedForMagnetic = 10f;
        [SerializeField]
        [Tooltip("The initial index of the object which must be initially centered")]
        private int indexStart = 0;
        [SerializeField]
        [Tooltip("The time to decelerate and aim for the pivot")]
        private float timeForDeceleration = 0.05f;

        private Vector2 _initVectorLerp;

        private float _pastPosition = 0;

        private float _currentSpeed = 0.0f;
        private float _stopValue = 0.0f;
        private float _currentTime = 0;
        private int _currentIndex = -1;

        private bool _isStopping = false;
        private bool _isMovement = false;
        private bool _isDrag = false;

        protected void Start()
        {
            //content.anchoredPosition = pivot.anchoredPosition;
            _isMovement = true;

            if (useInitIndex)
            {
                SetContentInPivot(indexStart);
            }
        }

        private void Update()
        {
            //OnScroll();

            if (!content || !pivot || items == null || !_isMovement)
            {
                return;
            }

            float currentPosition = GetRightAxis(content.anchoredPosition);
            _currentSpeed = Mathf.Abs(currentPosition - _pastPosition);
            _pastPosition = currentPosition;

            if (!_isStopping)
            {
                if (_isDrag || Mathf.Abs(_currentSpeed) >= maxSpeedForMagnetic)
                {
                    float distance = Mathf.Infinity;
                    int _pastIndex = _currentIndex;

                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];
                        if (item == null)
                        {
                            continue;
                        }

                        var aux = Mathf.Abs(GetRightAxis(pivot.position) - GetRightAxis(item.position));

                        if (aux < distance)
                        {
                            distance = aux;
                            _currentIndex = i;
                        }
                    }

                    if (_pastIndex != _currentIndex)
                    {
                        ScrollEvent();
                        AudioUI.PlayAudioSound(UIAudioStateEnum.OnScroll);
                    }
                }

                float absoluteSpeed = Mathf.Abs(_currentSpeed);

                if (!_isDrag && absoluteSpeed < maxSpeedForMagnetic)
                {
                    InitLerpMovement();
                }
            }

            if (!_isDrag && _isStopping)
            {
                _currentTime += Time.unscaledDeltaTime;
                float valueLerp = Mathf.Clamp(_currentTime / timeForDeceleration, 0, 1);

                float newPosition = Mathf.Lerp(GetRightAxis(_initVectorLerp), _stopValue, valueLerp);

                content.anchoredPosition = _isVertical ? new Vector2(_initVectorLerp.x, newPosition) :
                                new Vector2(newPosition, _initVectorLerp.y);


                if (newPosition == _stopValue && _currentIndex >= 0 && _currentIndex < items.Count)
                {
                    FinishMovement();
                }
            }
        }

        public override void OnScroll()
        {
            base.OnScroll();
        }

        private void OnDisable()
        {
            if (_currentIndex >= 0)
            {
                SetContentInPivot(_currentIndex);
            }
        }

        public GameObject GetCurrentItem()
        {
            if (items.IsAlmostSpecificCount(_currentIndex))
            {
                return items[_currentIndex].gameObject;
            }

            return null;
        }

        public override void SetNewItems(ref List<Transform> newItems)
        {
            foreach (var element in newItems)
            {
                RectTransform rectTransform = element.GetComponent<RectTransform>();
                if (rectTransform && pivot)
                {
                    rectTransform.sizeDelta = pivot.sizeDelta;
                }
            }
            base.SetNewItems(ref newItems);
            GetCurrentItem();
        }

        public void SetContentInPivot(int index)
        {
            _currentIndex = index;

            float newPos = GetAnchoredPositionForPivot(index);
            Vector2 anchoredPosition = content.anchoredPosition;

            if (content)
            {
                content.anchoredPosition = _isVertical ? new Vector2(anchoredPosition.x, newPos) :
                                            new Vector2(newPos, anchoredPosition.y);
                _pastPosition = GetRightAxis(content.anchoredPosition);
            }

            FinishMovement();
            ScrollEvent();
        }

        public void ScrollEvent()
        {
            if (items.IsAlmostSpecificCount(_currentIndex))
            {
                OnScrollCurrentElement?.Invoke(items[_currentIndex].gameObject);
            }
        }

        private void InitLerpMovement()
        {
            if (_scrollRect == null)
            {
                return;
            }


            _isStopping = true;
            _stopValue = GetAnchoredPositionForPivot(_currentIndex);
            _scrollRect.StopMovement();
            _initVectorLerp = content.anchoredPosition;
        }

        private void FinishMovement()
        {
            _isStopping = false;
            _isMovement = false;
            _currentTime = 0;
        }

        public void Drag()
        {
            _isStopping = false;
            _isDrag = true;
            _isMovement = true;
        }

        public void EndDrag()
        {
            _isDrag = false;
            _currentTime = 0;
        }

        private float GetAnchoredPositionForPivot(int index)
        {
            if (!pivot || items == null || items.Count <= index)
            {
                return 0f;
            }

            index = Mathf.Clamp(index, 0, items.Count - 1);

            float posItem = GetRightAxis(items[index].anchoredPosition);
            float posPivot = GetRightAxis(pivot.anchoredPosition);
            return posPivot - posItem;
        }

        private float GetRightAxis(Vector2 vector)
        {
            return _isVertical ? vector.y : vector.x;
        }
    }
}
