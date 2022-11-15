using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    /// <summary>
    /// the class allows automatic parallax based on speed and time.
    /// </summary>
    public class AutomaticParallax : MonoBehaviour
    {
        /// <summary>
        /// The container of the various levels of parallax.
        /// </summary>
        [SerializeField]
        private Transform containerLayers = null;
        /// <summary>
        /// The velocity vector of the first layer.
        /// </summary>
        [SerializeField]
        private Vector3 velocityParallax = Vector3.zero;
        /// <summary>
        /// The following layers will have percentages of velocity vector relative to the Parallax velocity.
        /// </summary>
        [SerializeField]
        private float[] percentSpeedSpecificLayer = null;
        /// <summary>
        /// The update mode
        /// </summary>
        [SerializeField]
        private UpdateModes updateModes = UpdateModes.Update;
        /// <summary>
        /// The maximum time before the velocity vector reverses.
        /// </summary>
        [SerializeField]
        private float timeForChangeDirection = default;

        private SimpleTimer _timer;
        private bool _isWrong;
        private Vector3 _currentVelocity;

        #region UnityCallbacks
        protected void Awake()
        {
            _isWrong = percentSpeedSpecificLayer == null || containerLayers == null ||
                percentSpeedSpecificLayer.Length < containerLayers.childCount;

            _timer = new SimpleTimer(timeForChangeDirection);
            _currentVelocity = velocityParallax;
        }

        protected void FixedUpdate()
        {
            if (updateModes == UpdateModes.FixedUpdate)
            {
                EveryFrame();
            }
        }

        protected void Update()
        {
            if (updateModes == UpdateModes.Update)
            {
                EveryFrame();
            }
        }

        protected void LateUpdate()
        {
            if (updateModes == UpdateModes.LateUpdate)
            {
                EveryFrame();
            }
        }
        #endregion

        private void EveryFrame()
        {
            if (_isWrong)
            {
                return;
            }

            if (_timer.IsFinish())
            {
                _timer.Reset();
                _currentVelocity = -_currentVelocity;
            }


            int i = 0;
            foreach (Transform container in containerLayers)
            {
                if (container.childCount == 0)
                {
                    container.SetTranslationInUpdate(_currentVelocity * percentSpeedSpecificLayer[i], TimeType.Scaled, updateModes);
                }
                else
                {
                    foreach (Transform child in container)
                    {
                        child.SetTranslationInUpdate(_currentVelocity * percentSpeedSpecificLayer[i], TimeType.Scaled, updateModes);
                    }
                }
                i++;
            }
        }
    }
}
