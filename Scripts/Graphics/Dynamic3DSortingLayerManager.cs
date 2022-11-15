using UnityEngine;

namespace Pearl
{
    public class Dynamic3DSortingLayerManager : DynamicSortingLayerManager
    {
        [SerializeField]
        private bool isCameraRotate = false;
        [SerializeField]
        protected float minStep = 0.01f;

        private Axis3DEnum _currentAxis;

        private float _currentStep;

        public void Awake()
        {
            _currentStep = 1f / minStep;

            CalculateAxis();
        }

        public override void Update()
        {
            if (isCameraRotate)
            {
                CalculateAxis();
            }

            base.Update();
        }

        public override void SetCamera(Camera newCam)
        {
            base.SetCamera(newCam);
            CalculateAxis();
        }

        public override int CalculateDistance(SortingOrderData sorter)
        {
            if (cam == null)
            {
                return 0;
            }

            float currentAxisValue = 0;
            float camCurrentAxisValue = 0;

            switch (_currentAxis)
            {
                case Axis3DEnum.X:
                    currentAxisValue = sorter.transform.position.x;
                    camCurrentAxisValue = cam.transform.position.x;
                    break;
                case Axis3DEnum.Y:
                    currentAxisValue = -sorter.transform.position.y;
                    camCurrentAxisValue = cam.transform.position.y;
                    break;
                case Axis3DEnum.Z:
                    currentAxisValue = sorter.transform.position.z;
                    camCurrentAxisValue = cam.transform.position.z;
                    break;
            }

            return Mathf.FloorToInt((-Mathf.Abs(camCurrentAxisValue - currentAxisValue) * _currentStep));
        }

        private void CalculateAxis()
        {
            if (cam == null)
            {
                return;
            }

            Vector3 cameraDirection = cam.transform.forward;
            float absX = Mathf.Abs(cameraDirection.x);
            float absY = Mathf.Abs(cameraDirection.y);
            float absZ = Mathf.Abs(cameraDirection.z);

            if (absX >= absY && absX >= absZ)
            {
                _currentAxis = Axis3DEnum.X;
            }
            else if (absY >= absX && absY >= absZ)
            {
                _currentAxis = Axis3DEnum.Y;
            }
            else if (absZ >= absX && absZ >= absY)
            {
                _currentAxis = Axis3DEnum.Z;
            }
        }
    }
}
