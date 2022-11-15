using Pearl.Debug;
using System;
using UnityEngine;

namespace Pearl
{
    public struct SpriteInfo
    {
        public Sprite sprite;
        public bool flipX;

        public SpriteInfo(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer != null)
            {
                this.sprite = spriteRenderer.sprite;
                this.flipX = spriteRenderer.flipX;
            }
            else
            {
                this.sprite = null;
                this.flipX = false;
            }
        }

        public SpriteInfo(Sprite sprite, bool flipX)
        {
            this.sprite = sprite;
            this.flipX = flipX;
        }
    }

    public static class SpriteExtend
    {
        public static void SetSprite(this SpriteRenderer SpriteRenderer, SpriteInfo spriteInfo)
        {
            if (SpriteRenderer != null)
            {
                SpriteRenderer.sprite = spriteInfo.sprite;
                SpriteRenderer.flipX = spriteInfo.flipX;
            }
        }

        public static Sprite CreateSprite(string imageInString, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if (imageInString != null)
            {
                byte[] bytes = null;
                try
                {
                    bytes = System.Convert.FromBase64String(imageInString);
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e);
                    return null;
                }
                return CreateSprite(bytes, pivotX, pivotY);
            }
            return null;
        }

        public static Sprite CreateSprite(byte[] bytes, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if (bytes.IsAlmostSpecificCount())
            {
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                return CreateSprite(texture, pivotX, pivotY);
            }
            return null;
        }

        public static Sprite CreateSprite(this Texture2D texture, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if (texture != null)
            {
                return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(pivotX, pivotY));
            }

            return null;
        }

        public static Sprite[] CreateSprites(this Texture2D[] textures, float pivotX = 0.5f, float pivotY = 0.5f)
        {
            if (!textures.IsAlmostSpecificCount())
            {
                return null;
            }

            Sprite[] sprites = new Sprite[textures.Length];

            for (int i = 0; i < textures.Length; i++)
            {
                var texture = textures[i];

                sprites[i] = CreateSprite(texture, pivotX, pivotY);
            }
            return sprites;
        }
    }
}
