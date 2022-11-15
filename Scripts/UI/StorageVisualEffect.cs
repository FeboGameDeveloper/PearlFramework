using Pearl.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl
{
    public class StorageVisualEffect : PearlBehaviour
    {
        [Header("Tween")]

        [SerializeField]
        private float timeTween = 2f;
        [SerializeField]
        private FunctionEnum function = FunctionEnum.Linear;

        private TweenContainer _tween;
        private Image _image;

        protected override void Start()
        {
            base.Start();

            AddAction(ConstantStrings.StartStorage, Play, DeleteGameObjectEnum.Destroy);
            AddAction(ConstantStrings.FinishStorage, Stop, DeleteGameObjectEnum.Destroy);

            _image = GetComponent<Image>();

            void SetNewValue(Vector4 newValue)
            {
                if (_image)
                {
                    _image.fillAmount += newValue.x;
                }
            }

            Vector4 GetInitValue()
            {
                if (_image)
                {
                    return new Vector4(_image.fillAmount, 0, 0, 0);
                }

                return Vector4.zero;
            }

            SetNewValue(Vector4.zero);
            _tween = TweenContainer.CreateTween(GetInitValue, timeTween, SetNewValue, false, new(function), ChangeModes.Time, new Vector4(1, 0, 0, 0));
            if (_tween != null)
            {
                _tween.IsLoop = true;
                _tween.PingPong = true;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_tween != null)
            {
                _tween.Kill();
            }
        }

        public void Play()
        {
            if (_tween.CurrentState != TweenState.Active)
            {
                if (_tween != null)
                {
                    _tween.Play();
                }
            }
        }

        public void Stop()
        {
            if (_tween != null)
            {
                _tween.ForceFinish();
            }
        }
    }
}
