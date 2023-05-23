#if INK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pearl;
using Pearl.Events;
using System;
using UnityEngine.Events;

namespace Pearl.Ink
{
    public class TalkManager : MonoBehaviour
    {
        #region Inspector fields
        [SerializeField]
        private ComponentReference<TextManager> textManagerComponent = null;
        [SerializeField]
        private StoryIndex storyIndex = default;
        [SerializeField]
        private bool onStart = false;
        [SerializeField]
        private StringEvent eventInText;
        #endregion

        #region Events
        public event Action OnFinishWriteText;
        #endregion

        #region Private fields
        private bool _active = false;
        private TextManager _textManager;
        private int _IDDialog;
        #endregion

        #region Unity callbacks
        public void Start()
        {
            DialogsManager.CreateDialog(storyIndex, out _IDDialog, OnText, OnFinishText);

            if (onStart)
            {
                ActiveText();
            }
        }

        public void OnDestroy()
        {
            DialogsManager.ForceDialog(_IDDialog);
            DisactiveText();
        }
        #endregion

        #region Pubblic methods
        public void ActiveText()
        {
            if (textManagerComponent != null)
            {
                _textManager = textManagerComponent.Component;
            }

            if (_textManager != null && !_active)
            {
                _textManager.OnFinishWriteText.AddListener(OnFinishText);
                _textManager.OnEvent += OnEvent;
                _active = true;
            }
        }

        public void DisactiveText()
        {
            if (_textManager != null && _active)
            {
                _textManager.OnFinishWriteText.RemoveListener(OnFinishText);
                _textManager.OnEvent -= OnEvent;
                _active = false;
            }
        }

        public void OnText(TextStruct textStruct)
        {
            if (_textManager != null)
            {
                _textManager.SetText(textStruct.text);
            }
        }

        public void OnFinishText()
        {
            OnFinishWriteText?.Invoke();
        }

        public void Talk()
        {
            if (_textManager != null && _textManager.CurrentState != StateText.Null)
            {
                _textManager.SkipText();
            }
            else
            {
                DialogsManager.ReadNextText(_IDDialog);
            }
        }

        public void ChangePath(in string path)
        {
            DialogsManager.ChangePath(_IDDialog, path);
        }

        public void SetVar(string var, string newValue)
        {
            DialogsManager.SetVar(_IDDialog, var, newValue);
        }
        #endregion

        #region Private methods
        private void OnEvent(string name, string value)
        {
            eventInText?.Invoke(name);
        }
        #endregion
    }
}

#endif
