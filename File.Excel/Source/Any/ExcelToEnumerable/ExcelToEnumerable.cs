using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using ExcelToEnumerable;
using ExcelToEnumerable.Exceptions;

namespace App.File.ExcelToEnumerable
{
    internal static class Excel
    {
        public static IEnumerable<T> Read<T>(string path, ExcelOptions<T> options) where T : new()
        {
            IEnumerable<T> list = new List<T>();
            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    IgnoreColumnsWithoutSet(list, options);

                    list = stream.ExcelToEnumerable<T>(options.OptionsBuilder);
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
    }
}