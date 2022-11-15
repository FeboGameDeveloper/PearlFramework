using Pearl.ClockManager;
using UnityEngine;

namespace Pearl
{
    public class PulseColor : MonoBehaviour
    {
        [SerializeField]
        private bool initAtStart = false;


        [SerializeField]
        private GameObject colorContainer1 = null;
        [SerializeField]
        private GameObject colorContainer2 = null;

        [SerializeField]
        private float timeForPulse = 0.3f;


        private SpriteManager _spriteManager1;
        private SpriteManager _spriteManager2;

        private Color _color1;
        private Color _color2;

        private bool _active;
        private Timer _timer;


        private void Awake()
        {
            _spriteManager1 = new SpriteManager(colorContainer1);
            _spriteManager2 = new SpriteManager(colorContainer2);

            _color1 = _spriteManager1.GetColor();
            _color2 = _spriteManager2.GetColor();

            _timer = new Timer(timeForPulse, false);
        }

        private void Start()
        {
            if (initAtStart)
            {
                PlayPulseColor();
            }
        }

        private void Update()
        {
            if (_active && _timer.IsFinish() && _spriteManager1 != null && _spriteManager2 != null)
            {
                if (_spriteManager1.GetColor() == _color1)
                {
                    _spriteManager1.SetColor(_color2);
                    _spriteManager2.SetColor(_color1);
                }
                else
                {
                    _spriteManager1.SetColor(_color1);
                    _spriteManager2.SetColor(_color2);
                }

                _timer.ResetOn();
            }
        }

        public void PlayPulseColor()
        {
            if (_timer != null)
            {
                _timer.ResetOn();
                _active = true;
            }
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.ResetOff();
                _active = true;
            }
        }
    }
}
