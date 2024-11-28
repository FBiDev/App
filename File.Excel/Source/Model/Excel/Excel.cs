using System.Collections.Generic;

namespace App.File.Excel
{
    public static class Excel
    {
        public static List<T> Read<T>(string path, ExcelOptions<T> options, bool is64Bits = false) where T : class, new()
        {
            if (AppType.IsDesktop)
            {
                return ExcelDesktop.Read(path, options, is64Bits);
            }
            else
            {
                return ExcelWeb.Read(path, options, is64Bits);
            }
        }
    }
}