#if INK

using Pearl.UI;
using System;
using System.Collections.Generic;

namespace Pearl
{
    public static class DialogsManager
    {
        private static readonly SimplePool<Dialog> _pool = new(false);
        private static readonly Dictionary<string, Dialog> _dialogs = new();

        #region Public Methods
        public static void AddEvent(in string idDIalog, Action<string> action, StartFinish typeEnum)
        {
            var dialog = GetDialog(idDIalog);
            if (dialog != null)
            {
                if (typeEnum == StartFinish.Start)
                {
                    dialog.EventLocalStart += action;
                }
                else
                {
                    dialog.EventLocalFinish += action;
                }
            }
        }

        public static void RemoveEvent(in string idDIalog, Action<string> action, StartFinish typeEnum)
        {
            var dialog = GetDialog(idDIalog);
            if (dialog != null)
            {
                if (typeEnum == StartFinish.Start)
                {
                    dialog.EventLocalStart -= action;
                }
                else
                {
                    dialog.EventLocalFinish -= action;
                }
            }
        }

        public static void ReadNextText(in string idDialog)
        {
            var dialog = GetDialog(idDialog);
            if (dialog != null)
            {
                dialog.ReadNextText();
            }
        }

        public static void ChangePath(in string idDialog, in string path)
        {
            var dialog = GetDialog(idDialog);
            if (dialog != null)
            {
                dialog.ChangePath(path);
            }
        }

        public static bool CreateDialog(in StoryIndex storyIndex, in string idDialog, in Action<TextStruct> actionText, Action finishText, in string stingTable = null)
        {
            return CreateDialog(storyIndex, idDialog, actionText, finishText, null, stingTable);
        }

        public static bool CreateDialog(in StoryIndex storyIndex, in string idDialog, in Action<TextStruct> actionText, Action finishText, in Action<QuestionInfo> actionQuestion, in string stingTable = null)
        {
            if (_pool != null && _dialogs != null && !_dialogs.ContainsKey(idDialog))
            {
                var dialog = _pool.Get();

                finishText += () => FinishDialog(dialog);
                if (dialog != null)
                {
                    _dialogs.Add(idDialog, dialog);
                    dialog.CreateDialog(storyIndex, actionText, finishText, stingTable, actionQuestion);
                    return true;
                }
            }
            return false;
        }

        public static void ForceDialog(in string idDialog)
        {
            var dialog = GetDialog(idDialog);
            if (dialog != null)
            {
                dialog.FinishDialog();
            }
        }

        public static Dialog GetDialog(string idDialog)
        {
            if (_dialogs.IsNotNullAndTryGetValue(idDialog, out var dialog))
            {
                return dialog;
            }
            return null;
        }
        #endregion

        #region Private Methods
        private static void FinishDialog(Dialog dialog)
        {
            if (_dialogs.TryGetKey(dialog, out var textEvent))
            {
                _dialogs.Remove(textEvent);
                _pool.Remove(dialog);
            }

        }

        public static void Pause(bool onPause)
        {
            foreach (var dialog in _dialogs.Values)
            {
                dialog.Pause(onPause);
            }
        }
        #endregion
    }
}

#endif