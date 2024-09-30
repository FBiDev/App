using System;
using System.Linq.Expressions;
using App.Core;

namespace App.File
{
    public class ExcelProperty<T, TProperty>
    {
        public ExcelProperty(Expression<Func<T, TProperty>> propertyExpression, ExcelOptions<T> options)
        {
            OptionsBuilder = options;
            PropertyName = propertyExpression.PropertyName();

            PropertyExpression = propertyExpression;
        }

        private Expression<Func<T, TProperty>> PropertyExpression { get; set; }

        private string PropertyName { get; set; }

        private ExcelOptions<T> OptionsBuilder { get; set; }

        public ExcelOptions<T> Map(string columnName)
        {
            OptionsBuilder.OptionsToEnumerable.Property(PropertyExpression).MapsToColumnNamed(columnName);

            var mapedProperty = OptionsBuilder.PropertyMap.Find(x => x.Property == PropertyName);

            if (mapedProperty != null)
            {
                mapedProperty.ColumnName = columnName;
            }
            else
            {
                OptionsBuilder.PropertyMap.Add(new ExcelMap
                {
                    Property = PropertyName,
                    ColumnName = columnName
                });
            }

            return OptionsBuilder;
        }

        public ExcelOptions<T> CustomMapping(Func<object, object> mapping)
        {
            OptionsBuilder.OptionsToEnumerable.Property(PropertyExpression).UsesCustomMapping(mapping);

            var mapedProperty = OptionsBuilder.PropertyMap.Find(x => x.Property == PropertyName);

            if (mapedProperty != null)
            {
                mapedProperty.CustomMap = mapping;
            }
            else
            {
                OptionsBuilder.PropertyMap.Add(new ExcelMap
                {
                    Property = PropertyName,
                    ColumnName = PropertyName,
                    CustomMap = mapping
                });
            }

            return OptionsBuilder;
        }

        public ExcelOptions<T> RowNumber()
        {
            OptionsBuilder.OptionsToEnumerable.Property(PropertyExpression).MapsToRowNumber();

            var mapedProperty = OptionsBuilder.PropertyMap.Find(x => x.Property == PropertyName);

            if (mapedProperty != null)
            {
                mapedProperty.ColumnName = ExcelAny.RowNumberColumn;
            }
            else
            {
                OptionsBuilder.PropertyMap.Add(new ExcelMap
                {
                    Property = PropertyName,
                    ColumnName = ExcelAny.RowNumberColumn
                });
            }

            return OptionsBuilder;
        }

        public ExcelOptions<T> Required()
        {
            OptionsBuilder.OptionsToEnumerable.Property(PropertyExpression).RequiredColumn();

            var mapedProperty = OptionsBuilder.PropertyMap.Find(x => x.Property == PropertyName);

            if (mapedProperty != null)
            {
                mapedProperty.Required = true;
            }
            else
            {
                OptionsBuilder.PropertyMap.Add(new ExcelMap
                {
                    Property = PropertyName,
                    ColumnName = PropertyName,
                    Required = true
                });
            }

            return OptionsBuilder;
        }

        public ExcelOptions<T> Ignore()
        {
            OptionsBuilder.OptionsToEnumerable.Property(PropertyExpression).IgnoreColumn();

            return OptionsBuilder;
        }

        public ExcelOptions<T> DefaultValue(TProperty value)
        {
            OptionsBuilder.DefaultValues.Remove(PropertyName);
            OptionsBuilder.DefaultValues.Add(PropertyName, value);

            return OptionsBuilder;
        }
    }
}