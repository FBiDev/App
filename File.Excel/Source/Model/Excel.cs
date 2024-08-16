using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExcelToEnumerable;

namespace App.File
{
    public static class Excel
    {
        public static IEnumerable<T> Read<T>(string path, ExcelOptions<T> builder) where T : new()
        {
            return path.ExcelToEnumerable<T>(builder.options);
        }

        public static IEnumerable<T> Read<T>(string path, Action<ExcelOptions<T>> builderAction = null) where T : new()
        {
            var builder = new ExcelOptions<T>();
            builderAction.Invoke(builder);

            return path.ExcelToEnumerable<T>(builder.options);
        }
    }

    public class ExcelOptions<T>
    {
        internal ExcelToEnumerableOptionsBuilder<T> options { get; set; }

        public ExcelOptions()
        {
            options = new ExcelToEnumerableOptionsBuilder<T>();
            options.Build();
            options.AllPropertiesOptionalByDefault();
            options.IgnoreColumsWithoutMatchingProperties();
        }

        public ExcelOptions<T> Build()
        {
            options.Build();
            return this;
        }

        public ExcelOptions<T> AllPropertiesOptionalByDefault()
        {
            options.AllPropertiesOptionalByDefault();
            return this;
        }

        public ExcelOptions<T> IgnoreColumsWithoutMatchingProperties()
        {
            options.IgnoreColumsWithoutMatchingProperties();
            return this;
        }

        public ExcelProperty<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return new ExcelProperty<T, TProperty>(propertyExpression, this);
        }
    }

    public class ExcelProperty<T, TProperty>
    {
        IExcelPropertyConfiguration<T, TProperty> Property { get; set; }
        ExcelOptions<T> OptionsBuilder { get; set; }

        public ExcelProperty(Expression<Func<T, TProperty>> propertyExpression, ExcelOptions<T> excelOptions)
        {
            OptionsBuilder = excelOptions;
            Property = OptionsBuilder.options.Property<TProperty>(propertyExpression);
        }

        public ExcelOptions<T> Ignore()
        {
            Property.Ignore();
            return OptionsBuilder;
        }

        public ExcelOptions<T> IsRequired()
        {
            Property.IsRequired();
            return OptionsBuilder;
        }

        public ExcelOptions<T> UsesColumnNamed(string columnName)
        {
            Property.UsesColumnNamed(columnName);
            return OptionsBuilder;
        }

        public ExcelOptions<T> UsesCustomMapping(Func<object, object> customMapping)
        {
            Property.UsesCustomMapping(customMapping);
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

        public ExcelOptions<T> Optional(bool isOptional = true)
        {
            Property.Optional(isOptional);
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

        public ExcelOptions<T> UsesColumnLetter(string columnLetter)
        {
            Property.UsesColumnLetter(columnLetter);
            return OptionsBuilder;
        }

        public ExcelOptions<T> UsesColumnNumber(int i)
        {
            Property.UsesColumnNumber(i);
            return OptionsBuilder;
        }
    }
}