#if INK

using Pearl.Events;
using UnityEngine;

namespace Pearl.Ink
{
    public class TextManagerWithSourceInk : MonoBehaviour
    {
        #region Inspector Fields
        [SerializeField]
        private bool initAtStart = true;

        [SerializeField]
        [ConditionalField("@initAtStart")]
        private StoryIndex storySource = default;

        [SerializeField]
        private TextManager textManager = null;
        #endregion

        public StoryIndex StorySource { set { storySource = value; } }

        #region Unity CallBacks
        private void Awake()
        {
            PearlEventsManager.AddAction(ConstantStrings.SetNewLanguageEvent, SetText);
        }

        private void OnDestroy()
        {
            PearlEventsManager.RemoveAction(ConstantStrings.SetNewLanguageEvent, SetText);
        }

        private void Start()
        {
            if (initAtStart)
            {
                SetText();
            }
        }

        private void Reset()
        {
            textManager = GetComponent<TextManager>();
        }
        #endregion

        #region Private Methods
        public void SetText()
        {
            if (textManager)
            {
                textManager.SetText(StoryExtend.GetSingleText(storySource));
            }
        }

        public void SetText(StoryIndex storySource)
        {
            StorySource = storySource;
            SetText();
        }
        #endregion
    }
}
#endif