using UnityEngine;

namespace Pearl
{
    //Disattiva components o gameobjects in relazione alla visibilità del render
    public class VisibleManager : MonoBehaviour
    {
        [SerializeField]
        private Renderer _renderer;

        [SerializeField]
        private TypeUnityElementEnum typeElements = TypeUnityElementEnum.GameObject;

        [SerializeField]
        [ConditionalField("@typeElements == Component")]
        private Behaviour[] components;
        [SerializeField]
        [ConditionalField("@typeElements == GameObject")]
        private GameObject[] gameObjects;

        private bool _isVisible;

        private void Reset()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            ChangeEnable();
        }

        // Update is called once per frame
        void Update()
        {
            if (_renderer == null)
            {
                return;
            }

            if (_renderer.isVisible != _isVisible)
            {
                ChangeEnable();
            }
        }

        private void ChangeEnable()
        {
            if (_renderer == null)
            {
                return;
            }

            _isVisible = _renderer != null ? _renderer.isVisible : false;

            if (typeElements == TypeUnityElementEnum.Component)
            {
                foreach (var component in components)
                {
                    component.enabled = _isVisible;
                }
            }
            else
            {
                foreach (var gameObject in gameObjects)
                {
                    gameObject.SetActive(_isVisible);
                }
            }
        }
    }
}
