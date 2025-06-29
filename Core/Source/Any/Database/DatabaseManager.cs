using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
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

            var result = await ExecuteNonQuery(cmd, sql, action);

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

        public string GetLastIDSQL()
        {
            string sql = "SELECT SCOPE_IDENTITY() AS LastID;";

            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                sql = "SELECT LAST_INSERT_ROWID() AS LastID;";
            }

            return ";" + Environment.NewLine + sql;
        }

        public string LIMIT_OFFSET(string sql, uint limit, uint offset = 0, string row_column = "")
        {
            if (limit == 0)
            {
                limit = uint.MaxValue;
            }

            var var_TOP = "@TOP";
            var var_LIMIT = "@LIMIT";

            var empty_TOP = new[] { @"([\t ]*[\r?\n]*[\t ]*[\t ]*" + var_TOP + "[\t ]*,[\t ]*)", " " };
            var value_TOP = new[] { @"([\t ]*[\r?\n]*[\t ]*[\t ]*" + var_TOP + "[\t ]*,[\t ]*)", " TOP " + limit + " " };

            var empty_LIMIT = new[] { @"([\t ]*[\r?\n]*[\t ]*,[\t ]*" + var_LIMIT + "[\t ]*)", " " };
            var value_LIMIT = new[] { @"([\t ]*[\r?\n]*[\t ]*,[\t ]*" + var_LIMIT + "[\t ]*)", " " + Environment.NewLine + "LIMIT " + limit + " " };

            var row_COLUMN = "@ROW_COLUMN AS RowIndex";
            var row_OFFSET = "AND RowIndex > @ROW_OFFSET";

            var var_OFFSET = "@OFFSET";

            var empty_ROW_COLUMN = new[] { @"([\t ]*[\r?\n]*[\t ]*,[\t ]*" + row_COLUMN + "[\t ]*)", " " };
            var value_ROW_COLUMN = new[] { @"(,[\t ]*" + row_COLUMN + "[\t ]*)", ", ROW_NUMBER() OVER (ORDER BY " + row_column + ") AS RowIndex " };

            var empty_ROW_OFFSET = new[] { @"([\t ]*[\r?\n]*[\t ]*" + row_OFFSET + "[\t ]*)", " " };
            var value_ROW_OFFSET = new[] { @"(" + row_OFFSET + "[\t ]*)", "AND RowIndex > " + offset + " " };

            var empty_OFFSET = new[] { @"([\t ]*[\r?\n]*[\t ]*,[\t ]*" + var_OFFSET + "[\t ]*)", " " };
            var value_OFFSET = new[] { @"([\t ]*[\r?\n]*[\t ]*,[\t ]*" + var_OFFSET + "[\t ]*)", " " + Environment.NewLine + "OFFSET " + offset + " " };

            var replaces = new Dictionary<string, string> { };

            if (limit == uint.MaxValue && offset == 0)
            {
                replaces = new Dictionary<string, string> 
                { 
                    { empty_TOP[0], empty_TOP[1] },
                    { empty_LIMIT[0], empty_LIMIT[1] },

                    { empty_ROW_COLUMN[0], empty_ROW_COLUMN[1] },
                    { empty_ROW_OFFSET[0], empty_ROW_OFFSET[1] },
                    { empty_OFFSET[0], empty_OFFSET[1] }
                };

                return RegexReplace(sql, replaces);
            }

            if (offset == 0)
            {
                replaces = new Dictionary<string, string> 
                { 
                    { empty_ROW_COLUMN[0], empty_ROW_COLUMN[1] },
                    { empty_ROW_OFFSET[0], empty_ROW_OFFSET[1] },
                    { empty_OFFSET[0], empty_OFFSET[1] }
                };

                sql = RegexReplace(sql, replaces);
            }

            switch (DatabaseType)
            {
                case DatabaseType.SQLServer:
                    replaces = new Dictionary<string, string>
                    {
                        { value_TOP[0], value_TOP[1] },
                        { empty_LIMIT[0], empty_LIMIT[1] },

                        { value_ROW_COLUMN[0], value_ROW_COLUMN[1] },
                        { value_ROW_OFFSET[0], value_ROW_OFFSET[1] },
                        { empty_OFFSET[0], empty_OFFSET[1] }
                    };
                    break;
                case DatabaseType.SQLite:
                    replaces = new Dictionary<string, string>
                    {
                        { empty_TOP[0], empty_TOP[1] },
                        { value_LIMIT[0], value_LIMIT[1] },

                        { empty_ROW_COLUMN[0], empty_ROW_COLUMN[1] },
                        { empty_ROW_OFFSET[0], empty_ROW_OFFSET[1] },
                        { value_OFFSET[0], value_OFFSET[1] }
                    };
                    break;
                default:
                    break;
            }

            return RegexReplace(sql, replaces);
        }

        private string RegexReplace(string sql, Dictionary<string, string> replaces)
        {
            foreach (KeyValuePair<string, string> item in replaces)
            {
                sql = Regex.Replace(sql, item.Key, item.Value, RegexOptions.Multiline);
            }

            return sql;
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

                if (timer.Stopped || lastcmd.Connection == null || lastcmd.Connection.State == ConnectionState.Closed)
                {
                    cmdList.Remove(lastcmd);

                    if (lastcmd.Connection is IDbConnection)
                    {
                        lastcmd.Connection.Dispose();
                    }

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

                sql = sql.Replace("GETDATE()", "STRFTIME('%Y-%m-%d %H:%M:%S', 'now', 'localtime')");

                var replaces = new Dictionary<string, string>
                {
                    { @"\bCAST\s*\(\s*@(\w+)\s+AS\s+DATE\s*\)", "DATE(@$1)" },
                    { @"\bCAST\s*\(\s*@(\w+)\s+AS\s+DATETIME\s*\)", "DATETIME(@$1)" }
                };

                foreach (KeyValuePair<string, string> item in replaces)
                {
                    sql = Regex.Replace(sql, item.Key, item.Value, RegexOptions.Multiline);
                }
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
        private async Task<SqlResult> ExecuteNonQuery(IDbCommand cmd, string sql, DatabaseAction action = DatabaseAction.Null)
        {
            if (cmd == null)
            {
                return new SqlResult();
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

                    var result = new SqlResult();

                    if (action == DatabaseAction.Insert)
                    {
                        result = ExecuteInsert(cmd);
                    }
                    else
                    {
                        result.AffectedRows = cmd.ExecuteNonQuery();
                        result.Success = result.AffectedRows > 0;
                    }

                    return result;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private SqlResult ExecuteInsert(IDbCommand cmd)
        {
            cmd.CommandText += GetLastIDSQL();

            var result = new SqlResult();

            try
            {
                using (IDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        result.LastId = Cast.ToInt(reader["LastID"]);
                    }

                    result.AffectedRows = reader.RecordsAffected;
                    result.Success = result.AffectedRows > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
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