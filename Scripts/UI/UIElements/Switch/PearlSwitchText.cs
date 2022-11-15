using TypeReferences;
using UnityEngine;

namespace Pearl.UI
{
    public class PearlSwitchText : PearlSwitchViewAbstract<string>
    {
        #region Inspector Fields
        [Header("Text Settings")]

        [SerializeField]
        private TextManager textContainer = null;

        [SerializeField]
        [ConditionalField("@useFiller")]
        [ClassImplements(typeof(Filler<string>))]
        private ClassTypeReference textFillerType = typeof(Filler<string>);
        #endregion

        #region UnityCallbacks
        protected override void Awake()
        {
            base.Awake();

            if (useFiller && textFillerType != null)
            {
                _filler = ReflectionExtend.CreateInstance<Filler<string>>(textFillerType);
            }
        }
        #endregion

        #region Override Methods
        protected override void InvokeEvent(in string currentValue)
        {
            InvokeEventIntern(currentValue);
        }

        protected override void SetContentView(in string currentValue)
        {
            if (currentValue != null && textContainer)
            {
                textContainer.SetText(currentValue);
            }
        }
        #endregion
    }
}
