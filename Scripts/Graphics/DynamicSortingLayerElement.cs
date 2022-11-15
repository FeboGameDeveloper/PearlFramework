using UnityEngine;

namespace Pearl
{
    public class DynamicSortingLayerElement : MonoBehaviour
    {
        [SerializeField]
        private int specialElementToAdd = 0;
        [SerializeField]
        private bool isEqualContainer = false;
        [SerializeField]
        private bool isStatic = false;

        // Start is called before the first frame update
        public void Start()
        {
            var dynamicSortingLayerManager = GameObject.FindObjectOfType<DynamicSortingLayerManager>();
            var spritesRenderer = gameObject.GetChildrenInHierarchy<SpriteRenderer>(true);

            if (dynamicSortingLayerManager != null && spritesRenderer != null)
            {
                dynamicSortingLayerManager.AddSpriteRenderer(GetComponent<SpriteRenderer>(), specialElementToAdd, isEqualContainer, isStatic);

                foreach (var spriteRenderer in spritesRenderer)
                {
                    if (!spriteRenderer.GetComponent<DynamicSortingLayerElement>())
                    {
                        dynamicSortingLayerManager.AddSpriteRenderer(spriteRenderer, specialElementToAdd, isEqualContainer, isStatic);
                    }
                }
            }
        }

        public void OnDestroy()
        {
            var dynamicSortingLayerManager = GameObject.FindObjectOfType<DynamicSortingLayerManager>();
            var spritesRenderer = gameObject.GetChildrenInHierarchy<SpriteRenderer>(false);
            if (dynamicSortingLayerManager != null && spritesRenderer != null)
            {
                foreach (var spriteRenderer in spritesRenderer)
                {
                    dynamicSortingLayerManager.RemoveSpriteRenderer(spriteRenderer);
                }
            }
        }

        public bool IsContainer()
        {
            return !isEqualContainer;
        }
    }
}

