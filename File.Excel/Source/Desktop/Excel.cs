using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace App.File.Desktop
{
    public static class Excel
    {
        public static List<T> Read<T>(string path, ExcelOptions<T> options, bool is64Bits = false) where T : class, new()
        {
            ExcelOleDb.Excel.versionType = is64Bits ? ExcelOleDb.Excel.Version.ACE_x86_x64 : ExcelOleDb.Excel.Version.Jet_x86;

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
                MessageBox.Show(ex.Message);
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