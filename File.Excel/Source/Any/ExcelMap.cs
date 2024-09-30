using System;

namespace App.File
{
    internal class ExcelMap
    {
        public string Property { get; set; }

        public string ColumnName { get; set; }

        public Func<object, object> CustomMap { get; set; }

        public bool Required { get; set; }
    }
}