using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows.Forms;
using ExcelToEnumerable;
using ExcelToEnumerable.Exceptions;

namespace App.File.Desktop
{
    public static class Excel
    {
        public static IEnumerable<T> Read<T>(string path, ExcelOptions<T> options) where T : new()
        {
            IEnumerable<T> list = new List<T>();
            try
            {
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    IgnoreColumnsWithoutSet(list, options);

                    list = stream.ExcelToEnumerable<T>(options.OptionsBuilder);

                    SetPropsValues(list, options);
                }
            }
            catch (Exception e)
            {
                var t = e.GetType();

                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + typeof(ExcelToEnumerableInvalidHeaderException).Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return list;
        }

        public static IEnumerable<T> Read<T>(string path, Action<ExcelOptions<T>> options = null) where T : new()
        {
            IEnumerable<T> list = new List<T>();
            try
            {
                var optionsObj = new ExcelOptions<T>();
                options.Invoke(optionsObj);
                IgnoreColumnsWithoutSet(list, optionsObj);

                list = path.ExcelToEnumerable<T>(optionsObj.OptionsBuilder);
                SetPropsValues(list, optionsObj);
            }
            catch (Exception e)
            {
                var t = e.GetType();

                MessageBox.Show(e.Message + Environment.NewLine + Environment.NewLine + typeof(ExcelToEnumerableInvalidHeaderException).Name, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return list;
        }

        private static void IgnoreColumnsWithoutSet<T>(IEnumerable<T> list, ExcelOptions<T> options) where T : new()
        {
            var props = new T().GetType().GetProperties().Where(x => x.CanWrite == false).ToList();
            foreach (var prop in props)
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var property = Expression.Property(parameter, prop);

                // var lambda = Expression.Lambda<Func<T, string>>(property, parameter);
                // var propertyBuilder = options.Property(lambda);
                // propertyBuilder.IgnoreColumn();
                var lambda = Expression.Lambda(property, parameter);

                var methodProperty = typeof(ExcelOptions<T>).GetMethod("Property").MakeGenericMethod(prop.PropertyType);
                var propertyBuilder = methodProperty.Invoke(options, new[] { lambda });

                var configIgnoreColumn = typeof(ExcelProperty<,>).MakeGenericType(typeof(T), prop.PropertyType);
                var methodIgnoreColumn = configIgnoreColumn.GetMethod("IgnoreColumn");

                methodIgnoreColumn.Invoke(propertyBuilder, null);
            }
        }

        private static void SetPropsValues<T>(IEnumerable<T> list, ExcelOptions<T> options) where T : new()
        {
            SetDefaultValues(list, options);
            SetPropsEmptyStrings(list, options);
            TrimProps(list, options);
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
                    var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;

                    return Equals(propertyValue, defaultValue);
                })
                .ToList()
                .ForEach(x =>
                {
                    x.GetType()
                        .GetProperty(item.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        .SetValue(x, item.Value);
                });
            }
        }

        private static void TrimProps<T>(IEnumerable<T> list, ExcelOptions<T> options) where T : new()
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

        private static void SetPropsEmptyStrings<T>(IEnumerable<T> list, ExcelOptions<T> options) where T : new()
        {
            if (options.EmptyStrings)
            {
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
        }
    }
}