using UnityEngine;

namespace Pearl
{
    public class ImageAndTextElementInfo : ImageElementInfo
    {
        public string text;
        public bool isLocalizeText;
        public Color color = Color.white;

        public ImageAndTextElementInfo(Sprite sprite, string ID, bool isLocalizeImage, string text, bool isLocalizeText) : base(sprite, ID, isLocalizeImage)
        {
            this.text = text;
            this.isLocalizeText = isLocalizeText;
        }

        public ImageAndTextElementInfo(Sprite sprite, string ID, bool isLocalizeImage, string text, bool isLocalizeText, Color color) : this(sprite, ID, isLocalizeImage, text, isLocalizeText)
        {
            this.color = color;
        }
    }
}

