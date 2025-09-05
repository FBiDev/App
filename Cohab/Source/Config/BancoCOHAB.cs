using System;
using System.Data;
using System.Threading.Tasks;
using App.Core;
using DbConnection = System.Data.SqlClient;

namespace App.Cohab
{
    public static class BancoCOHAB
    {
        public static ListSynced<SqlLog> Log
        {
            get { return Database.Log; }
        }

        public static bool Loaded { get; set; }

        private static DatabaseManager Database { get; set; }

        public static void Load(DatabaseMode mode = DatabaseMode.Producao)
        {
            Session.Options.IsDeveloperDatabase = Convert.ToBoolean(mode);

            Database = new DatabaseManager { };
            Reload();
            Loaded = true;
        }

        public static void Reload()
        {
            Database.DatabaseName = Session.Options.DatabaseCOHAB;
            Database.DatabaseType = DatabaseType.SQLServer;
            Database.Connection = new DbConnection.SqlConnection();
            Database.ServerAddress = Session.Options.IsDeveloperDatabase ? Session.Options.DeveloperDatabase : Session.Options.ProductionDatabase;

            Database.Username = Session.Options.DatabaseUsername;
            Database.Password = Session.Options.DatabasePassword;
            Database.DatabaseFile = string.Empty;
            Database.ConnectionString = string.Empty;
        }

        public static async Task<DataTable> ExecutarSelect(SqlQuery sql)
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

        public static async Task<string> ExecutarSelectString(SqlQuery sql)
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

        public static async Task<SqlResult> Executar(SqlQuery sql)
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

        public static async Task<DateTime> DataServidor()
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
