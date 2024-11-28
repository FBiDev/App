using System;
using System.Linq.Expressions;
using ExcelToEnumerable;

namespace App.File.ExcelToEnumerable
{
    internal class ExcelOptions<T>
    {
        public ExcelOptions()
        {
            OptionsBuilder = new ExcelToEnumerableOptionsBuilder<T>();
            OptionsBuilder.Build();
        }

        public ExcelOptions(Action<ExcelOptions<T>> options = null)
        {
            OptionsBuilder = new ExcelToEnumerableOptionsBuilder<T>();
            OptionsBuilder.Build();

            options.Invoke(this);
        }

        internal ExcelToEnumerableOptionsBuilder<T> OptionsBuilder { get; private set; }

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
    }
}