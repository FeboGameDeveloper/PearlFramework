using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pearl.Multitags
{
    public struct TagForValue
    {
        public string tag;
        public string key;

        public TagForValue(string tag, string key)
        {
            this.tag = tag;
            this.key = key;
        }
    }

    [DisallowMultipleComponent]
    public class MultitagsComponent : PearlBehaviour, IReset
    {
        #region Inspector fields
        [SerializeField]
        private List<string> _tags = default;
        [SerializeField]
        private StringStringDictionary _tagsValues = default;
        #endregion

        #region Private fields
        private bool _isAdded = false;
        #endregion

        #region Properties
        public List<string> ListTags { get { return _tags; } }
        #endregion

        #region Button
        [InspectorButton("ValidateTags")]
        [SerializeField]
        private bool validate;
        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            ValidateTags();
            ResetElement();
        }

        protected override void OnDestroy()
        {
            DisableElement();
        }
        #endregion

        #region Public Methods
        public void AddTags(params string[] newTags)
        {
            foreach (var tag in newTags)
            {
                AddTag(tag);
            }
        }

        public void AddTags(params TagForValue[] newTags)
        {
            foreach (var tuple in newTags)
            {
                AddTag(tuple.tag);

                if (_tagsValues == null)
                {
                    _tagsValues = new StringStringDictionary();
                }

                _tagsValues.Update(tuple.tag.ToLower(), tuple.key);
            }
        }

        public void RemoveTags(params string[] tags)
        {
            foreach (var tag in tags)
            {
                RemoveTag(tag);
                _tagsValues?.Remove(tag.ToLower());
            }
        }

        public void ClearTags()
        {
            if (ListTags != null)
            {
                ListTags.Clear();
                _tagsValues.Clear();
            }
        }

        public bool HasTags(bool only, params string[] tags)
        {
            if ((tags == null && ListTags == null) || (tags.Length == 0 && ListTags.Count == 0))
            {
                return true;
            }

            if (ListTags == null || ListTags.Count == 0)
            {
                return false;
            }


            if (tags == null || tags.Length == 0)
            {
                return !only;
            }


            if (only && tags.Length != ListTags.Count)
            {
                return false;
            }

            bool aux;
            foreach (var tag in tags)
            {
                aux = SearchTag(tag);
                if (!aux)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetTagValue(string tag)
        {
            if (tag != null && _tagsValues != null)
            {
                _tagsValues.IsNotNullAndTryGetValue(tag, out string result);
                return result;
            }
            return null;
        }

        public int GetCountTag()
        {
            return ListTags.Count;
        }
        #endregion

        #region Private Methods
        private void ValidateTags()
        {
            ModyfyStringsInLowerCase();
            _tags?.AddRange(_tagsValues.Keys.ToList());
            DeleteDuplicate();

            if (_tags != null)
            {
                _tags.Sort();
            }
        }

        private bool SearchTag(in string tag)
        {
            return tag != null && ListTags != null && ListTags.BinarySearch(tag.ToLower()) >= 0;
        }

        private void ModyfyStringsInLowerCase()
        {
            if (_tags != null)
            {
                for (int index = 0; index < _tags.Count; index++)
                {
                    _tags[index] = _tags[index].ToLower();
                }
            }

            if (_tagsValues != null)
            {
                for (int i = _tagsValues.Count - 1; i >= 0; i--)
                {
                    var key = _tagsValues.Keys.Get(i);
                    var value = _tagsValues[key];
                    _tagsValues.Remove(key);

                    key = key.ToLower();
                    _tagsValues.Add(key, value);
                }
            }
        }

        private void RemoveTag(in string tag)
        {
            if (ListTags != null)
            {
                ListTags.Remove(tag.ToLower());
            }
        }

        private void AddTag(string newTag)
        {
            newTag = newTag.ToLower();

            if (_tags == null)
            {
                _tags = new List<string>();
            }

            if (!SearchTag(newTag))
            {
                ListTags.AddInSort(newTag);
            }
        }

        private void DeleteDuplicate()
        {
            _tags = _tags?.Distinct()?.ToList();
        }
        #endregion

        #region Interface Methods
        public void ResetElement()
        {
            if (!_isAdded)
            {
                MultiTagsManager.NewElement(this);
                _isAdded = true;
            }
        }

        public void DisableElement()
        {
            if (_isAdded)
            {
                MultiTagsManager.RemoveElement(this);
                _isAdded = false;
            }
        }
        #endregion
    }
}
