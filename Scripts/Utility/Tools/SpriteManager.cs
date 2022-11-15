using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl
{
    //class che gestice tutti i componeti che hnno uno sprite o un colore.
    public class SpriteManager
    {
        private readonly SpriteRenderer spriteRender;
        private readonly Image spriteUIRender;
        private readonly TMP_Text textRender;
        private readonly bool _isActive = true;

        public bool IsActive { get { return _isActive; } }

        public Transform Transform
        {
            get
            {
                if (spriteRender != null)
                {
                    return spriteRender.transform;
                }
                else if (spriteUIRender != null)
                {
                    return spriteUIRender.transform;
                }
                else if (textRender != null)
                {
                    return textRender.transform;
                }

                return default;
            }
        }


        public static SpriteManager[] GetSpriteManagers(Transform parent, bool onlyChildren = false)
        {
            if (parent == null)
            {
                return null;
            }

            return GetSpriteManagers(parent.gameObject, onlyChildren);
        }

        public static SpriteManager[] GetSpriteManagers(GameObject parent, bool onlyChildren = false)
        {
            if (parent == null)
            {
                return null;
            }

            List<SpriteManager> spriteManagers = new();
            List<GameObject> allObj = parent.GetChildrenInHierarchy<GameObject>(onlyChildren);
            foreach (var element in allObj)
            {
                if (IsValid(element))
                {
                    spriteManagers.Add(new SpriteManager(element));
                }
            }

            return spriteManagers.ToArray();
        }

        public static bool IsValid(Transform tr)
        {
            if (tr == null)
            {
                return false;
            }

            return IsValid(tr.gameObject);
        }

        public static bool IsValid(GameObject obj)
        {
            return obj != null &&
                (obj.TryGetComponent<Image>(out var _) || obj.TryGetComponent<SpriteRenderer>(out var _) || obj.TryGetComponent<TMP_Text>(out var _));
        }

        public SpriteManager(GameObject owner)
        {
            if (owner)
            {
                var image = owner.GetComponent<Image>();

                if (image != null)
                {
                    spriteUIRender = image;
                    return;
                }


                var renderer = owner.GetComponent<SpriteRenderer>();

                if (renderer != null)
                {
                    spriteRender = renderer;
                    return;
                }

                var tmp = owner.GetComponent<TMP_Text>();
                if (tmp != null)
                {
                    textRender = tmp;
                    return;
                }
            }
            _isActive = false;
        }

        public SpriteManager(Transform component) : this(component.gameObject)
        {

        }

        public SpriteManager(Component component)
        {
            if (component)
            {
                if (component is Image image)
                {
                    spriteUIRender = image;
                    return;
                }

                if (component is SpriteRenderer renderer)
                {
                    spriteRender = renderer;
                    return;
                }

                if (component is TMP_Text tmp)
                {
                    textRender = tmp;
                    return;
                }
            }
            _isActive = false;
        }

        public void SetPreserveAspect(bool value)
        {
            if (spriteUIRender)
            {
                spriteUIRender.preserveAspect = value;
            }
        }

        public Color GetColor()
        {
            if (spriteRender)
            {
                return spriteRender.color;
            }
            if (spriteUIRender)
            {
                return spriteUIRender.color;
            }
            if (textRender)
            {
                return textRender.color;
            }
            return default;
        }

        public void SetSprite(Sprite newSprite)
        {
            if (spriteRender)
            {
                spriteRender.sprite = newSprite;
            }
            if (spriteUIRender)
            {
                spriteUIRender.sprite = newSprite;
            }
        }

        public void SetColor(float r, float g, float b, float alpha = 1)
        {
            SetColor(new Color(r, g, b, alpha));
        }

        public void SetAlpha(float alpha)
        {
            Color currentColor = GetColor();
            currentColor.a = alpha;
            SetColor(currentColor);
        }

        public void SetColor(Color newColor)
        {
            if (spriteRender)
            {
                spriteRender.color = newColor;
            }
            if (spriteUIRender)
            {
                spriteUIRender.color = newColor;
            }
            if (textRender)
            {
                textRender.color = newColor;
            }
        }
    }
}
