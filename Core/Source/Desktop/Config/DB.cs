using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace App.Core.Desktop
{
    public static class DB
    {
        public static ListSynced<SqlLog> Log
        {
            get { return Database.Log; }
        }

        public static bool Loaded { get; set; }

        private static DatabaseManager Database { get; set; }

        public static void Load(string server, string database)
        {
            Database = new DatabaseManager { };
            Reload(server, database);
            Loaded = true;
        }

        public static void Reload(string server, string database)
        {
            Database.ServerAddress = server;
            Database.DatabaseType = DatabaseType.SQLServer;
            Database.Connection = new SqlConnection();
            Database.DatabaseName = database;
            Database.DatabaseFile = string.Empty;
            Database.Username = string.Empty;
            Database.Password = string.Empty;
            Database.ConnectionString = string.Empty;
        }

        public static async Task<DataTable> ExecuteSelect(SqlQuery sql)
        {
            if (Loaded)
            {
                try
                {
                    return await Database.ExecuteSelect(sql);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Resolve(ex, Database.LastCall);
                }
            }

            return new DataTable();
        }

        public static async Task<string> ExecuteSelectString(SqlQuery sql)
        {
            if (Loaded)
            {
                try
                {
                    return await Database.ExecuteSelectString(sql);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Resolve(ex, Database.LastCall);
                }
            }

            return string.Empty;
        }

        public static async Task<SqlResult> Execute(SqlQuery sql)
        {
            if (Loaded)
            {
                try
                {
                    return await Database.Execute(sql);
                }
                catch (Exception ex)
                {
                    ExceptionManager.Resolve(ex, Database.LastCall);
                }
            }

            return new SqlResult();
        }

        public static async Task<DateTime> DateTimeServer()
        {
            if (Loaded)
            {
                try
                {
                    return await Database.DateTimeServer();
                }
                catch (Exception ex)
                {
                    ExceptionManager.Resolve(ex, Database.LastCall);
                }
            }

            return DateTime.MinValue;
        }
    }
}