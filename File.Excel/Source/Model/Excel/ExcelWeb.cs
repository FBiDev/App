using System.Collections.Generic;

namespace App.File.Excel
{
    internal static class ExcelWeb
    {
        public static List<T> Read<T>(string path, ExcelOptions<T> options, bool is64Bits = false) where T : class, new()
        {
            var records = new List<T>();
            return records;
        }
    }
}