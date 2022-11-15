using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public abstract class DynamicSortingLayerManager : MonoBehaviour
    {
        public struct SortingOrderData
        {
            public int specialElementToAdd;
            public Transform transform;

            public SortingOrderData(int specialElementToAdd, Transform transform)
            {
                this.specialElementToAdd = specialElementToAdd;
                this.transform = transform;
            }
        }

        [SerializeField]
        protected Camera cam = default;
        [SerializeField]
        protected string nameSortingLayer = "Default";

        protected Dictionary<SpriteRenderer, SortingOrderData> _spritesRenderer = new();

        public virtual void Update()
        {
            CalculatSotingOrders();
        }

        public virtual void SetCamera(Camera newCam)
        {
            cam = newCam;
        }

        public void AddSpriteRenderer(SpriteRenderer spriteRenderer, int specialElementToAdd, bool isEqualContainer, bool isStatic)
        {
            if (_spritesRenderer != null && spriteRenderer != null)
            {
                Transform transform = null;
                if (isEqualContainer)
                {
                    var elements = spriteRenderer.GetComponentsInParent<DynamicSortingLayerElement>();
                    foreach (var element in elements)
                    {
                        if (element != null && element.IsContainer())
                        {
                            transform = element.transform;
                            break;
                        }
                    }
                }
                else
                {
                    transform = spriteRenderer.transform;
                }

                if (transform == null)
                {
                    return;
                }

                SortingOrderData sorter = new(specialElementToAdd, transform);

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
        }

        public abstract int CalculateDistance(SortingOrderData sorter);
    }

}