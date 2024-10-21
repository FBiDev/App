using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace App.Core
{
    public class DatabaseManager
    {
        public const int DefaultCommandTimeout = 10;

        private const int DefaultConnectionTimeout = 10;

        private List<IDbCommand> cmdList = new List<IDbCommand>();

        public DatabaseManager()
        {
            Log = new ListSynced<SqlLog>();
        }

        public ListSynced<SqlLog> Log { get; set; }

        public string LastCall
        {
            get
            {
                if (Log.Count == 0)
                {
                    return string.Empty;
                }

                return Environment.NewLine + "[Log]" + Environment.NewLine + Log[0].Method2 + Environment.NewLine + Log[0].Method;
            }
        }

        public DatabaseType DatabaseType { get; set; }

        public string ServerAddress { get; set; }

        public string DatabaseName { get; set; }

        public string DatabaseFile { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public IDbConnection Connection { get; set; }

        public string ConnectionString { get; set; }

        private int ConnectionTimeout { get; set; }

        public async Task<DataTable> ExecuteSelect(string sql, List<SqlParameter> parameters = null, string storedProcedure = null)
        {
            var cmd = NewConnection(parameters);

            DataTable table = await ExecuteReader(cmd, sql, storedProcedure);

            CloseConnection(cmd);

            return table;
        }

        public async Task<SqlResult> Execute(string sql, DatabaseAction action, List<SqlParameter> parameters)
        {
            var cmd = NewConnection(parameters);

            var result = new SqlResult
            {
                AffectedRows = await ExecuteNonQuery(cmd, sql, action)
            };

            result.Success = result.AffectedRows > 0;

            if (action == DatabaseAction.Insert && result.Success)
            {
                result.LastId = await GetLastID(cmd);
            }

            CloseConnection(cmd);

            return result;
        }

        public async Task<string> ExecuteSelectString(string sql, List<SqlParameter> parameters = null)
        {
            var cmd = NewConnection(parameters);

            string select = await ExecuteScalar(cmd, sql);

            CloseConnection(cmd);

            return select;
        }

        public async Task<DateTime> DateTimeServer()
        {
            string sql = "SELECT GETDATE() AS DataServ;";
            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                sql = "SELECT strftime('%Y-%m-%d %H:%M:%f','now', 'localtime') AS DataServ;";
            }

            List<SqlParameter> parameters = null;
            var cmd = NewConnection(parameters);

            string select = await ExecuteScalar(cmd, sql);

            CloseConnection(cmd);

            return Convert.ToDateTime(select);
        }

        public async Task<int> GetLastID(IDbCommand cmd)
        {
            string sql = "SELECT SCOPE_IDENTITY() AS LastID;";
            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                sql = "SELECT LAST_INSERT_ROWID() AS LastID;";
            }

            if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
            {
                string select = await ExecuteScalar(cmd, sql);
                return Cast.ToInt(0 + select);
            }

            return 0;
        }

        private IDbCommand NewConnection(List<SqlParameter> parameters)
        {
            var conn = (IDbConnection)Connection.Clone();

            if (ConnectionTimeout == 0)
            {
                ConnectionTimeout = DefaultConnectionTimeout;
            }

            var cmd = conn.CreateCommand();
            cmdList.Insert(0, cmd);

            OpenConnection(cmd);

            if (parameters == null)
            {
                return cmd;
            }

            AddSQLParameters(cmd, parameters);

            return cmd;
        }

        private string DefaultConnectionString()
        {
            switch (DatabaseType)
            {
                case DatabaseType.SQLServer: return "User ID=" + Username + ";Password=" + Password + ";Data Source=" + ServerAddress + ";Initial Catalog=" + DatabaseName + ";Connection Timeout=" + ConnectionTimeout + ";Persist Security Info=False;Packet Size=4096";
                case DatabaseType.SQLite: return "Data Source=" + DatabaseFile + ";Version=3;";
                case DatabaseType.SQLiteODBC: return "Driver=SQLite3 ODBC Driver; Datasource=" + DatabaseFile + ";Version=3;New=True;Compress=True;";
            }

            return null;
        }

        private void SetConnectionString(IDbConnection conn)
        {
            if (conn == null || conn.ConnectionString.IsEmpty() == false)
            {
                return;
            }

            if (ConnectionString.IsEmpty())
            {
                conn.ConnectionString = DefaultConnectionString();
            }
            else
            {
                conn.ConnectionString = ConnectionString;
            }
        }

        private void OpenConnection(IDbCommand cmd)
        {
            try
            {
                var conn = cmd.Connection;

                SetConnectionString(conn);

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                if (cmd == null)
                {
                    cmd = conn.CreateCommand();
                }

                cmd.Connection = conn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void CloseConnection(IDbCommand cmd)
        {
            if (cmd == null || cmd.Connection == null)
            {
                return;
            }

            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }

            var timer = new StopwatchTimer(15);

            while (cmdList.Count > 10)
            {
                var lastcmd = cmdList.Last();

                if (timer.Stopped || lastcmd.Connection.State == ConnectionState.Closed)
                {
                    cmdList.Remove(lastcmd);
                    lastcmd.Connection.Dispose();
                    lastcmd.Dispose();

                    timer.Restart();
                }
            }
        }

        private void AddSQLParameters(IDbCommand cmd, List<SqlParameter> parameters)
        {
            if (cmd == null)
            {
                return;
            }

            foreach (var parameter in parameters)
            {
                var p = cmd.CreateParameter();
                p.ParameterName = parameter.ParameterName;
                p.DbType = parameter.DbType;
                p.Value = parameter.Value;
                p.Size = parameter.Size;
                p.Precision = parameter.Precision;
                p.Scale = parameter.Scale;
                cmd.Parameters.Add(p);
            }
        }

        private string ReplaceSQLCommands(string sql)
        {
            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                // Concat in SQLite = ||
                sql = sql.Replace("'%'+", "'%'||");
                sql = sql.Replace("+'%'", "||'%'");

                sql = sql.Replace("'%' +", "'%' ||");
                sql = sql.Replace("+ '%'", "|| '%'");

                // Convert dateString to date
                sql = sql.Replace("CONVERT(date,", "date(");
                sql = sql.Replace("CONVERT(datetime,", "datetime(");
                sql = sql.Replace("CONVERT(datetime2,", "datetime(");
                sql = sql.Replace("CONVERT(datetime2(0),", "datetime(");
                sql = sql.Replace("CONVERT(datetime2(3),", "strftime('%Y-%m-%d %H:%M:%f',");
            }

            return sql;
        }

        private void AddLog(IDbCommand cmd, DatabaseAction action = DatabaseAction.Null)
        {
            Log.Insert(0, new SqlLog(Log.Count, cmd, action, ObjectManager.GetDaoClassAndMethod(13), ObjectManager.GetDaoClassAndMethod(16)));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<DataTable> ExecuteReader(IDbCommand cmd, string sql, string storedProcedure)
        {
            var data = new DataTable();

            if (cmd == null || cmd.Connection == null || cmd.Connection.State != ConnectionState.Open)
            {
                return data;
            }

            cmd.CommandText = sql;

            if (string.IsNullOrWhiteSpace(storedProcedure) == false)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = storedProcedure;
            }
            else
            {
                cmd.CommandText = ReplaceSQLCommands(cmd.CommandText);
            }

            AddLog(cmd, DatabaseAction.Select);

            if (cmd.Parameters.Count > 0)
            {
                cmd.Prepare();
            }

            return await Task.Run(() =>
            {
                try
                {
                    using (IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        var schemaTabela = rdr.GetSchemaTable();

                        foreach (DataRow dataRow in schemaTabela.Rows)
                        {
                            var dataColumn = new DataColumn
                            {
                                ColumnName = dataRow["ColumnName"].ToString(),
                                DataType = Type.GetType(dataRow["DataType"].ToString()),
                                ReadOnly = (bool)dataRow["IsReadOnly"],
                                AutoIncrement = (bool)dataRow["IsAutoIncrement"],
                                Unique = (bool)dataRow["IsUnique"]
                            };
                            data.Columns.Add(dataColumn);
                        }

                        DataRow dt;
                        while (rdr.FieldCount > 0 && rdr.Read())
                        {
                            dt = data.NewRow();
                            for (int i = 0; i < data.Columns.Count; i++)
                            {
                                dt[i] = rdr[i];
                            }

                            data.Rows.Add(dt);
                        }
                    }

                    return data;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<int> ExecuteNonQuery(IDbCommand cmd, string sql, DatabaseAction action = DatabaseAction.Null)
        {
            int affectedRows = 0;

            if (cmd == null)
            {
                return affectedRows;
            }

            cmd.CommandText = sql;
            cmd.CommandText = ReplaceSQLCommands(cmd.CommandText);

            AddLog(cmd, action);

            return await Task.Run(() =>
            {
                try
                {
                    if (cmd.Parameters.Count > 0)
                    {
                        cmd.Prepare();
                    }

                    affectedRows = cmd.ExecuteNonQuery();
                    return affectedRows;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<string> ExecuteScalar(IDbCommand cmd, string sql)
        {
            if (cmd == null)
            {
                return string.Empty;
            }

            cmd.CommandText = sql;
            AddLog(cmd);

            return await Task.Run(() =>
            {
                try
                {
                    if (cmd.Parameters.Count > 0)
                    {
                        cmd.Prepare();
                    }

                    var select = cmd.ExecuteScalar();

                    if (select != null)
                    {
                        return select.ToString();
                    }

                    return string.Empty;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }
    }
}