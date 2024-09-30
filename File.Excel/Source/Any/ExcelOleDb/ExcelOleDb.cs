using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace App.File.ExcelOleDb
{
    internal static class Excel
    {
        public enum Version
        {
            Jet_x86,
            ACE_x86_x64
        }

        private static DataColumn[] columnNames;
        private static int totalRecords = 0;
        private static int topEmptyRecords = 0;
        private static int bottomEmptyRecords = 0;

        public static Version versionType { get; set; }

        [SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "hide")]
        public static DataTable Read<T>(string file, ExcelOptions<T> options) where T : class
        {
            var recordsTable = new DataTable();

            var oconn = ConnSelectNotNull(file, options.SheetName);

            var cmdAll = ConnSelectAll(file, options.SheetName);
            FillData(recordsTable, cmdAll);

            // Remove Headers
            recordsTable.Rows.RemoveAt(0);

            for (int i = 0; i < recordsTable.Rows.Count; i++)
            {
                if (recordsTable.Rows.Count == 0)
                {
                    break;
                }

                var row = recordsTable.Rows[i];
                var rowNumber = Convert.ToInt32(row[ExcelAny.RowNumberColumn]);

                if (rowNumber >= options.StartFromRow)
                {
                    break;
                }

                recordsTable.Rows.RemoveAt(0);
                i--;
            }

            return recordsTable;
        }

        private static OleDbCommand ConnSelectNotNull(string filePath, string excelTab)
        {
            var conn = OpenConnection(filePath);
            excelTab = GetTabName(excelTab, conn);

            var whereClause = WhereNotNull(conn);

            var command = new OleDbCommand
            {
                Connection = conn,
                CommandText = "SELECT 1 AS " + ExcelAny.RowNumberColumn + ", * FROM [" + excelTab + "$] WHERE " + whereClause
            };

            SetColumnsName(command);

            conn.Close();

            return command;
        }

        private static OleDbCommand ConnSelectAll(string filePath, string excelTab)
        {
            var conn = OpenConnection(filePath);
            excelTab = GetTabName(excelTab, conn);

            var whereClause = "1=1";

            var command = new OleDbCommand
            {
                Connection = conn,
                CommandText = "SELECT 1 AS " + ExcelAny.RowNumberColumn + ", * FROM [" + excelTab + "$] WHERE " + whereClause
            };

            conn.Close();

            return command;
        }

        private static void FillData(DataTable data, OleDbCommand command)
        {
            var sda = new OleDbDataAdapter(command);
            sda.Fill(data);
            sda.Dispose();

            for (int i = 0; i < data.Columns.Count; i++)
            {
                data.Columns[i].ColumnName = columnNames[i].ColumnName.Trim();
            }

            GetBlankRecords(data);
            RemoveEmptyRecords(data);
        }

        private static void SetColumnsName(OleDbCommand command)
        {
            using (OleDbDataReader reader = command.ExecuteReader())
            {
                var schemaTableColumns = reader.GetSchemaTable();

                columnNames = new DataColumn[reader.FieldCount];
                columnNames[0] = new DataColumn { ColumnName = schemaTableColumns.Rows[0][0].ToString() };

                if (reader.Read())
                {
                    for (int i = 1; i < reader.FieldCount; i++)
                    {
                        columnNames[i] = new DataColumn { ColumnName = reader[i].ToString() };
                    }
                }
            }
        }

        private static OleDbConnection OpenConnection(string filePath)
        {
            string constr = versionType == Version.ACE_x86_x64 ?
                "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=" + filePath + ";" +
                "Extended Properties='Excel 12.0 XML;HDR=No;IMEX=1';" +
                "Mode=Read;"
                :
                "Provider=Microsoft.Jet.OLEDB.4.0;" +
                "Data Source=" + filePath + ";" +
                "Extended Properties='Excel 8.0;HDR=No;IMEX=1';" +
                "Mode=Read;";

            var conn = new OleDbConnection(constr);
            conn.Open();

            return conn;
        }

        private static string GetTabName(string excelTab, OleDbConnection conn)
        {
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (excelTab == null && schemaTable != null && schemaTable.Rows.Count > 0)
            {
                excelTab = schemaTable.Rows[0]["TABLE_NAME"].ToString();
                excelTab = excelTab.Substring(0, excelTab.Length - 1);
            }

            return excelTab;
        }

        private static string WhereNotNull(OleDbConnection conn)
        {
            DataTable schemaColumns = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, null);
            string whereClause = string.Join(
                " OR ", schemaColumns.Rows.Cast<DataRow>().Select(row => "[" + row["COLUMN_NAME"] + "] IS NOT NULL AND TRIM([" + row["COLUMN_NAME"] + "]) <> ''"));

            return whereClause;
        }

        private static void GetBlankRecords(DataTable dataTable)
        {
            totalRecords = dataTable.Rows.Count;
            topEmptyRecords = 0;
            bottomEmptyRecords = 0;

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                bool isEmpty = true;

                for (int x = 1; x < dataTable.Rows[i].ItemArray.Count(); x++)
                {
                    var item = dataTable.Rows[i].ItemArray[x];

                    if (item != DBNull.Value && !string.IsNullOrEmpty(item.ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    topEmptyRecords++;
                }
                else
                {
                    break;
                }
            }

            for (int i = dataTable.Rows.Count - 1; i > topEmptyRecords; i--)
            {
                bool isEmpty = true;

                for (int x = 1; x < dataTable.Rows[i].ItemArray.Count(); x++)
                {
                    var item = dataTable.Rows[i].ItemArray[x];

                    if (item != DBNull.Value && !string.IsNullOrEmpty(item.ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    bottomEmptyRecords++;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i][0] = i + 1;
            }
        }

        private static void RemoveEmptyRecords(DataTable dataTable)
        {
            for (int i = 0; i < topEmptyRecords; i++)
            {
                dataTable.Rows.RemoveAt(0);
            }

            for (int i = 0; i < bottomEmptyRecords; i++)
            {
                dataTable.Rows.RemoveAt(dataTable.Rows.Count - 1);
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                bool isEmpty = true;

                for (int x = 1; x < dataTable.Rows[i].ItemArray.Count(); x++)
                {
                    var item = dataTable.Rows[i].ItemArray[x];

                    if (item != DBNull.Value && !string.IsNullOrEmpty(item.ToString()))
                    {
                        isEmpty = false;
                        break;
                    }
                }

                if (isEmpty)
                {
                    dataTable.Rows.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}