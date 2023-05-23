#if INK

using Pearl.UI;
using System;
using System.Collections.Generic;

namespace Pearl
{
    public static class DialogsManager
    {
        private static readonly SimplePool<Dialog> _pool = new(false);
        private static readonly Dictionary<int, Dialog> _dialogs = new();
        private static readonly IndexManager _indexManager = new();

        #region Public Methods
        public static void AddEvent(in int IDDialog, Action<string> action, StartFinish typeEnum)
        {
            var dialog = GetDialog(IDDialog);
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

        public static void RemoveEvent(in int IDDialog, Action<string> action, StartFinish typeEnum)
        {
            var dialog = GetDialog(IDDialog);
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

        public static void SetVar(in int IDDialog, string var, string newValue)
        {
            var dialog = GetDialog(IDDialog);
            dialog?.SetVar(var, newValue);
        }

        public static void ReadNextText(in int IDDialog)
        {
            var dialog = GetDialog(IDDialog);
            dialog?.ReadNextText();
        }

        public static void RepeatText(in int IDDialog)
        {
            var dialog = GetDialog(IDDialog);
            dialog?.ReadNextText(true);
        }

        public static void ChangePath(in int IDDialog, in string path)
        {
            var dialog = GetDialog(IDDialog);
            dialog?.ChangePath(path);
        }

        public static bool CreateDialog(in StoryIndex storyIndex, out int IDDialog, in Action<TextStruct> actionText, Action finishText, in string stingTable = null)
        {
            return CreateDialog(storyIndex, out IDDialog, actionText, finishText, null, stingTable);
        }

        public static bool CreateDialog(in StoryIndex storyIndex, out int IDDialog, in Action<TextStruct> actionText, Action finishText, in Action<QuestionInfo> actionQuestion, in string stingTable = null)
        {
            if (_pool != null && _dialogs != null)
            {
                var dialog = _pool.Get();

                finishText += () => FinishDialog(dialog);
                if (dialog != null && _indexManager != null)
                {
                    IDDialog = _indexManager.GetIndex();
                    _dialogs.Add(IDDialog, dialog);
                    dialog.CreateDialog(storyIndex, actionText, finishText, stingTable, actionQuestion);
                    return true;
                }
            }

            IDDialog = -1;
            return false;
        }

        public static void ForceDialog(in int IDDialog)
        {
            var dialog = GetDialog(IDDialog);
            dialog?.FinishDialog();
        }

        public static Dialog GetDialog(in int IDDialog)
        {
            if (_dialogs.IsNotNullAndTryGetValue(IDDialog, out var dialog))
            {
                return dialog;
            }
            return null;
        }
        #endregion

        #region Private Methods
        private static void FinishDialog(Dialog dialog)
        {
            if (_indexManager != null && _dialogs.TryGetKey(dialog, out var IDdialog))
            {
                _dialogs.Remove(IDdialog);
                _pool.Remove(dialog);
                _indexManager.FreeIndex(IDdialog);
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