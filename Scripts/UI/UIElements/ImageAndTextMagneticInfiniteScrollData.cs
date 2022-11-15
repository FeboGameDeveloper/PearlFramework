using Pearl.UI;
using TypeReferences;
using UnityEngine;

namespace Pearl
{
    public class ImageAndTextMagneticInfiniteScrollData : UI_MagneticInfiniteScrollData<ImageAndTextElementInfo, string>
    {
        [SerializeField]
        [ClassImplements(typeof(Filler<ImageAndTextElementInfo>))]
        private ClassTypeReference imageFillerType = null;

        protected override void Awake()
        {
            base.Awake();

            if (useFiller && imageFillerType != null)
            {
                filler = ReflectionExtend.CreateInstance<Filler<ImageAndTextElementInfo>>(imageFillerType);
            }
        }
    }
}