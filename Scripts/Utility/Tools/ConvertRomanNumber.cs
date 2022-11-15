using Pearl.Debug;

namespace Pearl
{
    public static class ConvertRomanNumber
    {
        // Function to convert decimal to Roman Numerals 
        public static string GetRomanNumber(int number)
        {
            string s = string.Empty;
            int i = 0;

            // If number entered is not valid 
            if (number <= 0)
            {
                LogManager.LogWarning("Invalid number");
                return null;
            }

            // TO convert decimal number to 
            // roman numerals 
            while (number != 0)
            {
                // If base value of number is 
                // greater than 1000 
                if (number >= 1000)
                {
                    // Add 'M' number/1000 times after index i 
                    i = Digit('M', number / 1000, i, ref s);
                    number %= 1000;
                }

                // If base value of number is greater 
                // than or equal to 500 
                else if (number >= 500)
                {
                    // To add base symbol to the character array 
                    if (number < 900)
                    {

                        // Add 'D' number/1000 times after index i 
                        i = Digit('D', number / 500, i, ref s);
                        number %= 500;
                    }

                    // To handle subtractive notation in case 
                    // of number having digit as 9 and adding 
                    // corresponding base symbol 
                    else
                    {

                        // Add C and M after index i/. 
                        i = SubDigit('C', 'M', i, ref s);
                        number %= 100;
                    }
                }

                // If base value of number is greater 
                // than or equal to 100 
                else if (number >= 100)
                {
                    // To add base symbol to the character array 
                    if (number < 400)
                    {
                        i = Digit('C', number / 100, i, ref s);
                        number %= 100;
                    }

                    // To handle subtractive notation in case 
                    // of number having digit as 4 and adding 
                    // corresponding base symbol 
                    else
                    {
                        i = SubDigit('C', 'D', i, ref s);
                        number %= 100;
                    }
                }

                // If base value of number is greater
                // than or equal to 50 
                else if (number >= 50)
                {

                    // To add base symbol to the character array 
                    if (number < 90)
                    {
                        i = Digit('L', number / 50, i, ref s);
                        number %= 50;
                    }

                    // To handle subtractive notation in case
                    // of number having digit as 9 and adding 
                    // corresponding base symbol 
                    else
                    {
                        i = SubDigit('X', 'C', i, ref s);
                        number %= 10;
                    }
                }

                // If base value of number is greater 
                // than or equal to 10 
                else if (number >= 10)
                {

                    // To add base symbol to the character array 
                    if (number < 40)
                    {
                        i = Digit('X', number / 10, i, ref s);
                        number %= 10;
                    }

                    // To handle subtractive notation in case of 
                    // number having digit as 4 and adding 
                    // corresponding base symbol 
                    else
                    {
                        i = SubDigit('X', 'L', i, ref s);
                        number %= 10;
                    }
                }

                // If base value of number is greater
                // than or equal to 5 
                else if (number >= 5)
                {
                    if (number < 9)
                    {
                        i = Digit('V', number / 5, i, ref s);
                        number %= 5;
                    }

                    // To handle subtractive notation in case 
                    // of number having digit as 9 and adding 
                    // corresponding base symbol 
                    else
                    {
                        i = SubDigit('I', 'X', i, ref s);
                        number = 0;
                    }
                }

                // If base value of number is greater 
                // than or equal to 1 
                else if (number >= 1)
                {
                    if (number < 4)
                    {
                        i = Digit('I', number, i, ref s);
                        number = 0;
                    }

                    // To handle subtractive notation in 
                    // case of number having digit as 4 
                    // and adding corresponding base symbol 
                    else
                    {
                        i = SubDigit('I', 'V', i, ref s);
                        number = 0;
                    }
                }
            }
            return s;
        }

        // To add corresponding base symbols in the 
        // array to handle cases which follow subtractive 
        // notation. Base symbols are added index 'i'. 
        private static int SubDigit(char num1, char num2,
                                 int i, ref string s)
        {
            s += num1;
            s += num2;
            return i;
        }

        // To add symbol 'ch' n times after index i in c[] 
        private static int Digit(char ch, int n, int i, ref string s)
        {
            for (int j = 0; j < n; j++)
            {
                s += ch;
            }
            return i;
        }
    }

    // This code is contributed by Rajput-Ji
}
