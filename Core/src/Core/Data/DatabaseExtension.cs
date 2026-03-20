using System;
using System.Collections.Generic;
using System.Data;

namespace App.Core
{
    public static class DatabaseExtension
    {
        public static string GetState(this IDbCommand cmd)
        {
            return cmd.Connection.State + ", " + cmd.Connection.Database + ", Params[" + cmd.Parameters.Count + "]";
        }

        public static List<T> ProcessRows<T>(this DataTable source, Action<DataRow, List<T>> action)
        {
            var list = new List<T>();

            foreach (DataRow row in source.Rows)
            {
                action(row, list);
            }

            return list;
        }

        public static void AddRange(this List<SqlParameter> source, params KeyValuePair<string, object>[] values)
        {
            foreach (var kvp in values)
            {
                source.Add(new SqlParameter(kvp.Key, kvp.Value));
            }
        }
    }
}