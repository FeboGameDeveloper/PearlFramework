using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    public class GIFExtend : MonoBehaviour
    {
        [SerializeField]
        private GameObject referenceContainerSprite = null;
        [SerializeField]
        private bool useGIF = true;
        [SerializeField]
        private float updateFrame = 0.05f;
        [SerializeField]
        private Sprite[] sprites = null;

        private SpriteManager _spriteManager;
        private SimpleTimer _timer;
        private int _currentIndex = 0;


        public bool UseGIF
        {
            set
            {
                useGIF = value;
                ResetGIF();
            }
        }

        public Sprite[] Sprites
        {
            set
            {
                sprites = value;
                ResetGIF();
            }
        }

        public int CurrentIndex
        {
            set
            {
                _currentIndex = value;
                if (sprites != null)
                {
                    _currentIndex = Mathf.Clamp(_currentIndex, 0, sprites.Length);
                }
                ResetGIF();
            }
        }



        // Start is called before the first frame update
        protected void Awake()
        {
            if (sprites.IsAlmostSpecificCount())
            {
                _spriteManager = new SpriteManager(referenceContainerSprite);
                _timer = new SimpleTimer(updateFrame);
            }
        }

        protected void Reset()
        {
            referenceContainerSprite = gameObject;
        }

        protected void Start()
        {
            if (useGIF)
            {
                SetFrame();
            }
        }

        // Update is called once per frame
        protected void Update()
        {
            if (useGIF && sprites != null && _timer.IsFinish())
            {
                _currentIndex = MathfExtend.ChangeInCircle(_currentIndex, 1, sprites.Length);
                _timer.Reset();
                SetFrame();
            }
        }

        private void ResetGIF()
        {
            _timer.Reset();
            _currentIndex = 0;
        }

        private void SetFrame()
        {
            if (sprites.IsAlmostSpecificCount() && _currentIndex >= 0 && _currentIndex < sprites.Length)
            {
                _spriteManager?.SetSprite(sprites[_currentIndex]);
            }
        }
    }
}
