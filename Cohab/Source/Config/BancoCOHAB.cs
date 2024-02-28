using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DbConnection = System.Data.SqlClient;
using App.Core;
using App.Core.Desktop;

namespace App.Cohab
{
    public class BancoCOHAB
    {
        static DatabaseManager Database { get; set; }
        public static ListSynced<SqlLog> Log { get { return Database.Log; } }
        public static bool Loaded { get; set; }

        public static void Load(DatabaseMode mode = DatabaseMode.Producao)
        {
            Session.Options.DatabaseDevMode = Convert.ToBoolean(mode);

            Database = new DatabaseManager { };
            Reload();
            Loaded = true;
        }

        public static void Reload()
        {
            Database.DatabaseName = Session.Options.Database_COHAB;
            Database.DatabaseType = DatabaseType.SQLServer;
            Database.Connection = new DbConnection.SqlConnection();
            Database.ServerAddress = Session.Options.DatabaseDevMode ? Session.Options.DatabaseDev : Session.Options.DatabaseProd;

            Database.Username = Session.Options.DatabaseUsername;
            Database.Password = Session.Options.DatabasePassword;
            Database.DatabaseFile = "";
            Database.ConnectionString = "";
        }

        public async static Task<DataTable> ExecutarSelect(string sql, List<SqlParameter> parameters = null, string storedProcedure = default(string))
        {
            if (Loaded)
            {
                try { return await Database.ExecuteSelect(sql, parameters, storedProcedure); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return new DataTable();
        }

        public async static Task<string> ExecutarSelectString(string sql, List<SqlParameter> parameters = null)
        {
            if (Loaded)
            {
                try { return await Database.ExecuteSelectString(sql, parameters); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return string.Empty;
        }

        public async static Task<SqlResult> Executar(string sql, DatabaseAction action, List<SqlParameter> parameters)
        {
            if (Loaded)
            {
                try { return await Database.Execute(sql, action, parameters); }
                catch (Exception ex) { ExceptionManager.Resolve(ex, Database.LastCall); }
            }
            return new SqlResult();
        }

        public async static Task<DateTime> DataServidor()
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
