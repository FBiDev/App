using System;
using System.Data;

namespace App.Core.Desktop
{
    public static class DataRowExtension
    {
        static object CastFieldValue<T>(this DataRow row, string column)
        {
            object result = null;

            var type = typeof(T).TypeCode();
            if (type == TypeCode.String) { result = string.Empty; }

            if (!row.Table.Columns.Contains(column))
            {
                DebugManager.AddError(Messages.ColumnError(column));
                return result;
            }

            return Core.DataRowExtension.CastFieldValue(row, column, type, result);
        }

        public static T Value<T>(this DataRow row, string column)
        {
            var result = row.CastFieldValue<T>(column);

            return result == null ? default(T) : (T)result;
        }

        public static T? ValueNullable<T>(this DataRow row, string column) where T : struct
        {
            var result = row.CastFieldValue<T>(column);

            return (T?)(result == null ? result : (T)result);
        }
    }
}