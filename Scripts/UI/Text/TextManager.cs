using Pearl.Editor;
using Pearl.Events;
using Pearl.Input;
using Pearl.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Pearl
{
    public enum StateText { Null, Dictated, Waiting }

    public class TextManager : MonoBehaviour, IPause, IPointerClickHandler
    {
        protected enum PositionMark { Start, End }
        protected enum SpaceBlockText { Null, Local, Global }

        protected struct MarkInfo
        {
            public PositionMark positionMark;
            public string tag;
            public string value;

            public MarkInfo(PositionMark positionMark, string tag, string value)
            {
                this.positionMark = positionMark;
                this.tag = tag;
                this.value = value;
            }
        }

        #region Inspector

#if LOCALIZATION
        [Header("Localization")]

        [SerializeField]
        protected bool isLocalize = true;
        [SerializeField]
        private bool dontVisualizeWithErrorLocalization = true;
        [SerializeField]
        protected string tableString = "";
#endif

        [Header("General")]

        [SerializeField]
        protected bool isDictated = false;
        [SerializeField]
        [ConditionalField("@isDictated")]
        private float speedTextDefault = 0;
        [SerializeField]
        [ReadOnly]
        [ConditionalField("@isDictated")]
        private float currentSpeedText;

        [SerializeField]
        protected TMP_Text textContainer = null;
        [SerializeField]
        private bool initAtStart = true;
        [SerializeField]
        private bool updateAtModifyVar = false;

        [SerializeField]
        private bool waitTextAdEnd = false;
        [SerializeField]
        [ConditionalField("@waitTextAdEnd")]
        private float timeForCharacter = 0.1f;
        [SerializeField]
        private bool deleteTextAtEnd = false;

        [SerializeField]
        private LayoutControllerManager layoutManager = null;
        #endregion

        #region Events
        public UnityEvent OnStartWriteText;
        public UnityEvent OnFinishWriteText;
        public event Action<string, string> OnEvent;
        public event Action<bool> OnWait;

        private static event Action OnModifyVar;
        #endregion

        #region Private Fiedls
        private const char LocalizationChar = '#';
        private const char startVariableChar = '[';
        private const char finalVariableChar = ']';
        protected const char startMarkChar = '<';
        protected const char endMarkChar = '>';
        private const char ignoreNextChar = '/';

        private const string timeTag = "time";
        private const string waitTag = "wait";
        private const string eventTag = "event";
        private const string globalEventTag = "globalEvent";

        protected static readonly string[] unityTags;
        private static bool isInitStatic = true;

        protected StateText _currentState = StateText.Null;

        private float pastSpeedText;
        private float waitIme = 0f;
        private readonly Dictionary<string, string> localStringVars = new();

        private bool isStandardMark = false;
        protected bool _skipText;
        private SpaceBlockText _blockText = SpaceBlockText.Null;
        protected string _stampText = string.Empty;
        private PearlEventType _typeEventCurrent;

        private int _storageIndex = 0;
        private string _waitEvent;
        private string _waitParam;
        protected string _auxText;
        private string _IDForLocalize;
        protected string _rawText;
        private static Dictionary<string, string> stringVars;
        private Stack<float> speedTextHistory;
#if LOCALIZATION
        private bool useLocalization = false;
#endif
        private PearlWaitForSeconds _waitForSeconds;
        #endregion

        #region Property

#if LOCALIZATION
        public string TableString
        {
            set { tableString = value; }
        }

        public StateText CurrentState
        {
            get { return _currentState; }
        }

        public bool Localize
        {
            get { return isLocalize; }
            set
            {
                isLocalize = value;
                if (isLocalize)
                {
                    ChangeActionLocalize(true);
                    SetText();
                }
                else
                {
                    ChangeActionLocalize(false);
                }
            }
        }
#endif
        #endregion

        #region Static

        #region Constructor
        static TextManager()
        {
            stringVars = new Dictionary<string, string>();

            unityTags = new string[45];
            unityTags[0] = "b";
            unityTags[1] = "i";
            unityTags[2] = "size";
            unityTags[3] = "color";
            unityTags[4] = "material";
            unityTags[5] = "quad";
            unityTags[6] = "sprite";
            unityTags[7] = "align";
            unityTags[8] = "alpha";
            unityTags[9] = "cspace";
            unityTags[10] = "font";
            unityTags[11] = "indent";
            unityTags[12] = "line-height";
            unityTags[13] = "line-indent";
            unityTags[14] = "link";
            unityTags[15] = "lowercase";
            unityTags[16] = "uppercase";
            unityTags[17] = "smallcaps";
            unityTags[18] = "margin";
            unityTags[19] = "mark";
            unityTags[20] = "msspace";
            unityTags[21] = "noparse";
            unityTags[22] = "nobr";
            unityTags[23] = "page";
            unityTags[24] = "post";
            unityTags[25] = "space";
            unityTags[26] = "mark";
            unityTags[27] = "s";
            unityTags[28] = "u";
            unityTags[29] = "style";
            unityTags[30] = "sub";
            unityTags[31] = "sup";
            unityTags[32] = "voffset";
            unityTags[33] = "width";
            unityTags[34] = "link";
            unityTags[35] = "allcaps";
            unityTags[36] = "alpha";
            unityTags[37] = "br";
            unityTags[38] = "space";
            unityTags[39] = "font-weight";
            unityTags[40] = "gradient";
            unityTags[41] = "mspace";
            unityTags[42] = "pos";
            unityTags[43] = "rotate";
            unityTags[44] = "strikethrough";
        }
        #endregion

        #region Public Methods
        public static string ConvertString(in string tableString, in string text, out bool thereIsLocalization, bool dontVisualizeWithErrorLocalization = true, TextManager textManager = null)
        {
            string aux = string.Empty;
            thereIsLocalization = false;

            if (text == null)
            {
                return aux;
            }

            for (int index = 0; index < text.Length; index++)
            {
                char character = text[index];

#if LOCALIZATION
                if (character == LocalizationChar)
                {
                    if (IsNotIgnorePreCharacther(text, index))
                    {
                        thereIsLocalization = true;
                        aux += LocalizeSubText(tableString, text, ref index, dontVisualizeWithErrorLocalization, textManager);
                    }
                    else
                    {
                        aux = aux[..^1];
                        aux += character;
                    }
                }
#endif

                if (character == startVariableChar)
                {
                    if (IsNotIgnorePreCharacther(text, index))
                    {
                        string s = GetSubstringVar(text, ref index, textManager);
#if LOCALIZATION
                        if (!string.IsNullOrEmpty(s) && s[0] == LocalizationChar)
                        {
                            s = GetStringLocalize(tableString, s.Substring(1, s.Length - 2));
                        }
#endif
                        aux += s;
                    }
                    else
                    {
                        aux = aux[..^1];
                        aux += character;
                    }
                }
                else
                {
                    aux += character;
                }
            }

            return aux;
        }

#if LOCALIZATION
        public static string GetStringLocalize(string tableString, string ID, bool dontVisualizeWithErrorLocalization = true, Action actionIfError = null)
        {
            return LocalizationManager.Translate(tableString, ID, dontVisualizeWithErrorLocalization, actionIfError);
        }
#endif

        public static void SetVariableString(string variableName, string newValue)
        {
            if (isInitStatic)
            {
                ResetDefaultVariableString();
                isInitStatic = false;
            }

            if (stringVars != null && variableName != null && newValue != null)
            {
                stringVars.Update(variableName, newValue);
                OnModifyVar?.Invoke();
            }
        }

        public static void DeleteVariableString(string variableName)
        {
            if (isInitStatic)
            {
                ResetDefaultVariableString();
                isInitStatic = false;
            }

            if (stringVars != null && variableName != null)
            {
                stringVars.Remove(variableName);
            }
        }

        public static string GetVariableString(string variableName)
        {
            if (variableName == null)
            {
                return string.Empty;
            }

            if (isInitStatic)
            {
                ResetDefaultVariableString();
                isInitStatic = false;
            }

            return stringVars.IsNotNullAndTryGetValue(variableName, out string stringVar) ? stringVar : "!" + variableName;
        }

        public static void ResetDefaultVariableString()
        {
            if (stringVars != null)
            {
                stringVars.Clear();
                var dictonary = AssetManager.LoadAsset<VarDefaultTextManagerScriptableObject>("VarDefafultText");
                if (dictonary)
                {
                    stringVars = new Dictionary<string, string>(dictonary.Dict);
                }
            }
        }

        #endregion

        #region Private Methods
#if LOCALIZATION
        private static string LocalizeSubText(in string tableString, in string text, ref int index, bool dontVisualizeWithErrorLocalization = true, TextManager textManager = null)
        {
            int indexFinalMark = text.IndexOf(LocalizationChar, index + 1);
            string stringToLocalize = text.SubstringWithIndex(index + 1, indexFinalMark - 1);

            if (/*!string.IsNullOrEmpty(stringToLocalize) &&*/ stringToLocalize[0] == startVariableChar)
            {
                index++;
                stringToLocalize = GetSubstringVar(text, ref index, textManager);
            }

            string result = GetStringLocalize(tableString, stringToLocalize);
            index = indexFinalMark;
            return result;
        }
#endif

        private static string GetSubstringVar(in string text, ref int index, TextManager textManager = null)
        {
            int indexPlusOne = index + 1;
            bool isLocal = text.Length > indexPlusOne && text[indexPlusOne] == '[';

            int indexFinalMark = text.IndexOf(finalVariableChar, index);
            if (isLocal)
            {
                indexFinalMark++;
            }

            int lessIndex = isLocal ? 2 : 1;
            string variableName = text.SubstringWithIndex(index + lessIndex, indexFinalMark - lessIndex);


            string value = isLocal && textManager ? textManager.GetLocalVariableString(variableName) : GetVariableString(variableName);
            index = indexFinalMark;
            return value;
        }
        #endregion

        #endregion

        #region Unity Callbacks
        // Start is called before the first frame update
        protected virtual void Awake()
        {
            _waitForSeconds = new();
            speedTextHistory = new();
            currentSpeedText = speedTextDefault;

#if LOCALIZATION
            if (isLocalize)
            {
                ChangeActionLocalize(true);
            }
#endif

            if (textContainer)
            {
                _rawText = _IDForLocalize = textContainer.text;
            }

            if (updateAtModifyVar)
            {
                OnModifyVar += SetText;
            }
        }

        protected void Reset()
        {
            textContainer = GetComponent<TMP_Text>();
        }

        protected virtual void OnDestroy()
        {
#if LOCALIZATION
            if (useLocalization)
            {
                ChangeActionLocalize(false);
            }
#endif

            OnModifyVar -= SetText;
        }

        protected virtual void Start()
        {
            if (initAtStart)
            {
                SetText();
            }
        }
        #endregion

        #region Public Methods

        #region LocalVariables
        public void SetLocalVariableString(string variableName, string newValue)
        {
            if (stringVars != null && variableName != null && newValue != null)
            {
                localStringVars.Update(variableName, newValue);
            }

            if (updateAtModifyVar)
            {
                SetText();
            }
        }

        public void DeleteLocalVariableString(string variableName)
        {
            if (stringVars != null && variableName != null)
            {
                localStringVars.Remove(variableName);
            }
        }

        public string GetLocalVariableString(string variableName)
        {
            if (variableName == null)
            {
                return string.Empty;
            }

            return localStringVars.IsNotNullAndTryGetValue(variableName, out string stringVar) ? stringVar : "!" + variableName;
        }
        #endregion

        public virtual void SkipText()
        {
            _skipText = true;
        }

        public void SetColor(Color color)
        {
            textContainer.color = color;
        }

        public void SetColor(ColorEnum colorEnum, float alpha = 1)
        {
            textContainer.color = ColorExtend.GetColor(colorEnum, alpha);
        }

        public void SetAlpha(float alpha)
        {
            Color color = textContainer.color;
            color.a = alpha;
            SetColor(color);
        }
        #region SetText
        public void SetText(object newText)
        {
            if (newText == null)
            {
                return;
            }

            if (newText is float single)
            {
                SetText(single);
            }
            else
            {
                SetText(newText.ToString());
            }
        }

#if LOCALIZATION
        public void SetText(string newText, bool isLocalize)
        {
            this.isLocalize = isLocalize;
            SetText(newText);
        }
#endif

        public void SetText(string newText)
        {
            this._rawText = _IDForLocalize = newText;
            SetText();
        }

        public void SetText(float newText)
        {
            SetText(newText, 2);
        }

        public void SetText(float newText, uint decimalAfterPoint)
        {
            SetText(StringExtend.SetFloat(newText, decimalAfterPoint));
        }

        public void SetText(int newText)
        {
            SetText(newText.ToString());
        }

        public void AddText(string newTest)
        {
            if (textContainer)
            {
                SetText(textContainer.text + newTest);
            }
        }

        public virtual void SetText()
        {
            ResetAll();

            if (_rawText == null)
            {
                return;
            }

            _rawText = _rawText.Replace("\\n", "\n");

#if LOCALIZATION
            if (isLocalize)
            {
                _auxText = ChangeLocalization();
            }
            else
            {
                _auxText = ConvertString(tableString, _rawText, out bool useLocalization, true, this);
                if (useLocalization)
                {
                    ChangeActionLocalize(true);
                }
            }
#else
            _auxText = ConvertString(null, _rawText, out bool useLocalization, true, this);
#endif

            if (_auxText == null)
            {
                return;
            }

            InvokeStartText();

            if (_auxText.Length != 0 && gameObject.activeInHierarchy && isDictated)
            {
                StartCoroutine(PrePrintText());
            }
            else
            {
                PrintText(_auxText);
                PreFinishText();
            }
        }
        #endregion

        #endregion

        #region Private Methods

        #region AnalizeTest
        protected IEnumerator PrePrintText()
        {
            _currentState = StateText.Dictated;
            if (_auxText != null)
            {
                float _delta = 0;
                for (int index = _storageIndex; index < _auxText.Length; index++)
                {
                    if (ActiveMark(in _auxText, ref index))
                    {
                        if (_blockText != SpaceBlockText.Null)
                        {
                            _storageIndex = index + 1;
                            break;
                        }

                        if (!_skipText && waitIme > 0)
                        {
                            OnWait?.Invoke(true);
                            yield return _waitForSeconds.Reset(waitIme - _delta);

                            OnWait?.Invoke(false);
                            _delta = _waitForSeconds.Delta;
                            waitIme = 0.0f;
                        }
                        continue;
                    }

                    _stampText += _auxText[index];

                    PrintText(_stampText);

                    if (!_skipText && currentSpeedText > 0)
                    {
                        yield return _waitForSeconds.Reset(currentSpeedText - _delta);
                        _delta = _waitForSeconds.Delta;
                    }
                }
            }

            _currentState = StateText.Waiting;

            if (_blockText != SpaceBlockText.Null)
            {
                OnWait?.Invoke(true);

                if (_blockText == SpaceBlockText.Global)
                {
                    PearlEventsManager.CallEventWithReturn(_waitEvent, _typeEventCurrent, OnResumeText);
                }
                else
                {
                    WaitManager.WaitInvokeEvent<string, string>(OnEvent, OnResumeText, _waitEvent, _waitParam);
                }
            }
            else
            {
                PreFinishText();
            }
        }

        private bool ActiveMark(in string text, ref int index)
        {
            char character = text[index];

            if (isStandardMark && character == endMarkChar)
            {
                currentSpeedText = speedTextHistory.Pop();
                isStandardMark = false;
            }
            else if (character == startMarkChar && IsNotIgnorePreCharacther(text, index))
            {
                GetMark(text, ref index, out MarkInfo markInfo, out int indexFinalMark);

                if (Array.Exists(unityTags, x => x == markInfo.tag))
                {
                    pastSpeedText = currentSpeedText;
                    currentSpeedText = 0;
                    speedTextHistory.Push(pastSpeedText);
                    isStandardMark = true;
                }
                else
                {
                    index = indexFinalMark;
                    isStandardMark = false;
                    AnalizeMarkWithValue(markInfo);
                    return true;
                }
            }
            return false;
        }

        protected void GetMark(in string text, ref int index, out MarkInfo markInfo, out int indexFinalMark)
        {
            indexFinalMark = text.IndexOf(endMarkChar, index);
            string mark = text.SubstringWithIndex(index + 1, indexFinalMark - 1);

            PositionMark positionMark;
            string tag;
            string value = null;

            if (mark[0] == ignoreNextChar)
            {
                tag = mark[1..];
                positionMark = PositionMark.End;
            }
            else
            {
                positionMark = PositionMark.Start;
                string[] pairKeyValue = mark.Split(true, '=');
                if (pairKeyValue.Length == 1)
                {
                    tag = mark;
                }
                else
                {
                    tag = pairKeyValue[0];
                    value = pairKeyValue[1];
                }
            }

            markInfo = new MarkInfo(positionMark, tag, value);
        }

        private void AnalizeMarkWithValue(in MarkInfo markInfo)
        {
            if (markInfo.tag.EqualsIgnoreCase(timeTag))
            {
                if (markInfo.positionMark == PositionMark.Start)
                {
                    float value = MathfExtend.ParseFloat(markInfo.value, System.Globalization.CultureInfo.InvariantCulture);
                    pastSpeedText = currentSpeedText;
                    currentSpeedText = value;
                    speedTextHistory.Push(pastSpeedText);
                }
                else
                {
                    try
                    {
                        currentSpeedText = speedTextHistory.Pop();
                    }
                    catch (Exception e)
                    {
                        Testing.LogManager.LogWarning(e);
                        currentSpeedText = 0;
                    }
                }
            }
            else if (markInfo.tag.EqualsIgnoreCase(waitTag))
            {
                waitIme = MathfExtend.ParseFloat(markInfo.value, System.Globalization.CultureInfo.InvariantCulture);
            }
            else if (markInfo.tag.EqualsIgnoreCase(eventTag))
            {
                string[] eventParams = markInfo.value.Trim().Split(true, '-');
                if (eventParams.Length == 1)
                {
                    OnEvent?.Invoke(eventParams[0], "");
                }
                else if (eventParams.Length == 2)
                {
                    if (eventParams[1] == "wait")
                    {
                        _typeEventCurrent = PearlEventType.Normal;
                        _waitEvent = eventParams[0];
                        _blockText = SpaceBlockText.Local;
                    }
                    else
                    {
                        OnEvent?.Invoke(eventParams[0], eventParams[1]);
                    }
                }
                else
                {
                    _typeEventCurrent = PearlEventType.Normal;
                    _waitEvent = eventParams[0];
                    _waitParam = eventParams[1];
                    _blockText = SpaceBlockText.Local;
                }
            }
            else if (markInfo.tag.EqualsIgnoreCase(globalEventTag))
            {
                string[] eventParams = markInfo.value.Trim().Split(true, '-');
                if (eventParams.Length == 1)
                {
                    PearlEventsManager.CallEvent(eventParams[0], PearlEventType.Normal);
                }
                else if (eventParams.Length > 1)
                {
                    if (eventParams[1] == "trigger")
                    {
                        PearlEventsManager.CallEvent(eventParams[0], PearlEventType.Trigger);
                    }
                    else
                    {
                        _typeEventCurrent = PearlEventType.Normal;
                        _waitEvent = eventParams[0];
                        _blockText = SpaceBlockText.Global;
                    }
                }
            }
        }
        #endregion

        protected void InvokeStartText()
        {
            OnStartWriteText?.Invoke();
        }

#if LOCALIZATION
        protected void ChangeActionLocalize(bool localize)
        {
            if (localize)
            {
                if (!useLocalization)
                {
                    PearlEventsManager.AddAction(ConstantStrings.SetNewLanguageEvent, SetText);
                    useLocalization = true;
                }
            }
            else
            {
                PearlEventsManager.RemoveAction(ConstantStrings.SetNewLanguageEvent, SetText);
                useLocalization = false;
            }
        }
#endif

        private void OnResumeText()
        {
            OnWait?.Invoke(false);
            _blockText = SpaceBlockText.Null;
            StartCoroutine(PrePrintText());
        }

        protected virtual void PreFinishText()
        {
            OnFinishText();
        }

        protected virtual void OnFinishText()
        {
            if (waitTextAdEnd && !isDictated)
            {
                _currentState = StateText.Waiting;
                int count = _auxText != null ? _auxText.Trim().Length : 0;
                PearlInvoke.WaitForMethod(count * timeForCharacter, FinishText);
            }
            else
            {
                FinishText();
            }
        }

        protected virtual void FinishText()
        {
            if (deleteTextAtEnd)
            {
                PrintText("");
            }

            ResetTextContainer();
            OnFinishWriteText?.Invoke();
        }

        protected static bool IsNotIgnorePreCharacther(in string text, in int index)
        {
            if (index == 0)
            {
                return true;
            }

            return text[index - 1] != ignoreNextChar;
        }

#if LOCALIZATION
        protected string ChangeLocalization()
        {
            string result = GetStringLocalize(tableString, _IDForLocalize, dontVisualizeWithErrorLocalization);
            return isLocalize && result != null ? result : _rawText;
        }
#endif

        protected void PrintText(in string currentText)
        {
            if (textContainer)
            {
                textContainer.SetText(currentText, true);
                textContainer.ForceMeshUpdate();
            }

            if (layoutManager)
            {
                layoutManager.UpdateLayout();
            }
        }

        public void ResetAll()
        {
            ResetText();
            ResetTextContainer();
        }

        protected void ResetText()
        {
            PrintText(string.Empty);
        }

        protected virtual void ResetTextContainer()
        {
            _auxText = null;
            _stampText = string.Empty;
            _storageIndex = 0;
            _skipText = false;
            waitIme = 0.0f;
            StopAllCoroutines();
            PearlInvoke.StopTimer(OnFinishText);
            PearlInvoke.StopTimer(FinishText);
            _currentState = StateText.Null;

            speedTextHistory?.Clear();

            currentSpeedText = speedTextDefault;
        }
        #endregion

        #region Interface Methods
        public void Pause(bool onPause)
        {
            _waitForSeconds?.Pause(onPause);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (textContainer == null || !textContainer.textInfo.linkInfo.IsAlmostSpecificCount()
               || eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            // First, get the index of the link clicked. Each of the links in the text has its own index.
            var linkIndex = TMP_TextUtilities.FindIntersectingLink(textContainer, PointerExtend.GetScreenPosition(), null);

            if (linkIndex < 0)
            {
                return;
            }

            // As the order of the links can vary easily (e.g. because of multi-language support),
            // you need to get the ID assigned to the links instead of using the index as a base for our decisions.
            // you need the LinkInfo array from the textInfo member of the TextMesh Pro object for that.
            var linkId = textContainer.textInfo.linkInfo[linkIndex].GetLinkID();

            if (OnlineExtend.CheckURLValid(linkId))
            {
                Application.OpenURL(linkId);
            }
        }
        #endregion
    }
}
