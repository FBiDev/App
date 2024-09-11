using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Core
{
    public static class StringExtension
    {
        public static string NormalizePath(this string s)
        {
            return s.Replace('\\', '/');
        }

        public static string PathAddDateTime(this string s)
        {
            var folder = Path.GetDirectoryName(s);
            var name = Path.GetFileNameWithoutExtension(s);
            var ext = Path.GetExtension(s);

            var NowString = DateTime.Now.ToFileName();
            var fullName = name + "(" + NowString + ")" + ext;
            return Path.Combine(folder, fullName).NormalizePath();
        }

        public static string RemoveWhiteSpaces(this string s)
        {
            return string.Join(" ", s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string HtmlRemoveTags(this string s)
        {
            return Regex.Replace(s, @"<[^>]+>|", "").Trim();
        }

        public static short? ToShortNull(this string s)
        {
            return Cast.ToShortNull(s);
        }

        public static float ToFloat(this string s)
        {
            return Cast.ToFloat(s);
        }

        public static TimeSpan? ToTimeSpanNull(this string s)
        {
            return Cast.ToTimeSpanNull(s);
        }

        public static DateTime ToDateTime(this string s)
        {
            return Cast.ToDateTime(s);
        }

        public static DateTime? ToDateTimeNull(this string s)
        {
            return Cast.ToDateTimeNull(s);
        }

        public static decimal ToDecimal(this string s)
        {
            return Cast.ToDecimal(s);
        }

        public static string ToMoney(this string s)
        {
            return Cast.ToMoney(s);
        }

        public static bool ToBoolean(this string s)
        {
            return Cast.ToBoolean(s);
        }

        public static int ToInt(this string s)
        {
            return Cast.ToInt(s);
        }

        public static bool IsEmpty(this string source)
        {
            return source == null || string.IsNullOrWhiteSpace(source);
        }

        public static bool HasValue(this string source)
        {
            return source.IsEmpty() == false;
        }

        public static bool Contains(this string[] source, string toCheck, StringComparison comp)
        {
            if (source == null) return false;
            foreach (var item in source)
            {
                if (item.IndexOf(toCheck, comp) >= 0) return true;
            }
            return false;
        }

        public static bool IsIn(this string source, params string[] values)
        {
            return values.Any(x => x.Equals(source, StringComparison.OrdinalIgnoreCase));
        }

        public static string[] ToArray(this string s, int chunkSize, bool distinct = false, bool removeEmpty = false)
        {
            string input = s;
            chunkSize = chunkSize <= 0 ? 1 : chunkSize;

            string[] result = Enumerable.Range(0, input.Length / chunkSize).Select(i => input.Substring(i * chunkSize, chunkSize)).ToArray();
            result = distinct ? result.Distinct().ToArray() : result;
            result = result.Where(x => x.Trim() != string.Empty).ToArray();

            return result;
        }

        public static string Value(this string[] s)
        {
            return string.Join(string.Empty, s);
        }

        public static bool ContainsExtend(this string source, string value)
        {
            string[] IgnoreSymbols = { ", The:", ",", ":", "'", "-", ".", "+", "/", " " };
            foreach (string symbol in IgnoreSymbols)
            {
                source = source.Replace(symbol, "");
                value = value.Replace(symbol, "");
            }

            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
            //| CompareOptions.IgnoreSymbols

            return index != -1;
        }

        public static bool NotContains(this string source, string value)
        {
            return source.ContainsExtend(value) == false;
        }

        public static bool NotContains(this string source, string[] valueArray)
        {
            foreach (var value in valueArray)
            {
                if (source.ContainsExtend(value)) { return false; }
            }
            return true;
        }

        public static string GetBetween(this string s, string start, string end, bool inclusive = false, bool firstMatch = true, bool singleLine = true)
        {
            string first = firstMatch ? "?" : "";

            string pattern = @"" + Regex.Escape(start) + "(.*" + first + ")" + Regex.Escape(end);
            RegexOptions opt = singleLine ? RegexOptions.Singleline : 0;
            var rgx = new Regex(pattern, opt | RegexOptions.IgnoreCase);

            var match = rgx.Match(s);
            if (match.Success)
            {
                return match.Groups[inclusive ? 0 : 1].Value;
            }
            return string.Empty;
        }

        public static List<string> GetBetweenList(this string s, string start, string end, bool inclusive = false, bool firstMatch = true, bool singleLine = true)
        {
            string first = firstMatch ? "?" : "";

            string pattern = @"" + Regex.Escape(start) + "(.*" + first + ")" + Regex.Escape(end);
            RegexOptions opt = singleLine ? RegexOptions.Singleline : 0;
            var rgx = new Regex(pattern, opt | RegexOptions.IgnoreCase);

            var matchList = rgx.Matches(s);
            var list = matchList.Cast<Match>().Select(match => match.Groups[inclusive ? 0 : 1].Value).ToList();
            return list;
        }

        public static string ReplaceSpecialCharacters(this string s)
        {
            string text = string.Empty;

            foreach (char c in s)
            {
                int len = text.Length;

                foreach (var entry in SpecialCharacters)
                {
                    if (entry.Key.IndexOf(c) != -1)
                    {
                        text += entry.Value;
                        break;
                    }
                }

                if (len == text.Length)
                {
                    text += c;
                }
            }

            return text;
        }

        #region SpecialCharacters
        public static readonly Dictionary<string, string> SpecialCharacters = new Dictionary<string, string>
        {
            { "äæǽ", "ae" },
            { "öœ", "oe" },
            { "ü", "ue" },
            { "Ä", "Ae" },
            { "Ü", "Ue" },
            { "Ö", "Oe" },
            { "ÀÁÂÃÄÅǺĀĂĄǍΑΆẢẠẦẪẨẬẰẮẴẲẶА", "A" },
            { "àáâãåǻāăąǎªαάảạầấẫẩậằắẵẳặа", "a" },
            { "Б", "B" },
            { "б", "b" },
            { "ÇĆĈĊČ", "C" },
            { "çćĉċč", "c" },
            { "Д", "D" },
            { "д", "d" },
            { "ÐĎĐΔ", "Dj" },
            { "ðďđδ", "dj" },
            { "ÈÉÊËĒĔĖĘĚΕΈẼẺẸỀẾỄỂỆЕЭ", "E" },
            { "èéêëēĕėęěέεẽẻẹềếễểệеэ", "e" },
            { "Ф", "F" },
            { "ф", "f" },
            { "ĜĞĠĢΓГҐ", "G" },
            { "ĝğġģγгґ", "g" },
            { "ĤĦ", "H" },
            { "ĥħ", "h" },
            { "ÌÍÎÏĨĪĬǏĮİΗΉΊΙΪỈỊИЫ", "I" },
            { "ìíîïĩīĭǐįıηήίιϊỉịиыї", "i" },
            { "Ĵ", "J" },
            { "ĵ", "j" },
            { "ĶΚК", "K" },
            { "ķκк", "k" },
            { "ĹĻĽĿŁΛЛ", "L" },
            { "ĺļľŀłλл", "l" },
            { "М", "M" },
            { "м", "m" },
            { "ÑŃŅŇΝН", "N" },
            { "ñńņňŉνн", "n" },
            { "ÒÓÔÕŌŎǑŐƠØǾΟΌΩΏỎỌỒỐỖỔỘỜỚỠỞỢО", "O" },
            { "òóôõōŏǒőơøǿºοόωώỏọồốỗổộờớỡởợо", "o" },
            { "П", "P" },
            { "п", "p" },
            { "ŔŖŘΡР", "R" },
            { "ŕŗřρр", "r" },
            { "ŚŜŞȘŠΣС", "S" },
            { "śŝşșšſσςс", "s" },
            { "ȚŢŤŦτТ", "T" },
            { "țţťŧт", "t" },
            { "ÙÚÛŨŪŬŮŰŲƯǓǕǗǙǛŨỦỤỪỨỮỬỰУ", "U" },
            { "ùúûũūŭůűųưǔǖǘǚǜυύϋủụừứữửựу", "u" },
            { "ÝŸŶΥΎΫỲỸỶỴЙ", "Y" },
            { "ýÿŷỳỹỷỵй", "y" },
            { "В", "V" },
            { "в", "v" },
            { "Ŵ", "W" },
            { "ŵ", "w" },
            { "ŹŻŽΖЗ", "Z" },
            { "źżžζз", "z" },
            { "ÆǼ", "AE" },
            { "ß", "ss" },
            { "Ĳ", "IJ" },
            { "ĳ", "ij" },
            { "Œ", "OE" },
            { "ƒ", "f" },
            { "ξ", "ks" },
            { "π", "p" },
            { "β", "v" },
            { "μ", "m" },
            { "ψ", "ps" },
            { "Ё", "Yo" },
            { "ё", "yo" },
            { "Є", "Ye" },
            { "є", "ye" },
            { "Ї", "Yi" },
            { "Ж", "Zh" },
            { "ж", "zh" },
            { "Х", "Kh" },
            { "х", "kh" },
            { "Ц", "Ts" },
            { "ц", "ts" },
            { "Ч", "Ch" },
            { "ч", "ch" },
            { "Ш", "Sh" },
            { "ш", "sh" },
            { "Щ", "Shch" },
            { "щ", "shch" },
            { "ЪъЬь", string.Empty },
            { "Ю", "Yu" },
            { "ю", "yu" },
            { "Я", "Ya" },
            { "я", "ya" },
        };
        #endregion
    }
}