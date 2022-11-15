using Pearl.Events;
using System;
using UnityEngine;

namespace Pearl.UI
{
    public class TimerUI : MonoBehaviour
    {
        public enum FormatTime { ss, mm_ss, hh_mm_ss }

        [SerializeField]
        private TextManager text = null;

        [SerializeField]
        private string ev = null;

        [SerializeField]
        private FormatTime formatTime = FormatTime.mm_ss;

        private string _format;

        private void Awake()
        {
            switch (formatTime)
            {
                case FormatTime.ss:
                    _format = "ss";
                    break;
                case FormatTime.mm_ss:
                    _format = "mm':'ss";
                    break;
                case FormatTime.hh_mm_ss:
                    _format = "hh':'mm':'ss";
                    break;
            }
        }

        private void OnEnable()
        {
            PearlEventsManager.AddAction<float>(ev, UpdateTimer);
        }

        private void OnDisable()
        {
            PearlEventsManager.RemoveAction<float>(ev, UpdateTimer);
        }

        public void UpdateTimer(float time)
        {
            if (text == null)
            {
                return;
            }

            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            text.SetText(timeSpan.ToString(_format));
        }
    }
}
