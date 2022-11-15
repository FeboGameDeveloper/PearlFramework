using Pearl.UI;
using TypeReferences;
using UnityEngine;

namespace Pearl
{
    public class ImageMagneticInfiniteScrollData : UI_MagneticInfiniteScrollData<ImageElementInfo, string>
    {
        [SerializeField]
        [ClassImplements(typeof(Filler<ImageElementInfo>))]
        private ClassTypeReference imageFillerType = null;

        protected override void Awake()
        {
            base.Awake();

            if (useFiller && imageFillerType != null)
            {
                filler = ReflectionExtend.CreateInstance<Filler<ImageElementInfo>>(imageFillerType);
            }
        }
    }
}
