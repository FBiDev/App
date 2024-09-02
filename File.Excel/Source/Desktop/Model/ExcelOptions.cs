using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ExcelToEnumerable;

namespace App.File.Desktop
{
    public class ExcelOptions<T>
    {
        public ExcelOptions()
        {
            DefaultValues = new Dictionary<string, object> { };

            OptionsBuilder = new ExcelToEnumerableOptionsBuilder<T>();
            OptionsBuilder.Build();
        }

        public ExcelOptions(Action<ExcelOptions<T>> options = null)
        {
            DefaultValues = new Dictionary<string, object> { };

            OptionsBuilder = new ExcelToEnumerableOptionsBuilder<T>();
            OptionsBuilder.Build();

            options.Invoke(this);
        }

        internal ExcelToEnumerableOptionsBuilder<T> OptionsBuilder { get; private set; }

        internal Dictionary<string, object> DefaultValues { get; private set; }

        internal bool EmptyStrings { get; private set; }

        public ExcelProperty<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return new ExcelProperty<T, TProperty>(propertyExpression, this);
        }

        public ExcelOptions<T> UsingSheet(string sheetName)
        {
            OptionsBuilder.UsingSheet(sheetName);
            return this;
        }

        public ExcelOptions<T> UsingSheet(int i)
        {
            OptionsBuilder.UsingSheet(i);
            return this;
        }

        public ExcelOptions<T> UsesEmptyStrings(bool emptyStrings = true)
        {
            EmptyStrings = emptyStrings;
            return this;
        }
    }
}