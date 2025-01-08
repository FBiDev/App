using System;
using System.Text;

namespace App.Core
{
    public static class EnumExtension
    {
        public static int Value(this Enum e)
        {
            return Convert.ToInt32(e);
        }

        public static string ToStringValue(this Enum e)
        {
            return Convert.ToInt32(e).ToString();
        }

        public static string ToStringHex(this Enum e)
        {
            var value = ((int)(object)e).ToString("X");
            var data = FromHex(value);
            var s = Encoding.UTF8.GetString(data);
            return s;
        }

        public static bool? ToBoolNullable(this Enum e)
        {
            var value = Convert.ToInt32(e);

            switch (value)
            {
                case 0: return false;
                case 1: return true;
                default: return null;
            }
        }

        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", string.Empty);
            byte[] raw = new byte[hex.Length / 2];

            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return raw;
        }
    }
}