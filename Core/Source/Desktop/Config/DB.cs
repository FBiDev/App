using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace App.Core.Desktop
{
    public static class DB
    {
        static DatabaseManager Database { get; set; }
        public static ListSynced<SqlLog> Log { get { return Database.Log; } }
        public static bool Loaded { get; set; }

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
            Database.DatabaseFile = "";
            Database.Username = "";
            Database.Password = "";
            Database.ConnectionString = "";
        }

        public async static Task<DataTable> ExecuteSelect(string sql, List<SqlParameter> parameters = null, string storedProcedure = null)
        {
            if (Loaded)
            {
                try { return await Database.ExecuteSelect(sql, parameters, storedProcedure); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return new DataTable();
        }

        public async static Task<string> ExecuteSelectString(string sql, List<SqlParameter> parameters = null)
        {
            if (Loaded)
            {
                try { return await Database.ExecuteSelectString(sql, parameters); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return string.Empty;
        }

        public async static Task<SqlResult> Execute(string sql, DatabaseAction action, List<SqlParameter> parameters)
        {
            if (Loaded)
            {
                try { return await Database.Execute(sql, action, parameters); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return new SqlResult();
        }

        public async static Task<DateTime> DateTimeServer()
        {
            if (Loaded)
            {
                try { return await Database.DateTimeServer(); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return DateTime.MinValue;
        }
    }
}