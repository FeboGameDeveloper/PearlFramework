using TypeReferences;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class PearlSwitchImage : PearlSwitchViewAbstract<ImageElementInfo>
    {
        #region Inspector
        [Header("Image Settings")]

        [SerializeField]
        private Transform imagesContainer = null;

        [ConditionalField("@useFiller")]
        [ClassImplements(typeof(Filler<string>))]
        public ClassTypeReference imageFillerType = typeof(Filler<string>);
        #endregion

        #region UnityCallbacks
        protected override void Awake()
        {
            base.Awake();

            if (useFiller && imageFillerType != null)
            {
                _filler = ReflectionExtend.CreateInstance<Filler<ImageElementInfo>>(imageFillerType);
            }
        }
        #endregion

        #region Override Methods
        protected override void InvokeEvent(in ImageElementInfo currentValue)
        {
            string ID = currentValue.ID;
            InvokeEventIntern(ID);
        }

        protected override void SetContentView(in ImageElementInfo currentValue)
        {
            if (imagesContainer != null && currentValue != null)
            {
                imagesContainer.GetComponent<Image>().sprite = currentValue.sprite;
            }
        }
        #endregion
    }
}
