using System.Globalization;

namespace App.Core
{
    public static class IntExtension
    {
        public static string ToDB(this int? value)
        {
            if (value.HasValue)
            {
                return value.ToString();
            }

            return StringValue.DBNull;
        }

        public static bool ToBool(this int value)
        {
            return Cast.ToBoolean(value.ToString());
        }

        public static string ToCurrency(this decimal d)
        {
            return Cast.ToMoney(d.ToString());
        }

        public static string ToNumber(this decimal value, int decimals = 2, bool fixedDecimals = false, bool customLanguage = false)
        {
            if (fixedDecimals)
            {
                return string.Format(customLanguage ? LanguageManager.LanguageNumbers : CultureInfo.CurrentCulture, "{0:N" + decimals + "}", value);
            }

            int precision = value % 1 == 0 ? 0 : (value * 10) % 1 == 0 ? 1 : 2;

            if (precision > decimals)
            {
                precision = decimals;
            }

            return string.Format(customLanguage ? LanguageManager.LanguageNumbers : CultureInfo.CurrentCulture, "{0:N" + precision + "}", value);
        }

        public static string ToNumber(this int? value, bool customLanguage = false)
        {
            return ToNumber((decimal)value, 0, customLanguage);
        }

        public static string ToNumber(this int value, bool customLanguage = false)
        {
            return ToNumber(value, 0, customLanguage);
        }

        public static string ToNumber(this float value, bool customLanguage = false)
        {
            return ToNumber(Cast.ToDecimal(value.ToString()), 2, customLanguage);
        }

        public static bool Contains(this int source, params int[] values)
        {
            foreach (var item in values)
            {
                if (source == item)
                {
                    return true;
                }
            }

            return false;
        }
    }
}