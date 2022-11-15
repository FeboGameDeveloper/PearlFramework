using System;
using System.Collections.Generic;
using System.Text;

namespace Pearl
{
    [Serializable]
    public struct StoryIndex
    {
        public string textName;
        public string path;

        public StoryIndex(string textName, string path)
        {
            this.textName = textName;
            this.path = path;
        }
    }

    [Serializable]
    public struct ContainerString
    {
        public string title;
        public bool useReference;
        [ConditionalField("!@useReference")]
        public string value;
        [ConditionalField("@useReference")]
        public StoryIndex storyIndex;

        public ContainerString(string title, string value)
        {
            this.title = title;
            this.value = value;
            this.useReference = false;
            this.storyIndex = default;
        }

        public ContainerString(string title, StoryIndex value)
        {
            this.title = title;
            this.value = default;
            this.useReference = true;
            this.storyIndex = value;
        }

        public ContainerString(string title, object value)
        {
            this.title = title;
            this.value = value.ToString();
            this.useReference = false;
            this.storyIndex = default;
        }
    }

    public static class StringExtend
    {
        /// <summary>
        /// The Pearl method for comparison two string
        /// </summary>
        public static void SetSimpleString(ref string str)
        {
            if (str != null)
            {
                str = str.Trim();
                str = str.ToLower();
            }
        }

        public static string SetFloat(float newText, uint decimalAfterPoint = 2)
        {
            string d = "";
            if (decimalAfterPoint > 0)
            {
                for (int i = 1; i <= decimalAfterPoint; i++)
                {
                    d += "0";
                }
            }

            return String.Format("{0:0." + d + "}", newText);
        }

        public static string GetEmptyOrString(this string @this)
        {
            return @this != null ? @this : string.Empty;
        }

        public static bool EqualsIgnoreCase(this string a, string b)
        {
            if (a != null && b != null)
            {
                return a.Equals(b, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        public static string Replaces(this string @this, string newStr, params string[] strsToRemove)
        {
            if (@this != null && newStr != null & strsToRemove.IsAlmostSpecificCount())
            {
                foreach (var str in strsToRemove)
                {
                    @this = @this.Replace(str, newStr);
                }
            }

            return @this;
        }

        public static List<string> GetListString(string container, char mark = ';')
        {
            List<string> list = new List<string>();
            string[] aux = container.Split(mark);
            foreach (string s in aux)
            {
                if (s != null && s != string.Empty && s != " ")
                {
                    list.Add(s);
                }
            }
            return list;
        }

        public static Dictionary<string, ContainerString> GetDictionaryString(string container, string equals = "=", string finishMark = ";")
        {
            int startIndex = 0;
            int indexFinish = 0;
            int indexValue = container.IndexOf(equals, startIndex);

            Dictionary<string, ContainerString> dict = new Dictionary<string, ContainerString>();

            while (indexValue != -1 && indexValue - 1 >= 0 && indexValue + 1 < container.Length)
            {
                startIndex = indexValue + 1;

                indexFinish = container.LastIndexOf(finishMark, indexValue);
                indexFinish = indexFinish != -1 ? indexFinish + 1 : 0;
                string title = container.SubstringWithIndex(indexFinish, indexValue - 1);


                indexFinish = container.IndexOf(finishMark, indexValue + 1);
                indexFinish = indexFinish != -1 ? indexFinish - 1 : container.Length - 1;
                string content = container.SubstringWithIndex(indexValue + 1, indexFinish);

                if (title != null && content != null)
                {
                    dict.Update(title, new ContainerString(title.Trim(), content.Trim()));
                }

                indexValue = container.IndexOf(equals, startIndex);
            }

            return dict;
        }

        public static bool ContainsIgnoreCamelCase(this string a, string b)
        {
            if (a != null && b != null)
            {
                return a.Contains(b) || a.Contains(b.CamelCase());
            }
            return false;
        }

        public static string[] Split(this string input, bool clearVoidSpace, params char[] separator)
        {
            if (input != null)
            {
                if (clearVoidSpace)
                {
                    StringBuilder stringBuilder = new StringBuilder(input.Length);
                    foreach (char c in input)
                    {
                        if (c != ' ')
                        {
                            stringBuilder.Append(c);
                        }
                    }
                    input = stringBuilder.ToString();
                }

                string[] subStrings = input.Split(separator, System.StringSplitOptions.RemoveEmptyEntries);
                return subStrings;
            }
            return null;
        }

        public static string InsertAtStartEnd(this string input, string startSubStrimg, string finalSubString)
        {
            if (input == null)
            {
                return null;
            }

            string result = string.Empty;
            result = input.Insert(0, startSubStrimg);
            result = result.Insert(result.Length, finalSubString);
            return result;
        }

        public static string InsertAtMiddle(this string input, string subString, in int initIndex)
        {
            if (input == null && initIndex < input.Length)
            {
                return null;
            }

            string secondPart = initIndex != input.Length - 1 ? input.Substring(initIndex + 1) : "";
            string text = input.SubstringWithIndex(0, initIndex) + subString + secondPart;
            return text;
        }

        public static string SubstringWithIndex(this string container, int startIndex, int finalIndex)
        {
            if (startIndex.IsContains(0, finalIndex))
            {
                return container.Substring(startIndex, finalIndex + 1 - startIndex);
            }

            return null;
        }

        public static string CamelCase(this string container)
        {
            if (container != null && container.Length > 0)
            {
                string firstChar = container[0].ToString().ToUpper();
                return container.Length > 1 ? firstChar + container.Substring(1) : firstChar;
            }
            return container;
        }

        public static string SubString(this string container, char charStart, char charEnd)
        {
            if (container != null)
            {
                int startIndex = container.IndexOf(charStart);

                if (startIndex < 0)
                {
                    return null;
                }

                int finalIndex = container.IndexOf(charEnd, startIndex);

                if (finalIndex < 0)
                {
                    return null;
                }

                return container.SubstringWithIndex(startIndex + 1, finalIndex - 1);
            }

            return null;
        }


        public static string[] SubdivideString(int maxCharacters, char subdivideChar, params string[] collectionString)
        {
            List<string> result = new List<string>(collectionString);

            if (result.IsAlmostSpecificCount())
            {
                for (int index = result.Count - 1; index >= 0; index--)
                {
                    RicorsiveSubdivideArrayString(result, index, maxCharacters, subdivideChar);
                }
            }

            return result.ToArray();
        }

        private static void RicorsiveSubdivideArrayString(List<string> result, int index, in int maxCharacters, in char subdivideChar)
        {
            string text = result[index];

            if (text != null && text.Length > maxCharacters)
            {
                int maxCurrent = text.IndexOf(subdivideChar, maxCharacters);
                if (maxCurrent == -1 || maxCurrent + 1 >= text.Length)
                {
                    maxCurrent = text.Length - 1;
                    return;
                }

                string subString1 = text.Substring(0, maxCurrent + 1);
                string subString2 = text.Substring(maxCurrent + 1);
                result.RemoveAt(index);

                result.Insert(index, subString2);
                result.Insert(index, subString1);

                RicorsiveSubdivideArrayString(result, index + 1, maxCharacters, subdivideChar);
                RicorsiveSubdivideArrayString(result, index, maxCharacters, subdivideChar);
            }
        }
    }
}
