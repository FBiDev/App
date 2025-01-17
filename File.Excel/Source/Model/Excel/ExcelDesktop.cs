using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App.File.Excel
{
    internal static class ExcelDesktop
    {
        public static string SheetName { get; set; }

        public static List<T> Read<T>(string path, ExcelOptions<T> options, bool is64Bits = false) where T : class, new()
        {
            SheetName = string.Empty;

            ExcelOleDb.Excel.VersionType = is64Bits ? ExcelOleDb.Excel.Version.x64_ACE : ExcelOleDb.Excel.Version.x32_Jet;

            path = GetFile(path);

            var records = new List<T>();

            if (string.IsNullOrWhiteSpace(path))
            {
                return records;
            }

            try
            {
                records = ExcelAny.Read(path, options);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Table: " + SheetName + Environment.NewLine + ex.Message);
            }

            return records;
        }

        private static string GetFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                using (OpenFileDialog ofd = new OpenFileDialog
                {
                    Filter = "Excel|*.xls;*.xlsx|Excel 97-2003|*.xls|Excel 2007-365|*.xlsx",
                    ValidateNames = true
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        return ofd.FileName;
                    }
                }
            }

            return filePath;
        }
    }
}