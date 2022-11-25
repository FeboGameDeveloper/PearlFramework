using Pearl.Pools;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.UI
{
    public class DynamicNavbar : MonoBehaviour
    {
        #region Inspector
        [SerializeField]
        private GameObject navbarElementPrefab = null;
        [SerializeField]
        private bool usePool = false;
        #endregion

        #region Private field
        private readonly List<NavbarElement> navbarElements = new();
        #endregion

        #region UnityCallbacks
        private void Awake()
        {
            if (usePool)
            {
                PoolManager.InstantiatePool(navbarElementPrefab, false);
            }

            foreach (Transform child in transform)
            {
                if (navbarElements != null && child.IsNotNullAndTryGetComponent<NavbarElement>(out var element))
                {
                    navbarElements.Add(element);
                }
            }
        }

        private void OnDestroy()
        {
            DestroyNavbarElements();
        }
        #endregion

        #region Public Methods
        public void CreateNavbarElements(params NavbarInfoElement[] elements)
        {
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    if (element != null)
                    {
                        NavbarElement navbarElement;

                        if (usePool)
                        {
                            PoolManager.Instantiate<NavbarElement>(out navbarElement, navbarElementPrefab, transform);
                        }
                        else
                        {
                            GameObjectExtend.CreateGameObject<NavbarElement>(navbarElementPrefab, out navbarElement, parent: transform);
                        }

                        if (navbarElement != null)
                        {
                            navbarElement.UpdateElement(element);
                            navbarElements.Add(navbarElement);
                        }
                    }
                }
            }
        }

        public void ActiveNavbarElements(bool enable)
        {
            if (navbarElements != null)
            {
                foreach (var element in navbarElements)
                {
                    if (element != null)
                    {
                        element.gameObject.SetActive(enable);
                    }
                }
            }
        }

        public void DestroyNavbarElements()
        {
            if (navbarElements != null)
            {
                foreach (var element in navbarElements)
                {
                    if (element != null)
                    {
                        GameObjectExtend.DestroyExtend(element.gameObject);
                    }
                }
                navbarElements.Clear();
            }
        }
        #endregion
    }
}
