using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public abstract class DynamicSortingLayerManager : MonoBehaviour
    {
        #region Struct
        public struct SortingOrderData
        {
            public int specialElementToAdd;
            public Transform transform;
            public UnityEvent eventWhenChangeSortingOrden;

            public SortingOrderData(int specialElementToAdd, Transform transform, UnityEvent eventWhenChangeSortingOrden)
            {
                this.specialElementToAdd = specialElementToAdd;
                this.transform = transform;
                this.eventWhenChangeSortingOrden = eventWhenChangeSortingOrden;
            }
        }
        #endregion

        #region Inspector Fields
        [SerializeField]
        protected Camera cam = default;
        [SerializeField]
        protected string nameSortingLayer = "Default";
        #endregion

        #region protected Fields
        protected Dictionary<SpriteRenderer, SortingOrderData> _spritesRenderer = new();
        #endregion

        #region Unity Callbacks
        protected void Reset()
        {
            cam = Camera.main;
        }

        public virtual void Update()
        {
            CalculatSotingOrders();
        }
        #endregion

        #region Public method
        public virtual void SetCamera(Camera newCam)
        {
            cam = newCam;
        }

        public void AddSpriteRenderer(SpriteRenderer spriteRenderer, int specialElementToAdd, bool isStatic, UnityEvent eventWhenChangeSortingOrden)
        {
            if (_spritesRenderer != null && spriteRenderer != null)
            {
                Transform transform = spriteRenderer.transform;
                SortingOrderData sorter = new(specialElementToAdd, transform, eventWhenChangeSortingOrden);

                if (!isStatic)
                {
                    _spritesRenderer.Update(spriteRenderer, sorter);
                }

                spriteRenderer.sortingLayerName = nameSortingLayer;
                CalculateSortingOrder(spriteRenderer, sorter);
            }
        }

        public void RemoveSpriteRenderer(SpriteRenderer spriteRenderer)
        {
            if (_spritesRenderer != null)
            {
                _spritesRenderer.Remove(spriteRenderer);
            }
        }
        #endregion

        #region Private methods
        private void CalculatSotingOrders()
        {
            if (_spritesRenderer != null)
            {
                foreach (var spriteRendererCollection in _spritesRenderer)
                {
                    CalculateSortingOrder(spriteRendererCollection.Key, spriteRendererCollection.Value);
                }
            }
        }

        private void CalculateSortingOrder(SpriteRenderer spriteRenderer, SortingOrderData sortingData)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            int distance = CalculateDistance(sortingData);
            spriteRenderer.sortingOrder = distance + sortingData.specialElementToAdd;
            sortingData.eventWhenChangeSortingOrden?.Invoke();

        }
        #endregion

        #region Abstract methods
        public abstract int CalculateDistance(SortingOrderData sorter);
        #endregion
    }

}