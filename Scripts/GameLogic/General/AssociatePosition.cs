using UnityEngine;

namespace Pearl
{
    public class AssociatePosition : MonoBehaviour
    {
        [SerializeField]
        private UpdateModes updateModes = UpdateModes.LateUpdate;
        [SerializeField]
        private ComponentReference<Transform> targetTransform = null;

        [SerializeField]
        private bool useScaleForDistance = false;
        [SerializeField]
        private bool useGraphicDistance = true;
        [SerializeField]
        [ConditionalField("!@useGraphicDistance")]
        private Vector3 distance = default;

        [UnityEngine.Header("Components")]
        [SerializeField]
        private bool ignoreX = false;
        [SerializeField]
        private bool ignoreY = false;
        [SerializeField]
        private bool ignoreZ = false;

        //Add button in inspector
        [InspectorButton("Setting")]
        [SerializeField]
        private bool settingButton;

        public Vector3 Distance { get { return distance; } set { distance = value; } }

        public Transform TargetTransform 
        {
            set 
            { 
                if (targetTransform != null)
                {
                    targetTransform.Component = value;
                    _target = targetTransform.Component;
                }
            } 
        }

        private bool _aux;
        private Transform _target;

        protected void Reset()
        {
            _aux = useGraphicDistance;
            if (targetTransform != null)
            {
                _target = targetTransform.Component;
            }
        }

        protected void OnValidate()
        {
            if (targetTransform != null)
            {
                _target = targetTransform.Component;
            }

            if (!useGraphicDistance && _aux && _target)
            {
                CalculateDistance();
            }

            _aux = useGraphicDistance;
        }

        private void Start()
        {
            if (targetTransform != null)
            {
                _target = targetTransform.Component;
            }

            InitSetting();
        }

        public void DestroyTarget()
        {
            if (targetTransform != null)
            {
                targetTransform.Component = null;
            }
            _target = null;
        }

        public void InitTarget(GameObject target)
        {
            if (target != null)
            {
                InitTarget(target.transform);
            }
        }

        public void InitTarget(Transform target)
        {
            if (target != null && targetTransform != null)
            {
                targetTransform.Component = target;
                _target = targetTransform.Component;
            }

            InitSetting();
        }

        private void InitSetting()
        {
            if (useGraphicDistance)
            {
                CalculateDistance();
            }

            Setting();
        }

        // Update is called once per frame
        private void Update()
        {
            if (updateModes == UpdateModes.Update)
            {
                Setting();
            }
        }

        private void FixedUpdate()
        {
            if (updateModes == UpdateModes.FixedUpdate)
            {
                Setting();
            }
        }

        private void LateUpdate()
        {
            if (updateModes == UpdateModes.LateUpdate)
            {
                Setting();
            }
        }

        private void Setting()
        {
            if (_target)
            {
                var currentDistance = useScaleForDistance ? Vector3.Scale(distance, _target.localScale) : distance;
                Vector3 aux = _target.position + currentDistance;
                if (ignoreX)
                {
                    aux.x = transform.position.x;
                }
                if (ignoreY)
                {
                    aux.y = transform.position.y;
                }
                if (ignoreZ)
                {
                    aux.z = transform.position.z;
                }

                transform.position = aux;
            }
        }

        private void CalculateDistance()
        {
            if (_target && transform)
            {
                distance = transform.position - _target.position;
            }
        }
    }

}