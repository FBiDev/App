using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.File
{
    public class ExcelOptions<T>
    {
        public ExcelOptions(Action<ExcelOptions<T>> options = null)
        {
            OptionsToEnumerable = new ExcelToEnumerable.ExcelOptions<T>();

            PropertyMap = new List<ExcelMap>();

            DefaultValues = new Dictionary<string, object> { };

            options.Invoke(this);
        }

        internal List<ExcelMap> PropertyMap { get; set; }

        internal Dictionary<string, object> DefaultValues { get; private set; }

        internal bool NullToEmptyString { get; private set; }

        internal int StartFromRow { get; private set; }

        internal string SheetName { get; private set; }

        internal ExcelToEnumerable.ExcelOptions<T> OptionsToEnumerable { get; set; }

        public ExcelProperty<T, TProperty> Property<TProperty>(Expression<Func<T, TProperty>> propertyExpression)
        {
            return new ExcelProperty<T, TProperty>(propertyExpression, this);
        }

        public ExcelOptions<T> UsingSheet(string sheetName)
        {
            OptionsToEnumerable.UsingSheet(sheetName);

            SheetName = sheetName;

            return this;
        }

        public ExcelOptions<T> StartingFromRow(int startRow)
        {
            OptionsToEnumerable.OptionsBuilder.StartingFromRow(startRow);

            StartFromRow = startRow;

            return this;
        }

        public ExcelOptions<T> UsingEmptyStrings(bool emptyStrings = true)
        {
            NullToEmptyString = emptyStrings;

            return this;
        }
    }
}