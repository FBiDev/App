using System;
using System.Data;

namespace App.Core
{
    internal static class DataRowExtension
    {
        public static string Cell(this DataRow row, int collumn)
        {
            return row.ItemArray[collumn].ToString();
        }

        internal static object CastFieldValue(DataRow row, string column, TypeCode type, object result)
        {
            try
            {
                object columnObj = row[column];
                var columnValue = columnObj.ToString().Trim();

                if (Convert.IsDBNull(columnObj) || string.IsNullOrEmpty(columnValue))
                {
                    return result;
                }

                switch (type)
                {
                    case TypeCode.Object: result = columnObj;
                        break;
                    case TypeCode.String: result = columnValue;
                        break;
                    case TypeCode.Boolean: result = Cast.ToBoolean(columnValue);
                        break;
                    case TypeCode.DateTime: result = (columnObj is DateTime) ? columnObj : Cast.ToDateTimeNull(columnValue);
                        break;
                    case TypeCode.Int16: result = Cast.ToShortNull(columnValue);
                        break;
                    case TypeCode.Int32: result = Cast.ToIntNull(columnValue);
                        break;
                    case TypeCode.Single: result = Cast.ToFloatNull(columnValue);
                        break;
                    case TypeCode.Double: result = Cast.ToDoubleNull(columnValue);
                        break;
                    case TypeCode.Decimal: result = Cast.ToDecimalNull(columnValue);
                        break;
                    default: throw new Exception("Tipo de dado inválido");
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}