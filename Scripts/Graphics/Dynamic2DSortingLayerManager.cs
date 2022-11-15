using UnityEngine;

namespace Pearl
{

    public class Dynamic2DSortingLayerManager : DynamicSortingLayerManager
    {
        [SerializeField]
        private SemiAxis2DEnum currentAxis = SemiAxis2DEnum.Up;
        private float _currentStep;

        public override int CalculateDistance(SortingOrderData sorter)
        {
            if (cam == null)
            {
                return 0;
            }

            float currentAxisValue = 0;

            Vector2 viewportPoint = cam.WorldToViewportPoint(sorter.transform.position);
            int maxSortingLayer = SpriteRenderExtend.MaxSortingLayer;

            currentAxisValue = currentAxis == SemiAxis2DEnum.Up || currentAxis == SemiAxis2DEnum.Down ? viewportPoint.y : viewportPoint.x;
            float a = currentAxis == SemiAxis2DEnum.Up || currentAxis == SemiAxis2DEnum.Right ? maxSortingLayer : -maxSortingLayer;
            float b = currentAxis == SemiAxis2DEnum.Up || currentAxis == SemiAxis2DEnum.Right ? -maxSortingLayer : maxSortingLayer;

            float aux = Mathf.Lerp(a, b, currentAxisValue);
            return Mathf.FloorToInt(aux);
        }
    }
}