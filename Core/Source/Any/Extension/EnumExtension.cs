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

        private static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            byte[] raw = new byte[hex.Length / 2];

            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }
    }
}