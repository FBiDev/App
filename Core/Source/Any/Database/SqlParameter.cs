using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace App.Core
{
    public class SqlParameter
    {
        public string ParameterName { get; set; }
        public DbType DbType { get; set; }
        public object Value { get; set; }
        public int Size { get; set; }
        public byte Precision { get; set; }
        public byte Scale { get; set; }

        public SqlParameter(string ParameterName, object Value, DbType dbType = DbType.AnsiString, int Size = 0)
        {
            this.ParameterName = ParameterName;

            this.Value = Value ?? DBNull.Value;
            if (this.Value == DBNull.Value) Size = 1;

            if (this.Value != null && this.Value != DBNull.Value)
            {
                var ValueType = Value.GetType();

                switch (ValueType.Name)
                {
                    case "String":
                        DbType = DbType.String;
                        if (string.IsNullOrWhiteSpace((string)Value))
                        { this.Value = DBNull.Value; }
                        else { this.Value = ((string)Value).Trim(); }

                        if (Size == 0) Size = -1;
                        break;
                    case "Int32":
                        DbType = DbType.Int32; break;
                    case "Decimal":
                        DbType = DbType.Decimal;
                        Precision = 10;
                        Scale = 2;
                        break;
                    case "DateTime":
                        DbType = DbType.DateTime2;

                        if (DateTime.MinValue == ((DateTime)Value))
                        {
                            DbType = DbType.AnsiString;
                            this.Value = DBNull.Value;
                        }
                        else
                        {
                            if (dbType == DbType.Date)
                                this.Value = ((DateTime?)Value).ToDB(dbType);
                            else
                                this.Value = ((DateTime?)Value).ToDB();
                        }

                        if (Size == 0) Size = 1;
                        break;
                    case "Boolean":
                        DbType = DbType.Boolean;
                        this.Value = Cast.ToIntNull(Value);
                        break;
                }
            }

            if (Size > 0 || Size == -1) this.Size = Size;
        }

        public SqlParameter(string ParameterName, DbType DbType, object Value, int Size = 0, byte Precision = 0, byte Scale = 0)
        {
            if (DbType == DbType.String && Size == 0) Size = -1;

            this.ParameterName = ParameterName;
            this.DbType = DbType;
            this.Value = Value.ToString();
            this.Size = Size;
            this.Precision = Precision;
            this.Scale = Scale;
        }

        public static string Replace(string query, List<SqlParameter> lParamsReplace)
        {
            foreach (SqlParameter p in lParamsReplace)
            {
                query = ReplaceItem(query, p.Value, p.DbType, p.ParameterName);
            }
            return query;
        }

        public static string Replace(string query, IDataParameterCollection Parameters)
        {
            foreach (IDbDataParameter p in Parameters)
            {
                query = ReplaceItem(query, p.Value, p.DbType, p.ParameterName);
            }
            return query;
        }

        static string ReplaceItem(string query, object value, DbType DbType, string ParameterName)
        {
            string val;

            if (value == null || value == DBNull.Value)
            {
                val = "NULL";
            }
            else
            {
                val = value.ToString();
                val = val.Replace("'", "''");

                switch (DbType)
                {
                    case DbType.String: val = "'" + val + "'"; break;
                    case DbType.DateTime2: val = "'" + val + "'"; break;
                    case DbType.DateTime: val = "'" + val + "'"; break;
                }
            }

            query = Regex.Replace(query, ParameterName + @"\b", val);

            return query;
        }
    }
}