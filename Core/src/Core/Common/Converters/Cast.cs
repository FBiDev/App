using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;

namespace App.Core
{
    public static class Cast
    {
        public static int ToInt(string value)
        {
            int result;
            int.TryParse(value, out result);
            return result;
        }

        public static int ToInt(object obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return ToInt(obj.ToString());
        }

        public static int? ToIntNull(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            value = value.Trim().ToLower();

            if (value == "true")
            {
                return 1;
            }

            if (value == "false")
            {
                return 0;
            }

            int result;
            if (!int.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static int? ToIntNull(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            return ToIntNull(obj.ToString());
        }

        public static short ToShort(string value)
        {
            short result;
            short.TryParse(value, out result);
            return result;
        }

        public static short? ToShortNull(string value)
        {
            short result;
            if (string.IsNullOrEmpty(value) || !short.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static float ToFloat(string value)
        {
            float result;
            value = ToNumber(value);
            float.TryParse(value, out result);
            return result;
        }

        public static float? ToFloatNull(string value)
        {
            float result;
            if (string.IsNullOrEmpty(value) || !float.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static double ToDouble(string value)
        {
            double result;
            value = ToNumber(value);
            double.TryParse(value, out result);
            return result;
        }

        public static double? ToDoubleNull(string value)
        {
            double result;
            if (string.IsNullOrEmpty(value) || !double.TryParse(value, out result))
            {
                return null;
            }

            return result;
        }

        public static decimal ToDecimal(string value)
        {
            value = ToNumber(value);
            decimal result;
            decimal.TryParse(value, out result);
            return result;
        }

        public static decimal? ToDecimalNull(string value)
        {
            decimal result;
            if (string.IsNullOrEmpty(value) || !decimal.TryParse(value, out result))
            {
                return null;
            }

            result /= 1.000000000000000000000000000000000m;
            return result;
        }

        public static string ToMoney(string value)
        {
            decimal result;
            if (decimal.TryParse(value, out result))
            {
                value = string.Format("{0:C2}", result);
            }

            return value;
        }

        public static bool ToBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            value = value.Trim().ToLower();

            if (value == "1" || value == "true" ||
                value == "y" || value == "yes" ||
                value == "s" || value == "sim")
            {
                return true;
            }

            return false;
        }

        public static DateTime ToDateTime(string value)
        {
            DateTime result;
            DateTime.TryParse(value, LanguageManager.CultureBrazil, DateTimeStyles.None, out result);

            return result;
        }

        public static DateTime? ToDateTimeNull(string value)
        {
            DateTime result;
            if (string.IsNullOrEmpty(value) || !DateTime.TryParse(value, LanguageManager.CultureBrazil, DateTimeStyles.None, out result))
            {
                return null;
            }

            return result;
        }

        public static TimeSpan? ToTimeSpanNull(string value)
        {
            TimeSpan time;

            if (value != string.Empty && value.Length >= 2)
            {
                value = value.Replace("h", string.Empty).Replace(":", string.Empty).Insert(2, ":");
                if (value.Length == 3)
                {
                    value = value.Insert(3, "00");
                }

                TimeSpan validTime;
                if (TimeSpan.TryParse(value, out validTime))
                {
                    time = validTime;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

            return time;
        }

        public static T ToObject<T>(object obj) where T : new()
        {
            if (obj != null)
            {
                return (T)obj;
            }

            // return default(T);
            return new T();
        }

        public static Icon ToIco(Image img, Size size)
        {
            Icon icon;
            using (var msImg = new MemoryStream())
            {
                var msIco = new MemoryStream();

                img.Save(msImg, ImageFormat.Png);
                var bw = new BinaryWriter(msIco);

                bw.Write((short)0);           // 0-1 reserved
                bw.Write((short)1);           // 2-3 image type, 1 = icon, 2 = cursor
                bw.Write((short)1);           // 4-5 number of images
                bw.Write((byte)size.Width);   // 6 image width
                bw.Write((byte)size.Height);  // 7 image height
                bw.Write((byte)0);            // 8 number of colors
                bw.Write((byte)0);            // 9 reserved
                bw.Write((short)0);           // 10-11 color planes
                bw.Write((short)32);          // 12-13 bits per pixel
                bw.Write((int)msImg.Length);  // 14-17 size of image data
                bw.Write(22);                 // 18-21 offset of image data
                bw.Write(msImg.ToArray());    // write image data
                bw.Flush();
                bw.Seek(0, SeekOrigin.Begin);
                icon = new Icon(msIco);

                bw.Dispose();
            }

            return icon;
        }

        private static string ToNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                string cs = LanguageManager.CurrencySymbol;
                string gs = LanguageManager.CurrencyGroupSeparator;
                value = value.Replace(cs, string.Empty).Replace(gs, string.Empty).Replace("'", string.Empty).Trim();
            }

            return value;
        }
    }
}