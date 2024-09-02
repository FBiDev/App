using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using App.Core;
using ExcelToEnumerable;

namespace App.File.Desktop
{
    public class ExcelProperty<T, TProperty>
    {
        public ExcelProperty(Expression<Func<T, TProperty>> propertyExpression, ExcelOptions<T> options)
        {
            OptionsBuilder = options;
            PropertyName = propertyExpression.GetName();
            Property = options.OptionsBuilder.Property<TProperty>(propertyExpression);
        }

        private ExcelOptions<T> OptionsBuilder { get; set; }

        private IExcelPropertyConfiguration<T, TProperty> Property { get; set; }

        private string PropertyName { get; set; }

        public ExcelOptions<T> IgnoreColumn()
        {
            Property.IgnoreColumn();
            return OptionsBuilder;
        }

        public ExcelOptions<T> RequiredColumn()
        {
            Property.RequiredColumn();
            return OptionsBuilder;
        }

        public ExcelOptions<T> DefaultValue(object value)
        {
            OptionsBuilder.DefaultValues.Remove(PropertyName);
            OptionsBuilder.DefaultValues.Add(PropertyName, value);
            return OptionsBuilder;
        }

        public ExcelOptions<T> NotNull()
        {
            Property.NotNull();
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapsToColumnNamed(string columnName)
        {
            Property.MapsToColumnNamed(columnName);
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapsToColumnLetter(string columnLetter)
        {
            Property.MapsToColumnLetter(columnLetter);
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapsToColumnNumber(int i)
        {
            Property.MapsToColumnNumber(i);
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapFromColumns(IEnumerable<string> columnNames)
        {
            Property.MapFromColumns(columnNames);
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapFromColumns(params string[] columnNames)
        {
            Property.MapFromColumns(columnNames);
            return OptionsBuilder;
        }

        public ExcelOptions<T> MapsToRowNumber()
        {
            Property.MapsToRowNumber();
            return OptionsBuilder;
        }

        public ExcelOptions<T> OptionalColumn(bool isOptional = true)
        {
            Property.OptionalColumn(isOptional);
            return OptionsBuilder;
        }

        public ExcelOptions<T> ShouldBeGreaterThan(double minValue)
        {
            Property.ShouldBeGreaterThan(minValue);
            return OptionsBuilder;
        }

        public ExcelOptions<T> ShouldBeLessThan(double maxValue)
        {
            Property.ShouldBeLessThan(maxValue);
            return OptionsBuilder;
        }

        public ExcelOptions<T> ShouldBeOneOf(IEnumerable<TProperty> minValue)
        {
            Property.ShouldBeOneOf(minValue);
            return OptionsBuilder;
        }

        public ExcelOptions<T> ShouldBeOneOf(params TProperty[] permissiableValues)
        {
            Property.ShouldBeOneOf(permissiableValues);
            return OptionsBuilder;
        }

        public ExcelOptions<T> ShouldBeUnique()
        {
            Property.ShouldBeUnique();
            return OptionsBuilder;
        }

        public ExcelOptions<T> UsesCustomMapping(Func<object, object> mapping)
        {
            Property.UsesCustomMapping(mapping);
            return OptionsBuilder;
        }
    }
}