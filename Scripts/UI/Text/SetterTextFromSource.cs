using TMPro;
using UnityEngine;

namespace Pearl
{
    public class SetterTextFromSource : MonoBehaviour
    {
        [SerializeField]
        private Transform source = null;
        [SerializeField]
        private TMP_Text textManager = null;

        private Transform _textSource;
        private PartAlignment _currentAlignment;
        private SemiAxis2DEnum _currentPosition;

        private enum PartAlignment { Top, Middle, Bottom, Baseline, Midline, Capline }

        // Start is called before the first frame update
        protected void Start()
        {
            if (textManager)
            {
                _textSource = textManager.transform;
                string alignment = textManager.alignment.ToString();

                if (alignment.Contains("Top"))
                {
                    _currentAlignment = PartAlignment.Top;
                }
                else if (alignment.Contains("Bottom"))
                {
                    _currentAlignment = PartAlignment.Bottom;
                }
                else if (alignment.Contains("Baseline"))
                {
                    _currentAlignment = PartAlignment.Baseline;
                }
                else if (alignment.Contains("Midline"))
                {
                    _currentAlignment = PartAlignment.Midline;
                }
                else if (alignment.Contains("Capline"))
                {
                    _currentAlignment = PartAlignment.Capline;
                }
                else
                {
                    _currentAlignment = PartAlignment.Middle;
                }

                _currentPosition = CalculatePosition();
                SetText();
            }
        }

        // Update is called once per frame
        protected void FixedUpdate()
        {
            var aux = CalculatePosition();
            if (aux != _currentPosition)
            {
                _currentPosition = aux;
                SetText();
            }
        }

        private SemiAxis2DEnum CalculatePosition()
        {
            if (source != null && _textSource != null)
            {
                Vector3 delta = _textSource.position - source.position;
                return delta.x > 0 ? SemiAxis2DEnum.Left : SemiAxis2DEnum.Right;
            }

            return default;
        }

        private void SetText()
        {
            if (textManager != null && _textSource != null)
            {
                TextAlignmentOptions options = GetAlignment();
                textManager.alignment = options;
            }
        }


        private TextAlignmentOptions GetAlignment()
        {
            if (_currentPosition == SemiAxis2DEnum.Left)
            {
                switch (_currentAlignment)
                {
                    case PartAlignment.Baseline:
                        return TextAlignmentOptions.BaselineLeft;
                    case PartAlignment.Bottom:
                        return TextAlignmentOptions.BottomLeft;
                    case PartAlignment.Capline:
                        return TextAlignmentOptions.CaplineLeft;
                    case PartAlignment.Middle:
                        return TextAlignmentOptions.Left;
                    case PartAlignment.Midline:
                        return TextAlignmentOptions.MidlineLeft;
                    default:
                        return TextAlignmentOptions.TopLeft;
                }
            }
            else
            {
                switch (_currentAlignment)
                {
                    case PartAlignment.Baseline:
                        return TextAlignmentOptions.BaselineRight;
                    case PartAlignment.Bottom:
                        return TextAlignmentOptions.BottomRight;
                    case PartAlignment.Capline:
                        return TextAlignmentOptions.CaplineRight;
                    case PartAlignment.Middle:
                        return TextAlignmentOptions.Right;
                    case PartAlignment.Midline:
                        return TextAlignmentOptions.MidlineRight;
                    default:
                        return TextAlignmentOptions.TopRight;
                }
            }
        }
    }
}