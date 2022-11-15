using Pearl.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl.UI
{
    public class LoadingBarManager : MonoBehaviour, IFill
    {
        #region Inspector Fields
        [SerializeField]
        private Slider slider = default;
        #endregion

        #region Unity CallBacks
        void Start()
        {
            PearlEventsManager.AddAction<float>(ConstantStrings.loadingLevel, Fill);
        }

        private void Reset()
        {
            slider = GetComponent<Slider>();
        }
        #endregion

        #region Public Methods
        public void Fill(float percent)
        {
            if (slider)
            {
                slider.value = percent;
            }
        }
        #endregion
    }
}