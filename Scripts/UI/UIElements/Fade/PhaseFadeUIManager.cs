using System.Collections;
using UnityEngine;


namespace Pearl.UI
{
    public class PhaseFadeUIManager : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private Sprite[] sprites = null;

        [SerializeField]
        private float fadeTime = default;

        [SerializeField]
        private float waitTime = default;

        [SerializeField]
        private StateImageFade initStateLogo = default;

        [SerializeField]
        private FadeInOutImages fadeInOutImages1 = null;

        [SerializeField]
        private FadeInOutImages fadeInOutImages2 = null;
        #endregion

        #region Private Fields
        private Sprite currentSprite1;
        private Sprite currentSprite2;
        private StateLogosFadeInfoDictionary fadeInfoDictonary;
        #endregion

        #region Unity callbacks
        private void Awake()
        {
            fadeInfoDictonary = new StateLogosFadeInfoDictionary
            {
                { StateImageFade.FadeIn, new FadeInfo(fadeTime, 0) },
                { StateImageFade.WaitEnter, new FadeInfo(waitTime, 1)  },
                { StateImageFade.FadeOut, new FadeInfo(fadeTime, 1)  },
                { StateImageFade.WaitExit, new FadeInfo(waitTime, 0)  }
            };

            if (fadeInOutImages1 && fadeInOutImages2)
            {
                fadeInOutImages1.SetAlpha(0);
                fadeInOutImages2.SetAlpha(0);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            if (fadeInOutImages1)
            {
                fadeInOutImages1.Sprites = sprites;
                fadeInOutImages1.FadeDictonary = fadeInfoDictonary;
                fadeInOutImages1.InitState = initStateLogo;
                fadeInOutImages1.Init();
            }

            float phaseTime = 0;

            switch (initStateLogo)
            {
                case StateImageFade.FadeIn:
                    phaseTime = fadeTime + waitTime;
                    break;
                case StateImageFade.FadeOut:
                    phaseTime = 0;
                    break;
                case StateImageFade.WaitEnter:
                    phaseTime = waitTime;
                    break;
                case StateImageFade.WaitExit:
                    phaseTime = fadeTime + (2 * waitTime);
                    break;
            }

            StartCoroutine(InvokeFadeManager(phaseTime));
        }
        #endregion

        #region Public Methods
        public Sprite GetRandomSprite(Sprite currentSprite)
        {
            if (currentSprite == null)
            {
                if (currentSprite1 == null)
                {
                    currentSprite1 = RandomExtend.GetRandomElement(sprites);
                    return currentSprite1;
                }
                else
                {
                    currentSprite2 = RandomExtend.GetRandomElement(sprites);
                    return currentSprite2;
                }
            }

            if (currentSprite.Equals(currentSprite1))
            {
                if (sprites.Length == 1)
                {
                    currentSprite1 = RandomExtend.GetRandomElement(sprites);
                }
                else if (sprites.Length == 2)
                {
                    currentSprite1 = sprites[0];
                }
                else
                {
                    currentSprite1 = RandomExtend.GetRandomElement(sprites, currentSprite1, currentSprite2);

                }

                return currentSprite1;
            }
            else
            {
                if (sprites.Length == 1)
                {
                    currentSprite2 = RandomExtend.GetRandomElement(sprites);
                }
                else if (sprites.Length == 2)
                {
                    currentSprite2 = sprites[1];
                }
                else
                {
                    currentSprite2 = RandomExtend.GetRandomElement(sprites, currentSprite1, currentSprite2);
                }

                return currentSprite2;
            }
        }
        #endregion

        #region Private Methods
        private IEnumerator InvokeFadeManager(float waitTime)
        {
            if (waitTime != 0)
            {
                yield return new WaitForSeconds(waitTime);
            }

            if (fadeInOutImages2)
            {
                fadeInOutImages2.Sprites = sprites;
                fadeInOutImages2.FadeDictonary = fadeInfoDictonary;

                fadeInOutImages2.Init();
            }
        }
        #endregion
    }
}
