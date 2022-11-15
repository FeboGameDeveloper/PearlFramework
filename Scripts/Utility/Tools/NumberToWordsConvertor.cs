using System;
using System.Collections.Generic;

namespace Pearl
{
    //Classe che converte i numeri in parole in inglese
    static class NumberToWordsConvertor
    {
        public static string NumberToCardinaText(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToCardinaText(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToCardinaText(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToCardinaText(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToCardinaText(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static string NumberToOrdinalText(int number)
        {
            string s = NumberToCardinaText(number);
            return NumberToOrdinalText(s);
        }

        public static string NumberToOrdinalText(string number)
        {
            if (String.IsNullOrEmpty(number)) return number;

            var dict = new Dictionary<string, string>();
            dict.Add("zero", "zeroth");
            dict.Add("nought", "noughth");
            dict.Add("one", "first");
            dict.Add("two", "second");
            dict.Add("three", "third");
            dict.Add("four", "fourth");
            dict.Add("five", "fifth");
            dict.Add("six", "sixth");
            dict.Add("seven", "seventh");
            dict.Add("eight", "eighth");
            dict.Add("nine", "ninth");
            dict.Add("ten", "tenth");
            dict.Add("eleven", "eleventh");
            dict.Add("twelve", "twelfth");
            dict.Add("thirteen", "thirteenth");
            dict.Add("fourteen", "fourteenth");
            dict.Add("fifteen", "fifteenth");
            dict.Add("sixteen", "sixteenth");
            dict.Add("seventeen", "seventeenth");
            dict.Add("eighteen", "eighteenth");
            dict.Add("nineteen", "nineteenth");
            dict.Add("twenty", "twentieth");
            dict.Add("thirty", "thirtieth");
            dict.Add("forty", "fortieth");
            dict.Add("fifty", "fiftieth");
            dict.Add("sixty", "sixtieth");
            dict.Add("seventy", "seventieth");
            dict.Add("eighty", "eightieth");
            dict.Add("ninety", "ninetieth");
            dict.Add("hundred", "hundredth");
            dict.Add("thousand", "thousandth");
            dict.Add("million", "millionth");

            dict.Add("billion", "billionth");
            dict.Add("trillion", "trillionth");
            dict.Add("quadrillion", "quadrillionth");
            dict.Add("quintillion", "quintillionth");

            // rough check whether it's a valid number
            string temp = number.ToLower().Trim().Replace(" and ", " ");
            string[] words = temp.Split(new char[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (!dict.ContainsKey(word)) return number;
            }

            // extract last word
            number = number.TrimEnd().TrimEnd('-');
            int index = number.LastIndexOfAny(new char[] { ' ', '-' });
            string last = number.Substring(index + 1);

            // make replacement and maintain original capitalization
            if (last == last.ToLower())
            {
                last = dict[last];
            }
            else if (last == last.ToUpper())
            {
                last = dict[last.ToLower()].ToUpper();
            }
            else
            {
                last = last.ToLower();
                last = Char.ToUpper(dict[last][0]) + dict[last].Substring(1);
            }

            return number.Substring(0, index + 1) + last;
        }

    }
}