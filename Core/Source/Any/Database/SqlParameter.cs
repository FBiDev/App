using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace App.Core
{
    public class SqlParameter
    {
        public SqlParameter(string name, object value, DbType type = DbType.AnsiString, int size = 0)
        {
            this.ParameterName = name;
            this.Value = value ?? DBNull.Value;

            if (this.Value == DBNull.Value)
            {
                size = 1;
            }

            if (this.Value != null && this.Value != DBNull.Value)
            {
                var valueType = value.GetType();

                switch (valueType.Name)
                {
                    case TypeName.ByteArray:
                    case TypeName.FileStream:
                        DbType = DbType.Binary;

                        if (size == 0)
                        {
                            size = -1;
                        }

                        break;
                    case TypeName.String:
                        DbType = DbType.String;

                        if (string.IsNullOrWhiteSpace((string)value))
                        {
                            this.Value = DBNull.Value;
                        }
                        else
                        {
                            this.Value = ((string)value).Trim();
                        }

                        if (size == 0)
                        {
                            size = -1;
                        }

                        break;
                    case TypeName.Int16: DbType = DbType.Int16;
                        break;
                    case TypeName.Int32: DbType = DbType.Int32;
                        break;
                    case TypeName.Decimal:
                        DbType = DbType.Decimal;
                        Precision = 10;
                        Scale = 2;
                        break;
                    case TypeName.DateTime:
                        DbType = DbType.DateTime2;

                        if (DateTime.MinValue == ((DateTime)value))
                        {
                            DbType = DbType.AnsiString;
                            this.Value = DBNull.Value;
                        }
                        else
                        {
                            if (type == DbType.Date)
                                this.Value = ((DateTime?)value).ToDB(type);
                            else
                                this.Value = ((DateTime?)value).ToDB();
                        }

                        if (size == 0)
                        {
                            size = 1;
                        }

                        break;
                    case TypeName.Boolean:
                        DbType = DbType.Boolean;
                        this.Value = Cast.ToIntNull(value);
                        break;
                }
            }

            if (size > 0 || size == -1)
            {
                this.Size = size;
            }
        }

        public SqlParameter(string name, DbType type, object value, int size = 0, byte precision = 0, byte scale = 0)
        {
            if (type == DbType.String && size == 0)
            {
                size = -1;
            }

            this.ParameterName = name;
            this.DbType = type;
            this.Value = value.ToString();
            this.Size = size;
            this.Precision = precision;
            this.Scale = scale;
        }

        public string ParameterName { get; set; }

        public DbType DbType { get; set; }

        public object Value { get; set; }

        public int Size { get; set; }

        public byte Precision { get; set; }

        public byte Scale { get; set; }

        public static List<SqlParameter> CreateList(params KeyValuePair<string, object>[] values)
        {
            var source = new List<SqlParameter>();

            foreach (var kvp in values)
            {
                source.Add(new SqlParameter(kvp.Key, kvp.Value));
            }

            return source;
        }

        public static string Replace(List<SqlParameter> parameters, string query)
        {
            foreach (SqlParameter p in parameters)
            {
                query = ReplaceItem(p.ParameterName, p.Value, p.DbType, query);
            }

            return query;
        }

        public static string Replace(IDataParameterCollection parameters, string query)
        {
            foreach (IDbDataParameter p in parameters)
            {
                query = ReplaceItem(p.ParameterName, p.Value, p.DbType, query);
            }

            return query;
        }

        public override string ToString()
        {
            return DbType.ToString() + Formatter.SpaceAround(ParameterName) + Value;
        }

        private static string ReplaceItem(string parameterName, object value, DbType type, string query)
        {
            string val;

            if (value == null || value == DBNull.Value)
            {
                val = StringValue.DBNull;
            }
            else
            {
                val = value.ToString();
                val = val.Replace("'", "''");

                switch (type)
                {
                    case DbType.String: val = Formatter.SingleQuote(val);
                        break;
                    case DbType.DateTime2: val = Formatter.SingleQuote(val);
                        break;
                    case DbType.DateTime: val = Formatter.SingleQuote(val);
                        break;
                }
            }

            query = Regex.Replace(query, parameterName + @"\b", val);

            return query;
        }
    }
}