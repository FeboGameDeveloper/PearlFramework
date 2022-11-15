using Pearl.UI;
using System.Collections.Generic;

namespace Pearl.Examples.UI
{
    public class TextFillerExample : Filler<string>
    {
        List<string> text = new();

        TextFillerExample()
        {
            text.Add("1");
            text.Add("2");
            text.Add("3");
            text.Add("4");
            text.Add("5");
            text.Add("6");
            text.Add("7");
            text.Add("8");
            text.Add("9");
            text.Add("10");
        }

        protected override string GetCurrentValue()
        {
            return text[0];
        }

        protected override List<string> Take()
        {
            return text;
        }
    }
}
