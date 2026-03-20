using System.Text.RegularExpressions;

namespace App.Core
{
    public static class Formatter
    {
        public static string SpaceAround(string source)
        {
            return " " + source + " ";
        }

        public static string SingleQuote(string source)
        {
            return "'" + source + "'";
        }

        public static string Quote(string source)
        {
            return "\"" + source + "\"";
        }

        public static string Quote(object source)
        {
            if (source == null)
            {
                source = string.Empty;
            }

            return Quote(source.ToString());
        }

        public static string Brace(string source)
        {
            return "{" + source + "}";
        }

        public static string OnlyNumbers(string text)
        {
            return text != null ? Regex.Replace(text, "[^0-9]", string.Empty) : null;
        }

        public static string CPF(string text)
        {
            if (text == null)
            {
                return null;
            }

            var num = OnlyNumbers(text);

            num = num.PadLeft(11, '0');

            num = string.Format(
                "{0}.{1}.{2}-{3}",
                num.Substring(0, 3),
                num.Substring(3, 3),
                num.Substring(6, 3),
                num.Substring(9, 2));

            return num;
        }
    }
}