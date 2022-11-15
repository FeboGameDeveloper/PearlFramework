using System;
using UnityEngine;

namespace Pearl
{
    [Serializable]
    public struct TextData
    {
        public bool isBold;
        public bool isItalic;
        public int fontSize;
        public Color color;
    }

    public static class TextExtend
    {
        public static string InsertSize(string input, int size)
        {
            string markSize = "<size=" + size + ">";
            string markSizeEnd = "</size>";

            return StringExtend.InsertAtStartEnd(input, markSize, markSizeEnd);
        }

        public static string InsertColor(string input, Color color)
        {
            string colorHexadecimal = ColorUtility.ToHtmlStringRGBA(color).ToLower();

            string markSize = "<color=/#" + colorHexadecimal + ">";
            string markSizeEnd = "</color>";

            return StringExtend.InsertAtStartEnd(input, markSize, markSizeEnd);
        }

        public static string InsertBold(string input)
        {
            string markSize = "<b>";
            string markSizeEnd = "</b>";

            return StringExtend.InsertAtStartEnd(input, markSize, markSizeEnd);
        }

        public static string InsertItalic(string input)
        {
            string markSize = "<i>";
            string markSizeEnd = "</i>";

            return StringExtend.InsertAtStartEnd(input, markSize, markSizeEnd);
        }

        public static string InsertTextData(string input, TextData textData)
        {
            if (input == null)
            {
                return null;
            }

            input = InsertSize(input, textData.fontSize);
            input = InsertColor(input, textData.color);

            if (textData.isBold)
            {
                input = InsertBold(input);
            }

            if (textData.isItalic)
            {
                input = InsertItalic(input);
            }

            return input;

        }
    }
}
