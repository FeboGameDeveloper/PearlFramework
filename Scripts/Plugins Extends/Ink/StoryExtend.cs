#if INK

using Ink.Runtime;
using Pearl.Debug;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Ink
{
    #region Struct
    [Serializable]
    public struct SimpleChoise
    {
        public string textChoise;
        public int index;

        public SimpleChoise(string textChoise, int index)
        {
            this.index = index;
            this.textChoise = textChoise;
        }
    }

    [Serializable]
    public struct PieceOfStory
    {
        public string text;
        public Dictionary<string, string> tags;
        public List<SimpleChoise> choises;

        public PieceOfStory(string text, Dictionary<string, string> tags, List<SimpleChoise> choises)
        {
            this.text = text;
            this.tags = tags;
            this.choises = choises;
        }
    }

    [Serializable]
    public struct PieceSimpleOfStory
    {
        public string text;
        public Dictionary<string, string> tags;

        public PieceSimpleOfStory(string text, Dictionary<string, string> tags)
        {
            this.text = text;
            this.tags = tags;
        }
    }
    #endregion

    public class StoryExtend
    {
        private Story _story;

        #region Static
        public static string GetSingleText(in StoryIndex storySource)
        {
            StoryExtend story = new(storySource);
            if (story != null && story.GetPieceOfStory(out var pieceOfStory))
            {
                return pieceOfStory.text;
            }
            return null;
        }

        public static PieceSimpleOfStory GetSinglePiece(in StoryIndex storySource)
        {
            StoryExtend story = new(storySource);
            if (story != null && story.GetPieceOfStory(out var pieceOfStory))
            {
                return new PieceSimpleOfStory(pieceOfStory.text, pieceOfStory.tags);
            }
            return default;
        }
        #endregion

        #region Constructors
        public StoryExtend()
        {
        }

        public StoryExtend(string jsonString)
        {
            Reset(jsonString);
        }

        public StoryExtend(string jsonString, in string path)
        {
            Reset(jsonString, path);
        }

        public StoryExtend(StoryIndex storyIndex)
        {
            Reset(storyIndex);
        }
        #endregion

        #region Public Methods
        public void Reset(StoryIndex storyIndex)
        {
            Reset(storyIndex.textName, storyIndex.path);
        }

        public void Reset(string jsonString, in string path = null)
        {
            var text = GetText(jsonString);

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            _story = new Story(text);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            ChangePath(path);
        }

        public void ChangePath(in string path)
        {
            if (_story)
            {
                try
                {
                    _story.ChoosePathString(path);
                }
                catch (StoryException e)
                {
                    LogManager.LogWarning(e);
                }
            }
        }

        public void ChooseChoiceIndex(SimpleChoise simpleChoise)
        {
            if (_story == null)
            {
                return;
            }

            _story.ChooseChoiceIndex(simpleChoise.index);
        }

        public bool GetPieceOfStory(out PieceOfStory pieceOfStory)
        {
            if (_story == null)
            {
                pieceOfStory = default;
                return false;
            }

            string text;

            if (_story.canContinue)
            {
                text = _story.Continue();
                text = text.Replace("/\\", "\\");

                Dictionary<string, string> complexTags = GetCurrentTags();
                List<SimpleChoise> simpleChoises = new();

                //createQuestion
                if (!_story.canContinue && _story.currentChoices != null && _story.currentChoices.Count != 0)
                {
                    for (int i = 0; i < _story.currentChoices.Count; ++i)
                    {
                        Choice choice = _story.currentChoices[i];
                        simpleChoises.Add(new SimpleChoise(choice.text, choice.index));
                    }
                }


                pieceOfStory = new(text, complexTags, simpleChoises);
                return true;
            }

            pieceOfStory = default;
            return false;
        }

        public int NumDialogues()
        {
            int index;
            for (index = 0; _story.canContinue; index++)
            {
                _story.Continue();
            }
            _story.ResetState();
            return index;
        }

        public bool CanContinue()
        {
            return _story.canContinue;
        }
        #endregion

        #region Private Methods
        private string GetText(in string jsonString)
        {
#if LOCALIZATION
            var dictionaryTexts = AssetManager.LoadAsset<DictionaryTextScriptableObject[]>("DictionaryText");
            if (dictionaryTexts != null)
            {
                TextAsset textAsset;
                foreach (var dictionaryText in dictionaryTexts)
                {
                    textAsset = dictionaryText.Get(jsonString);

                    if (textAsset != null)
                    {
                        return textAsset.text;
                    }
                }
            }
            return null;
#else
            return jsonString;
#endif
        }

        private Dictionary<string, string> GetCurrentTags()
        {
            var currentTags = _story.currentTags;
            Dictionary<string, string> complexTags = new();

            if (currentTags != null && complexTags != null)
            {
                foreach (string tag in currentTags)
                {
                    string[] infoTags = tag.Trim().Split(':');

                    if (infoTags.Length >= 2)
                    {
                        complexTags.Add(infoTags[0], infoTags[1]);
                    }
                    else
                    {
                        complexTags.Add(infoTags[0], null);
                    }
                }
            }

            return complexTags;
        }
#endregion
    }
}

#endif
