using Pearl.Input;
using Pearl.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.UI.Button;

namespace Pearl
{
    public class InteractiveTextManager : TextManager
    {
        public enum TypeDynamicText { Interactive, Wait }

        #region Inspector

        [Header("Interactive")]

        [SerializeField]
        private TypeDynamicText typeDynamicText = TypeDynamicText.Interactive;
        [SerializeField]
        [ConditionalField("@typeDynamicText == Wait")]
        private float waitBeetweenText = 4f;
        [SerializeField]
        [ConditionalField("@typeDynamicText == Interactive")]
        private DynamicNavbar navbar = null;
        [SerializeField]
        [ConditionalField("@typeDynamicText == Interactive")]
        private string inputName = "Use";
        [SerializeField]
        [ConditionalField("@typeDynamicText == Interactive")]
        private bool usSkipText = true;

        [Header("Subdivide")]
        [SerializeField]
        private bool isAutomaic = true;
        [SerializeField]
        [ConditionalField("!@isAutomaic")]
        private int maxChar = 30;

        #endregion

        #region Private Methods
        private readonly List<string> _piecesOfText = new();
        private int _indexPieces = 0;
        private bool _longText = false;
        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            if (typeDynamicText == TypeDynamicText.Interactive && navbar)
            {
                ButtonClickedEvent unityEvent = new();
                unityEvent?.AddListener(SetElementsOfPieces);
                NavbarInfoElement info = new(unityEvent, "Use", InputDataType.Text);

                navbar.CreateNavbarElements(info);
                navbar.ActiveNavbarElements(false);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PearlInvoke.StopTimer(SetElementsOfPieces);
        }
        #endregion

        #region Public Methods
        public override void SetText()
        {
            ResetAll();

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

            _piecesOfText.Clear();
            SubdivideText(_auxText);

            if (_piecesOfText.IsAlmostSpecificCount(1))
            {
                _longText = true;
            }

            _indexPieces = 0;
            SetElementsOfPieces();
        }
        #endregion

        #region Private Methods
        private void SetElementsOfPieces()
        {
            if (typeDynamicText == TypeDynamicText.Interactive)
            {
                var input = InputManager.Input;
                if (input != null && usSkipText)
                {
                    InputManager.Input.PerformedHandle(inputName, SkipText, ActionEvent.Add, StateButton.Down);
                }

                if (navbar)
                {
                    navbar.ActiveNavbarElements(false);
                }
            }

            ResetText();
            _stampText = string.Empty;

            if (_piecesOfText != null && _piecesOfText.Count > _indexPieces)
            {
                _auxText = _piecesOfText[_indexPieces];

                if (_auxText != null && _auxText.Length != 0 && gameObject.activeInHierarchy && isDictated)
                {
                    StartCoroutine(PrePrintText());
                }
                else
                {
                    PrintText(_auxText);
                    PreFinishText();
                }
            }
            else
            {
                OnFinishText();
            }
        }

        protected override void ResetTextContainer()
        {
            PearlInvoke.StopTimer(SetElementsOfPieces);

            base.ResetTextContainer(); 
        }

        public override void SkipText()
        {
            if (_currentState == StateText.Dictated)
            {
                _skipText = true;
            }
            else if (_currentState == StateText.Waiting)
            {
                _skipText = false;
                PearlInvoke.StopTimer(SetElementsOfPieces);
                SetElementsOfPieces();
            }
        }

        protected override void FinishText()
        {
            _longText = false;

            base.FinishText();
        }

        protected override void PreFinishText()
        {
            _indexPieces++;
            _currentState = StateText.Waiting;
            if (typeDynamicText == TypeDynamicText.Interactive)
            {
                var input = InputManager.Input;
                if (input != null && usSkipText)
                {
                    input.PerformedHandle(inputName, SkipText, ActionEvent.Remove, StateButton.Down);
                }

                if (navbar)
                {
                    navbar.ActiveNavbarElements(true);
                }
            }
            else if (typeDynamicText == TypeDynamicText.Wait)
            {
                PearlInvoke.WaitForMethod(waitBeetweenText, SetElementsOfPieces);
            }
        }

        private void SubdivideText(string text)
        {
            if (textContainer)
            {
                int length = 0;

                while (length >= 0)
                {
                    GetTextWithoutMarks(text, out string textWithoutCustomTag, out string soloText);

                    text = text.Trim();
                    textContainer.text = textWithoutCustomTag;
                    textContainer.ForceMeshUpdate();

                    length = isAutomaic ? textContainer.firstOverflowCharacterIndex : maxChar;

                    bool lastIndex = true;
                    if (length >= 1)
                    {
                        var aux = soloText.LastIndexOf(".", length - 1);
                        if (aux == -1)
                        {
                            aux = soloText.LastIndexOf(";", length - 1);
                            if (aux == -1)
                            {
                                aux = soloText.LastIndexOf(",", length - 1);
                                if (aux == -1)
                                {
                                    aux = soloText.LastIndexOf(" ", length - 1);
                                    if (aux == -1)
                                    {
                                        lastIndex = false;
                                    }
                                }
                            }
                        }

                        if (lastIndex)
                        {
                            length = aux + 2;
                        }

                    }

                    if (length < 0)
                    {
                        string subString = text.SubstringWithIndex(0, text.Length - 1);
                        _piecesOfText.Add(subString);
                    }
                    else
                    {
                        int count = 0;
                        int index;
                        for (index = 0; index < text.Length; index++)
                        {
                            var character = text[index];
                            if (character == startMarkChar && IsNotIgnorePreCharacther(text, index))
                            {
                                index = text.IndexOf(endMarkChar, index);
                            }
                            else
                            {
                                count++;
                                if (count == length)
                                {
                                    break;
                                }
                            }
                        }

                        string firstPart = text.SubstringWithIndex(0, index - 1);
                        string finalPart = text.SubstringWithIndex(index, text.Length - 1);
                        text = finalPart;
                        _piecesOfText.Add(firstPart);
                    }
                }
            }
        }

        private void GetTextWithoutMarks(in string text, out string textWithoutCustomTag, out string soloText)
        {
            textWithoutCustomTag = string.Empty;
            soloText = string.Empty;

            for (int index = 0; index < text.Length; index++)
            {
                char character = text[index];

                if (character == startMarkChar && IsNotIgnorePreCharacther(text, index))
                {
                    GetMark(text, ref index, out MarkInfo markInfo, out int indexFinalMark);

                    if (Array.Exists(unityTags, x => x == markInfo.tag))
                    {
                        textWithoutCustomTag += text.SubstringWithIndex(index, indexFinalMark);
                    }

                    index = indexFinalMark;
                }
                else
                {
                    textWithoutCustomTag += character;

                    if (Char.GetUnicodeCategory(character) != UnicodeCategory.Control)
                    {
                        soloText += character;
                    }
                }
            }
        }
        #endregion
    }
}
