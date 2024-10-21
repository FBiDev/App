using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;

namespace App.Core
{
    internal static class DatabaseDao
    {
        #region " _Load "
        public static T Load<T, U>(DataTable table, Func<DataRow, U> mapFunction) where T : IList<U>, new()
        {
            var list = new T();

            foreach (DataRow row in table.Rows)
            {
                var obj = mapFunction(row);
                list.Add(obj);
            }

            return list;
        }

        public static T Load<T>(DataTable table, Type dataRow) where T : IList, new()
        {
            var list = new T();
            var listItemType = list.GetType().GetGenericArguments().Single();
            var listItemProperties = TypeDescriptor.GetProperties(listItemType);

            foreach (DataRow row in table.Rows)
            {
                var listItem = Activator.CreateInstance(listItemType);

                foreach (PropertyDescriptor prop in listItemProperties)
                {
                    var propData = (FieldAttribute)prop.Attributes[typeof(FieldAttribute)];

                    if (propData == null || string.IsNullOrWhiteSpace(propData.Name))
                    {
                        continue;
                    }

                    bool propIsNullable = Nullable.GetUnderlyingType(prop.PropertyType) != null;
                    Type propType = prop.PropertyType;

                    Type dataRowType = dataRow;
                    MethodInfo dataRowMethod;

                    if (propIsNullable)
                    {
                        dataRowMethod = dataRowType.GetMethods().FirstOrDefault(m =>
                            m.Name == "ValueNullable" && m.IsGenericMethod).MakeGenericMethod(propType.GetNotNullableType());
                    }
                    else
                    {
                        dataRowMethod = dataRowType.GetMethods().FirstOrDefault(m =>
                            m.Name == "Value" && m.IsGenericMethod).MakeGenericMethod(propType);
                    }

                    prop.SetValue(listItem, dataRowMethod.Invoke(null, new object[] { row, propData.Name }));
                }

                list.Add(listItem);
            }

            return list;
        }
        #endregion
    }
}