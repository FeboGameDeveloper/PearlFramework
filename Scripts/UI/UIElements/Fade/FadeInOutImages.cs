using Pearl.ClockManager;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Pearl.UI
{
    public enum StateImageFade { FadeIn, WaitEnter, FadeOut, WaitExit }

    [Serializable]
    public struct FadeInfo
    {
        public float deltaTime;
        public float initAlpha;

        public FadeInfo(float deltaTime, float initAlpha)
        {
            this.deltaTime = deltaTime;
            this.initAlpha = initAlpha;
        }
    }

    public class FadeInOutImages : MonoBehaviour
    {
        #region Serializable Field

        [SerializeField]
        protected Sprite[] sprites = null;

        [SerializeField]
        protected Image imageComponent = null;

        [SerializeField]
        private bool initAtStart = true;

        [SerializeField]
        private StateImageFade initState = StateImageFade.FadeIn;

        [SerializeField]
        private StateLogosFadeInfoDictionary fadeInfosDictonary = null;

        [SerializeField]
        private UnityEvent OnFinish = null;
        #endregion

        #region Privte fields
        private Timer timer;
        protected int currentIndex = -1;
        protected StateImageFade currentState;
        private bool finish = false;
        #endregion

        #region Properties
        public Sprite[] Sprites { set { sprites = value; } }

        public StateImageFade InitState { set { initState = value; } }

        public StateLogosFadeInfoDictionary FadeDictonary { set { fadeInfosDictonary = value; } }
        #endregion

        #region Unity Callback
        protected void Awake()
        {
            timer = new Timer();
            currentIndex = -1;
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if (initAtStart)
            {
                Init();
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (timer == null || !timer.On || finish || sprites == null)
            {
                return;
            }

            if (currentState == StateImageFade.FadeIn)
            {
                SetAlpha(timer.TimeInPercent);

                if (timer.IsFinish())
                {

                    if (fadeInfosDictonary.TryGetValue(StateImageFade.WaitEnter, out FadeInfo fadoInfo)
                        && fadoInfo.deltaTime > 0)
                    {
                        ChangeState(StateImageFade.WaitEnter);
                    }
                    else
                    {
                        ChangeState(StateImageFade.FadeOut);
                    }
                }
            }
            if (currentState == StateImageFade.WaitEnter && timer.IsFinish())
            {
                if (fadeInfosDictonary.TryGetValue(StateImageFade.FadeOut, out FadeInfo fadoInfo)
                      && fadoInfo.deltaTime > 0)
                {
                    ChangeState(StateImageFade.FadeOut);
                }
                else
                {
                    ChangeState(StateImageFade.WaitExit);
                }
            }
            if (currentState == StateImageFade.FadeOut)
            {
                SetAlpha(timer.TimeInPercentReversed);

                if (timer.IsFinish())
                {
                    if (fadeInfosDictonary.TryGetValue(StateImageFade.WaitExit, out FadeInfo fadoInfo)
                        && fadoInfo.deltaTime > 0)
                    {
                        ChangeState(StateImageFade.WaitExit);
                    }
                    else
                    {
                        ChangeState(StateImageFade.FadeIn);
                        ChangeSprite();
                    }
                }
            }
            if (currentState == StateImageFade.WaitExit && timer.IsFinish())
            {
                ChangeSprite();

                if (fadeInfosDictonary.TryGetValue(StateImageFade.FadeIn, out FadeInfo fadoInfo)
                    && fadoInfo.deltaTime > 0)
                {
                    ChangeState(StateImageFade.FadeIn);
                }
                else
                {
                    ChangeState(StateImageFade.WaitEnter);
                }
            }
        }
        #endregion

        #region Public Methods
        public void Init()
        {
            ChangeSprite();
            ChangeState(initState);
        }
        #endregion

        #region Protected Methds
        protected virtual void ChangeSprite()
        {
            currentIndex++;
            if (imageComponent && sprites != null && currentIndex < sprites.Length)
            {
                imageComponent.sprite = sprites[currentIndex];
            }
            else
            {
                finish = true;
                if (OnFinish != null)
                {
                    OnFinish.Invoke();
                }
            }
        }

        public void SetAlpha(float newValue)
        {
            if (imageComponent != null)
            {
                Color color = new Color(1, 1, 1, newValue);
                imageComponent.color = color;
            }
        }
        #endregion

        #region Private Methods
        private void ChangeState(StateImageFade newState)
        {
            if (timer != null && fadeInfosDictonary.IsNotNullAndTryGetValue(newState, out FadeInfo fadeInfo))
            {
                currentState = newState;
                SetAlpha(fadeInfo.initAlpha);
                timer.ResetOn(fadeInfo.deltaTime);
            }
        }
        #endregion
    }

}