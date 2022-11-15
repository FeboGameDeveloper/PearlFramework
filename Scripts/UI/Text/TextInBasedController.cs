using Pearl.Events;
using Pearl.Input;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Pearl.UI
{
    public class TextInBasedController : PearlBehaviour
    {
        #region Inspector

        [SerializeField]
        private Dictionary<InputDeviceEnum, string> texts = null;

        [SerializeField]
        private bool isTextManager = false;

        [SerializeField]
        [ConditionalField("!@isTextManager")]
        private TMP_Text textContainer = null;

        [SerializeField]
        [ConditionalField("@isTextManager")]
        private TextManager textManager = null;

        [SerializeField]
        private int player = 0;
        #endregion

        #region Property
        public int Player { set { player = value; } }
        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            PearlEventsManager.AddAction<InputDeviceEnum, int>(ConstantStrings.ChangeInputDevice, ChangeText);
            var manager = InputManager.Input;
            ChangeText(manager ? manager.CurrentInputDevice : InputDeviceEnum.Null, player);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PearlEventsManager.RemoveAction<InputDeviceEnum, int>(ConstantStrings.ChangeInputDevice, ChangeText);
        }
        #endregion

        #region Private Methods
        private void ChangeText(InputDeviceEnum newInputDevice, int player)
        {
            if (this.player == player && texts != null &&
                texts.TryGetValue(newInputDevice, out string result))
            {
                if (!isTextManager && textContainer != null)
                {
                    textContainer.text = result;
                }
                else if (isTextManager && textManager != null)
                {
                    textManager.SetText(result);
                }
            }
        }
        #endregion
    }
}
