using TMPro;
using UnityEngine;

namespace Pearl.FrameRate
{
    /// <summary>
    /// The class that writes the frameRate in UI
    /// </summary>
    public class FrameRateDebugManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text frameRateTextContainer = null;

        #region Private Fields
        private FrameRateManager frameRateManager;
        #endregion

        #region Unity CallBacks

        private void Start()
        {
            frameRateManager = FrameRateManager.Instance;
        }

        protected void Update()
        {
            ShowFPS(frameRateManager.FrameRate);
        }
        #endregion

        #region Private
        /// <summary>
        /// Writes the framRate in the component text
        /// </summary>
        /// <param name = "FPS"> The current FrameRate</param>
        private void ShowFPS(int FPS)
        {
            if (frameRateTextContainer)
            {
                frameRateTextContainer.text = FPS.ToString() + " " + "FPS";
            }
        }
        #endregion
    }
}
