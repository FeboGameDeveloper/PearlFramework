using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public class DynamicSortingLayerElement : PearlBehaviour
    {
        [SerializeField]
        private int specialElementToAdd = 0;
        [SerializeField]
        private bool isStatic = false;
        [SerializeField]
        private bool useParent = false;
        [SerializeField, ConditionalField("@useParent")]
        private Transform parent = default;

        [SerializeField]
        private UnityEvent eventWhenChangeSortingOrden = null;

        private SpriteRenderer _spriteRenderParent;
        private SpriteRenderer _spriteRender;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            _spriteRender = GetComponent<SpriteRenderer>();

            if (useParent)
            {
                var dynamicSortiLayerParentElement = parent != null ? parent.GetComponent<DynamicSortingLayerElement>() : transform.GetComponentOnlyInParent<DynamicSortingLayerElement>();
                if (dynamicSortiLayerParentElement != null)
                {
                    dynamicSortiLayerParentElement.ForceStart();
                }

                _spriteRenderParent = parent != null ? parent.GetComponent<SpriteRenderer>() : transform.GetComponentOnlyInParent<SpriteRenderer>();
                EvalutateSortingWithParent();
            }
            else
            {
                var dynamicSortingLayerManager = GameObject.FindObjectOfType<DynamicSortingLayerManager>();

                if (dynamicSortingLayerManager != null && _spriteRender != null)
                {
                    dynamicSortingLayerManager.AddSpriteRenderer(_spriteRender, specialElementToAdd, isStatic, eventWhenChangeSortingOrden);
                }
            }

            if (isStatic)
            {
                this.enabled = false;
            }
        }

        protected void Reset()
        {
            parent = transform.parent;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!useParent && !isStatic)
            {
                var dynamicSortingLayerManager = GameObject.FindObjectOfType<DynamicSortingLayerManager>();

                if (dynamicSortingLayerManager != null && _spriteRender != null)
                {
                    dynamicSortingLayerManager.RemoveSpriteRenderer(_spriteRender);
                }
            }
        }

        protected void LateUpdate()
        {
            if (!isStatic)
            {
                EvalutateSortingWithParent();
            }
        }

        private void EvalutateSortingWithParent()
        {
            if (_spriteRenderParent != null && _spriteRender != null)
            {
                _spriteRender.sortingLayerName = _spriteRenderParent.sortingLayerName;
                _spriteRender.sortingOrder = _spriteRenderParent.sortingOrder + specialElementToAdd;
                eventWhenChangeSortingOrden?.Invoke();
            }
        }
    }
}

