using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace App.Core
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        public static bool IsNotEmpty(this string source)
        {
            return !string.IsNullOrWhiteSpace(source);
        }

        public static string Value(this string[] s)
        {
            return string.Join(string.Empty, s);
        }

        public static bool IsIn(this string source, params string[] values)
        {
            return values.Any(x => x.Equals(source, StringComparison.OrdinalIgnoreCase));
        }

        public static string RemoveWhiteSpaces(this string s)
        {
            return string.Join(" ", s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public static string HtmlDecode(this string s)
        {
            return WebUtility.HtmlDecode(s);
        }

        public static string HtmlEncode(this string s)
        {
            return WebUtility.HtmlEncode(s);
        }

        public static string RemoveHtmlTags(this string s)
        {
            return Regex.Replace(s, @"<[^>]+>|", string.Empty).Trim();
        }

        public static bool ToBoolean(this string s)
        {
            return Cast.ToBoolean(s);
        }

        public static int ToInt(this string s)
        {
            return Cast.ToInt(s);
        }

        public static short? ToShortNull(this string s)
        {
            return Cast.ToShortNull(s);
        }

        public static float ToFloat(this string s)
        {
            return Cast.ToFloat(s);
        }

        public static decimal ToDecimal(this string s)
        {
            return Cast.ToDecimal(s);
        }

        public static string ToMoney(this string s)
        {
            return Cast.ToMoney(s);
        }

        public static DateTime ToDateTime(this string s)
        {
            return Cast.ToDateTime(s);
        }

        public static DateTime? ToDateTimeNull(this string s)
        {
            return Cast.ToDateTimeNull(s);
        }

        public static TimeSpan? ToTimeSpanNull(this string s)
        {
            return Cast.ToTimeSpanNull(s);
        }

        public static byte[] ToBytesFromHex(this string source)
        {
            var length = source.Length;
            var bytes = new byte[length / 2];

            for (var i = 0; i < length; i += 2)
            {
                if (i < 0 || i + 2 > source.Length) { continue; }

                bytes[i / 2] = Cast.ToByte(source.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string[] ToArray(this string s, int chunkSize, bool distinct = false, bool removeEmpty = false)
        {
            var input = s;
            chunkSize = chunkSize <= 0 ? 1 : chunkSize;

            var result = Enumerable.Range(0, input.Length / chunkSize).Select(i => input.Substring(i * chunkSize, chunkSize)).ToArray();
            result = distinct ? result.Distinct().ToArray() : result;
            result = result.Where(x => x.Trim() != string.Empty).ToArray();

            return result;
        }

        public static string NormalizePath(this string s)
        {
            return s.Replace('\\', '/');
        }

        public static string PathAddDateTime(this string s)
        {
            var folder = Path.GetDirectoryName(s);
            if (folder == null) { return null; }

            var name = Path.GetFileNameWithoutExtension(s);
            var ext = Path.GetExtension(s);

            var nowString = DateTime.Now.ToFileName();
            var fullName = name + "(" + nowString + ")" + ext;
            return Path.Combine(folder, fullName).NormalizePath();
        }

        public static bool Contains(this string[] source, string toCheck, StringComparison comp)
        {
            return source != null && source.Any(item => item.IndexOf(toCheck, comp) >= 0);
        }

        public static bool ContainsExtend(this string source, string value)
        {
            string[] ignoreSymbols = { ", The:", ",", ":", "'", "-", ".", "+", "/", " " };
            foreach (var symbol in ignoreSymbols)
            {
                source = source.Replace(symbol, string.Empty);
                value = value.Replace(symbol, string.Empty);
            }

            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf(
                source, value, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace); // | CompareOptions.IgnoreSymbols

            return index != -1;
        }

        public static bool NotContains(this string source, string value)
        {
            return source.ContainsExtend(value) == false;
        }

        public static bool NotContains(this string source, string[] valueArray)
        {
            return valueArray.All(value => !source.ContainsExtend(value));
        }

        public static string GetBetween(this string s, string start, string end, bool inclusive = false, bool firstMatch = true, bool singleLine = true)
        {
            var first = firstMatch ? "?" : string.Empty;

            var pattern = Regex.Escape(start) + "(.*" + first + ")" + Regex.Escape(end);
            var opt = singleLine ? RegexOptions.Singleline : 0;

            var rgx = new Regex(pattern, opt | RegexOptions.IgnoreCase);
            var match = rgx.Match(s);

            return match.Success ? match.Groups[inclusive ? 0 : 1].Value : string.Empty;
        }

        public static List<string> GetBetweenList(this string s, string start, string end, bool inclusive = false, bool firstMatch = true, bool singleLine = true)
        {
            var first = firstMatch ? "?" : string.Empty;

            var pattern = Regex.Escape(start) + "(.*" + first + ")" + Regex.Escape(end);
            var opt = singleLine ? RegexOptions.Singleline : 0;
            var rgx = new Regex(pattern, opt | RegexOptions.IgnoreCase);

            var matchList = rgx.Matches(s);
            var list = matchList.Cast<Match>().Select(match => match.Groups[inclusive ? 0 : 1].Value).ToList();
            return list;
        }

        public static string ReplaceSpecialCharacters(this string s)
        {
            var text = string.Empty;

            foreach (var c in s)
            {
                var len = text.Length;

                foreach (var entry in StringValue.SpecialCharacters)
                {
                    if (entry.Key.IndexOf(c) == -1) { continue; }

                    text += entry.Value;
                    break;
                }

                if (len == text.Length)
                {
                    text += c;
                }
            }

            return text;
        }
    }
}