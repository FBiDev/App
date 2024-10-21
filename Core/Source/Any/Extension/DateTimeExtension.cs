using System;
using System.Data;

namespace App.Core
{
    public static class DateTimeExtension
    {
        public static string ToString(this DateTime? dt, string format)
        {
            return dt == null ? null : ((DateTime)dt).ToString(format);
        }

        public static string ToDMY(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy");
        }

        public static string ToDMY_TimeShort(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm");
        }

        public static string ToDMY_TimeLong(this DateTime dt)
        {
            return dt.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string ToTimeLong(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }

        public static string ToFileName(this DateTime dt)
        {
            if (dt > DateTime.MinValue)
            {
                return dt.ToString("yyyy-MM-dd_HH-mm");
            }

            return "NULL";
        }

        public static string ToDB(this DateTime? dt, DbType dbType = DbType.DateTime)
        {
            if (dt != null && dt.HasValue && dt > DateTime.MinValue)
            {
                if (dbType == DbType.Date)
                {
                    return dt.Value.ToString("yyyy-MM-dd");
                }

                return dt.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return "NULL";
        }

        public static string ToDBquoted(this DateTime? dt)
        {
            var result = ToDB(dt);
            if (result != "NULL")
            {
                return "'" + result + "'";
            }

            return result;
        }
    }
}