using UnityEngine;

namespace Pearl.FrameRate
{
    /// <summary>
    /// This singleton class calculates the frameRate
    /// </summary>
    public class FrameRateManager : PearlBehaviour, ISingleton
    {
        #region Inspector Fields
        /// <summary>
        /// The refresh time is the time for calculate the new frameRate
        /// </summary>
        [SerializeField]
        private float refreshTime = 0.5f;
        /// <summary>
        /// The limit frame rate is the upper limit for not making the game go too fast
        /// </summary>
        [SerializeField]
        private int limitFrameRate = 60;
        #endregion

        #region Private Fields
        private float currentTime = 0;
        private int frameCounter = 0;
        private int lastFramerate = 0;
        #endregion

        #region Properties
        public static FrameRateManager Instance
        {
            get { return Singleton<FrameRateManager>.GetIstance(); }
        }


        /// <summary>
        /// This actual FrameRate
        /// </summary>
        public int FrameRate { get { return lastFramerate; } }
        #endregion

        #region Unity CallBacks
        protected override void Awake()
        {
            base.Awake();

            SettingLimitFrameRate();
        }

        protected void Update()
        {
            CalculateFrameRate();
        }
        #endregion

        #region Logical Methods
        /// <summary>
        /// This method calculate the actual frame rate 
        /// </summary>
        private void CalculateFrameRate()
        {
            currentTime += Time.deltaTime;

            if (currentTime >= refreshTime)
            {
                lastFramerate = Mathf.RoundToInt((float)frameCounter / currentTime);
                ResetCalculateFrame();
            }
            else
            {
                frameCounter++;
            }
        }

        /// <summary>
        /// This method set the limit of the desidered frame rate
        /// </summary>
        private void SettingLimitFrameRate()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = limitFrameRate;
        }

        private void ResetCalculateFrame()
        {
            frameCounter = 0;
            currentTime = 0;
        }
        #endregion
    }
}
