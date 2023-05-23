#if INK

using Pearl.Testing;
using Pearl.Events;
using Pearl.Ink;
using Pearl.UI;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Pearl
{
    public struct TextStruct
    {
        public string title;
        public string text;

        public TextStruct(string title, string text)
        {
            this.title = title;
            this.text = text;
        }
    }

    public class Dialog : IReset, IPause
    {
        private enum PartOfDialog { Start, Finish }

        private readonly StoryExtend _story = new();

        private PieceOfStory _currentPiece;
        private Dictionary<string, string> _tags;
        private string _stringTable;

        public event Action<string> EventLocalStart;
        public event Action<string> EventLocalFinish;
        private event Action<TextStruct> ActionText;
        private event Action FinishText;
        private event Action<QuestionInfo> ActionQuestion;

        //enter event
        private const string titleTag = "title";

        private const string eventStartLocal = "eventStartLocal";

        private const string startWait = "startWait";
        private const string startWaitEventTag = "startWaitEvent";
        private const string startEventTag = "startEvent";
        private const string startTriggerEventTag = "startTriggerEvent";
        private const string loopTag = "loop";

        // finish event
        private const string eventFinishLocal = "eventFinishLocal";

        private const string finishWait = "finishWait";
        private const string finishWaitEventTag = "finishWaitEvent";
        private const string finishEventTag = "finishEvent";
        private const string finishTriggerEventTag = "finishTriggerEvent";

        //aux
        private PartOfDialog currentPart;
        private string _title;
        private int _countWaitEvents = 0;
        private int _indexWaitEvents = 0;
        private bool _wait = false;
        private float _timeWait = 0;
        private bool _isQuestion = false;

        private bool _isLoop = false;
        private string _pathLoop = null;

        #region Public Methods
        public void CreateDialog(in StoryIndex storyIndex, in Action<TextStruct> actionText, in Action finishText, in string stringTable, in Action<QuestionInfo> actionQuestion = null)
        {
            if (_story == null)
            {
                return;
            }

            this.ActionText = actionText;
            this.FinishText = finishText;
            this.ActionQuestion = actionQuestion;
            this._stringTable = stringTable;
            this._story.Reset(storyIndex);
        }

        public void ChangePath(in string path)
        {
            if (_story != null)
            {
                _story.ChangePath(path);
                _isLoop = false;
            }
        }

        public void ReadNextText(bool repeat = false)
        {
            currentPart = PartOfDialog.Finish;
            ReadFinishTrigger();
            if (_countWaitEvents == 0 && !_wait)
            {
                Read(repeat);
            }
        }

        public void FinishDialog()
        {
            ResetDialogs();
            FinishText?.Invoke();
        }

        public void SetVar(string var, string newValue)
        {
            if (_story != null)
            {
                _story.SetVar(var, newValue);
            }
        }
        #endregion

        #region Private Methods
        private void Read(bool repeat = false)
        {
            if (_story == null)
            {
                return;
            }

            ResetDialogs();

            currentPart = PartOfDialog.Start;

            bool isContinue = true;

            if (_isLoop && _pathLoop != null)
            {
                ChangePath(_pathLoop);
            }

            if ((!repeat || _currentPiece.text == null) && (!_isLoop || _pathLoop != null))
            {
                isContinue = _story.GetPieceOfStory(out _currentPiece);
            }

            if (isContinue)
            {
                _isQuestion = _currentPiece.choises != null && _currentPiece.choises.Count > 0;
                _tags = _currentPiece.tags;

                ReadStartTrigger();

                if (_countWaitEvents == 0 && !_wait)
                {
                    if (_isQuestion)
                    {
                        ReadQuestion();
                    }
                    else
                    {
                        CallNewText();
                    }
                }
                else if (_wait)
                {
                    if (_isQuestion)
                    {
                        ReadQuestion();
                    }
                    else
                    {
                        PearlInvoke.WaitForMethod(_timeWait, OnFinshWait);
                    }
                }
            }
            else
            {
                FinishDialog();
            }
        }

        private void ReadQuestion()
        {
            var choises = _currentPiece.choises;

            if (choises != null)
            {
                ButtonInfo[] buttons = new ButtonInfo[choises.Count];

                for (int i = 0; i < buttons.Length; i++)
                {
                    var choise = choises[i];
                    buttons[i].text = choise.textChoise;
                    buttons[i].isFirstFocus = i == 0;
                    buttons[i].unityEvent = new();
                    buttons[i].unityEvent.AddNotPersistantListener(() => { OnChoiseAnswer(choise); });
                }

                QuestionInfo questionInfo = new(titleTag, _currentPiece.text, true, buttons);
                ActionQuestion?.Invoke(questionInfo);
            }
        }

        private void OnChoiseAnswer(SimpleChoise simpleChoise)
        {
            if (_story == null)
            {
                return;
            }

            _story.ChooseChoiceIndex(simpleChoise);
            Read();
        }

        private void OnWaitEvents()
        {
            _indexWaitEvents++;

            if (_indexWaitEvents == _countWaitEvents)
            {
                _indexWaitEvents = 0;
                _countWaitEvents = 0;

                if (!_wait)
                {
                    CallNewSection();
                }
                else
                {
                    PearlInvoke.WaitForMethod(_timeWait, OnFinshWait);
                }
            }
        }

        private void OnFinshWait()
        {
            _wait = false;

            if (_indexWaitEvents == _countWaitEvents)
            {
                CallNewSection();
            }
        }

        private void CallNewSection()
        {
            if (currentPart == PartOfDialog.Start)
            {
                CallNewText();
            }
            else
            {
                Read();
            }
        }

        private void ReadStartTrigger()
        {
            _countWaitEvents = 0;
            _indexWaitEvents = 0;
            _wait = false;


            if (_tags == null)
            {
                return;
            }

            _isLoop = _tags.TryGetValue(loopTag, out _pathLoop);

            if (_tags.TryGetValue(titleTag, out _title))
            {
                _title = TextManager.ConvertString(_stringTable, _title, out _);
            }

            if (_tags.TryGetValue(startWait, out string startEventString))
            {
                try
                {
                    _timeWait = float.Parse(startEventString, CultureInfo.InvariantCulture.NumberFormat);
                    _wait = true;
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e);
                }
            }

            if (_tags.TryGetValue(eventStartLocal, out startEventString))
            {
                string[] events = startEventString.Split(true, ',');
                if (events.Length == 1)
                {
                    events = ArrayExtend.CreateArray(events[0], "");
                }

                string evConvert = TextManager.ConvertString(_stringTable, events[0], out _);
                if (events[1] == "")
                {
                    EventLocalStart?.Invoke(evConvert);
                }
                else
                {
                    WaitManager.WaitInvokeEvent<string>(EventLocalStart, OnWaitEvents, evConvert);
                }
            }

            if (_tags.TryGetValue(startEventTag, out startEventString))
            {
                string[] events = startEventString.Split(true, ',');
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEvent(evConvert, PearlEventType.Normal);
                }
            }

            if (_tags.TryGetValue(startTriggerEventTag, out startEventString))
            {
                string[] events = startEventString.Split(true, ',');
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEvent(evConvert, PearlEventType.Trigger);
                }
            }

            if (_tags.TryGetValue(startWaitEventTag, out startEventString))
            {
                string[] events = startEventString.Split(true, ',');
                _countWaitEvents = events.Length;
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEventWithReturn(evConvert, OnWaitEvents);
                }
            }
        }

        private void ReadFinishTrigger()
        {
            _countWaitEvents = 0;
            _indexWaitEvents = 0;

            if (_tags == null)
            {
                return;
            }

            if (_tags.TryGetValue(finishWait, out string finishEventString))
            {
                try
                {
                    _timeWait = float.Parse(finishEventString, CultureInfo.InvariantCulture.NumberFormat);
                    _wait = true;
                }
                catch (Exception e)
                {
                    LogManager.LogWarning(e);
                }
            }

            if (_tags.TryGetValue(eventFinishLocal, out finishEventString))
            {
                string[] events = finishEventString.Split(true, ',');
                if (events.Length == 1)
                {
                    events = ArrayExtend.CreateArray(events[0], "");
                }

                string evConvert = TextManager.ConvertString(_stringTable, events[0], out _);
                if (events[1] == "")
                {
                    EventLocalFinish?.Invoke(evConvert);
                }
                else
                {
                    WaitManager.WaitInvokeEvent<string>(EventLocalFinish, OnWaitEvents, evConvert);
                }
            }

            if (_tags.TryGetValue(finishEventTag, out finishEventString))
            {
                string[] events = finishEventString.Split(true, ',');
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEvent(evConvert, PearlEventType.Normal);
                }
            }

            if (_tags.TryGetValue(finishTriggerEventTag, out finishEventString))
            {
                string[] events = finishEventString.Split(true, ',');
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEvent(evConvert, PearlEventType.Trigger);
                }
            }

            if (_tags.TryGetValue(finishWaitEventTag, out finishEventString))
            {
                string[] events = finishEventString.Split(true, ',');
                _countWaitEvents = events.Length;
                foreach (string ev in events)
                {
                    string evConvert = TextManager.ConvertString(_stringTable, ev, out _);
                    PearlEventsManager.CallEventWithReturn(evConvert, OnWaitEvents);
                }
            }
        }

        private void ResetDialogs()
        {
            _countWaitEvents = 0;
            _indexWaitEvents = 0;
            _isQuestion = false;
        }

        private void CallNewText()
        {
            TextStruct textStruct = new(_title, _currentPiece.text);
            ActionText?.Invoke(textStruct);
        }
        #endregion

        #region Interface
        public void ResetElement()
        {
        }

        public void DisableElement()
        {
            EventLocalStart = null;
            EventLocalFinish = null;
            ResetDialogs();
        }

        public void Pause(bool onPause)
        {
            PearlInvoke.PauseTimer(OnFinshWait, onPause);
        }
        #endregion
    }
}

#endif