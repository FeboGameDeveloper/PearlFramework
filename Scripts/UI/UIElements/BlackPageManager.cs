using Pearl.Tweens;
using UnityEngine;
using UnityEngine.UI;

namespace Pearl
{
    public class BlackPageManager : PearlBehaviour, ISingleton
    {
        [SerializeField]
        private Image container = null;

        private TweenContainer tween;

        public static void AppearBlackPage(float time = 0, TypeSibilling sibilling = TypeSibilling.Last, int positionChild = 0)
        {
            if (Singleton<BlackPageManager>.GetIstance(out var blackManager))
            {
                if (blackManager)
                {
                    blackManager.Appear(time, sibilling, positionChild);
                }
            }
        }

        public static void DisappearBlackPage(float time = 0, TypeSibilling sibilling = TypeSibilling.Last, int positionChild = 0)
        {
            if (Singleton<BlackPageManager>.GetIstance(out var blackManager))
            {
                if (blackManager)
                {
                    blackManager.Disappear(time, sibilling, positionChild);
                }
            }
        }


        private void Reset()
        {
            container = GetComponent<Image>();
        }

        protected override void Awake()
        {
            base.Awake();

            tween = TweensExtend.FadeTween(container, 0, false, ChangeModes.Time);
        }

        public void Appear(in float time = 0, TypeSibilling sibilling = TypeSibilling.Last, int positionChild = 0)
        {
            transform.SetSibilling(sibilling, positionChild);

            if (tween != null)
            {
                tween.Stop();
                tween.Duration = time;
                tween.FinalValues = ArrayExtend.CreateArray(Vector4.one);
                tween.Play(true);
            }
        }

        public void Disappear(in float time = 0, TypeSibilling sibilling = TypeSibilling.Last, int positionChild = 0)
        {
            transform.SetSibilling(sibilling, positionChild);

            if (tween != null)
            {
                tween.Stop();
                tween.Duration = time;
                tween.FinalValues = ArrayExtend.CreateArray(Vector4.zero);
                tween.Play(true);
            }
        }

    }
}
