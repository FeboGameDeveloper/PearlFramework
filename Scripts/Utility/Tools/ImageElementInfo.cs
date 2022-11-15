using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public class ImageElementInfo
    {
        public bool isLocalize;
        public Sprite sprite;
        public string ID;

        public ImageElementInfo(Sprite sprite, string ID, bool isLocalize)
        {
            this.sprite = sprite;
            this.ID = ID;
            this.isLocalize = isLocalize;
        }
    }
}
