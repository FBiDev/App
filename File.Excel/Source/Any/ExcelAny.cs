using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace App.File
{
    internal static class ExcelAny
    {
        private enum ExcelType
        {
            None,
            ExcelOleDb,
            ExcelToEnumerable
        }

        internal static string RowNumberColumn
        {
            get
            {
                return "RowNumber";
            }
        }

        public static List<T> Read<T>(string path, ExcelOptions<T> options) where T : class, new()
        {
            var recordsTable = new List<T>();

            switch (CheckFileFormat(path))
            {
                case ExcelType.ExcelOleDb:
                    recordsTable = ReadExcelOleDb<T>(path, options);
                    break;
                case ExcelType.ExcelToEnumerable:
                    recordsTable = ReadExcelToEnumerable<T>(path, options);
                    break;
            }

            SetDefaultValues<T>(recordsTable, options);
            SetPropsEmptyStrings<T>(recordsTable, options);
            TrimProps(recordsTable, options);

            return recordsTable;
        }

        private static ExcelType CheckFileFormat(string path)
        {
            var extension = Path.GetExtension(path).ToLower();

            if (extension == ".xls")
            {
                return ExcelType.ExcelOleDb;
            }
            else if (extension == ".xlsx")
            {
                return ExcelType.ExcelToEnumerable;
            }

            return ExcelType.None;
        }

        private static List<T> ReadExcelToEnumerable<T>(string path, ExcelOptions<T> options) where T : class, new()
        {
            var records = ExcelToEnumerable.Excel.Read<T>(path, options.OptionsToEnumerable).ToList();

            return records;
        }

        private static List<T> ReadExcelOleDb<T>(string path, ExcelOptions<T> options) where T : class, new()
        {
            var records = new List<T>();

            var table = ExcelOleDb.Excel.Read(path, options);

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();

                options.PropertyMap.ForEach(map =>
                {
                    SetExcelMap<T>(map, obj, row);
                });

                records.Add(obj);
            }

            return records;
        }

        private static void SetExcelMap<T>(ExcelMap map, T obj, DataRow row)
        {
            var prop = obj.GetType().GetProperties().Where(p => p.Name == map.Property).First();
            var rowValue = row[map.ColumnName];

            // CustomMapping
            if (map.CustomMap != null)
            {
                var customValue = map.CustomMap(rowValue);
                prop.SetValue(obj, customValue);
            }
            else
            {
                DateTime parsedDate;
                double parsedDouble;

                rowValue = string.IsNullOrWhiteSpace(rowValue.ToString()) ? DBNull.Value : rowValue;

                // NamedColumns
                switch (Type.GetTypeCode(prop.PropertyType))
                {
                    case TypeCode.DateTime:
                        var dateValue = DateTime.TryParse(rowValue.ToString(), out parsedDate) ? parsedDate : DateTime.MinValue;
                        prop.SetValue(obj, dateValue);
                        break;

                    case TypeCode.Double:
                        var doubleValue = double.TryParse(rowValue.ToString(), out parsedDouble) ? parsedDouble : 0;
                        prop.SetValue(obj, doubleValue);
                        break;

                    case TypeCode.String:
                        prop.SetValue(obj, rowValue is DBNull ? null : Convert.ChangeType(rowValue, prop.PropertyType));
                        break;

                    default:
                        prop.SetValue(obj, rowValue is DBNull ? null : rowValue);
                        break;
                }
            }
        }

        private static void SetDefaultValues<T>(IEnumerable<T> list, ExcelOptions<T> options) where T : new()
        {
            foreach (var item in options.DefaultValues)
            {
                list.Where(x =>
                {
                    var property = x.GetType().GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (property == null)
                    {
                        return false;
                    }

                    var propertyValue = property.GetValue(x);
                    propertyValue = propertyValue != null && string.IsNullOrWhiteSpace(propertyValue.ToString()) ? null : propertyValue;
                    var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;

                    return Equals(propertyValue, defaultValue);
                })
                .ToList()
                .ForEach(x =>
                {
                    var prop = x.GetType().GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    prop.SetValue(x, Convert.ChangeType(item.Value, prop.PropertyType));
                });
            }
        }

        private static void SetPropsEmptyStrings<T>(IEnumerable<T> list, ExcelOptions<T> options)
        {
            if (options.NullToEmptyString == false)
            {
                return;
            }

            foreach (var obj in list)
            {
                var stringProperties = obj.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(string) && p.GetValue(obj) == null);

                foreach (var prop in stringProperties)
                {
                    prop.SetValue(obj, string.Empty);
                }
            }
        }

        private static void TrimProps<T>(IEnumerable<T> list, ExcelOptions<T> options)
        {
            foreach (var obj in list)
            {
                var stringProperties = obj.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType == typeof(string) && p.GetValue(obj) != null && p.CanWrite);

                foreach (var prop in stringProperties)
                {
                    prop.SetValue(obj, prop.GetValue(obj).ToString().Trim());
                }
            }
        }
    }
}