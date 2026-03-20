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

        public async Task<DataTable> ExecuteSelect(SqlQuery sql)
        {
            sql.Cmd = NewConnection(sql.Parameters);

            DataTable table = await ExecuteReader(sql);

            CloseConnection(sql.Cmd);

            return table;
        }

        public async Task<SqlResult> Execute(SqlQuery sql)
        {
            sql.Cmd = NewConnection(sql.Parameters);

            sql.Result = await ExecuteNonQuery(sql);

            CloseConnection(sql.Cmd);

            return sql.Result;
        }

        public async Task<string> ExecuteSelectString(SqlQuery sql)
        {
            sql.Cmd = NewConnection(sql.Parameters);

            sql.ResultSelect = await ExecuteScalar(sql);

            CloseConnection(sql.Cmd);

            return sql.ResultSelect;
        }

        public async Task<DateTime> DateTimeServer()
        {
            string query = "SELECT GETDATE() AS DataServ;";

            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                query = "SELECT strftime('%Y-%m-%d %H:%M:%f','now', 'localtime') AS DataServ;";
            }

            var sql = new SqlQuery(query, DatabaseAction.Select)
            {
                Cmd = NewConnection(null),
            };

            sql.ResultSelect = await ExecuteScalar(sql);

            CloseConnection(sql.Cmd);

            return Convert.ToDateTime(sql.ResultSelect);
        }

        public async Task<int> GetLastID(IDbCommand cmd)
        {
            string query = "SELECT SCOPE_IDENTITY() AS LastID;";

            if (DatabaseType == DatabaseType.SQLite || DatabaseType == DatabaseType.SQLiteODBC)
            {
                query = "SELECT LAST_INSERT_ROWID() AS LastID;";
            }

            var sql = new SqlQuery(query, DatabaseAction.Select)
            {
                Cmd = cmd
            };

            if (sql.Cmd != null && sql.Cmd.Connection != null && sql.Cmd.Connection.State == ConnectionState.Open)
            {
                sql.ResultSelect = await ExecuteScalar(sql);
                return Cast.ToInt(0 + sql.ResultSelect);
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

            if (parameters == null || parameters.Count == 0)
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

        private void AddLog(SqlQuery sql)
        {
            var newLog = new SqlLog(Log.Count, sql.Cmd, sql.Action, ObjectManager.GetDaoClassAndMethod(13), ObjectManager.GetDaoClassAndMethod(16));

            sql.SetQueryValues(newLog.Command);
            Log.Insert(0, newLog);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<DataTable> ExecuteReader(SqlQuery sql)
        {
            var data = new DataTable();

            if (sql.Cmd == null || sql.Cmd.Connection == null || sql.Cmd.Connection.State != ConnectionState.Open)
            {
                return data;
            }

            sql.Cmd.CommandText = sql.Query;

            if (string.IsNullOrWhiteSpace(sql.StoredProcedure) == false)
            {
                sql.Cmd.CommandType = CommandType.StoredProcedure;
                sql.Cmd.CommandText = sql.StoredProcedure;
            }
            else
            {
                sql.Cmd.CommandText = ReplaceSQLCommands(sql.Cmd.CommandText);
            }

            AddLog(sql);

            if (sql.Cmd.Parameters.Count > 0)
            {
                sql.Cmd.Prepare();
            }

            return await Task.Run(() =>
            {
                try
                {
                    using (IDataReader rdr = sql.Cmd.ExecuteReader(CommandBehavior.CloseConnection))
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

                    sql.Result = new SqlResult
                    {
                        Success = true,
                        ReturnedRows = data.Rows.Count
                    };

                    return data;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<SqlResult> ExecuteNonQuery(SqlQuery sql)
        {
            if (sql.Cmd == null)
            {
                return new SqlResult();
            }

            sql.Cmd.CommandText = sql.Query;
            sql.Cmd.CommandText = ReplaceSQLCommands(sql.Cmd.CommandText);

            AddLog(sql);

            return await Task.Run(() =>
            {
                try
                {
                    if (sql.Cmd.Parameters.Count > 0)
                    {
                        sql.Cmd.Prepare();
                    }

                    sql.Result = new SqlResult();

                    if (sql.Action == DatabaseAction.Insert)
                    {
                        sql.Result = ExecuteInsert(sql);
                    }
                    else
                    {
                        sql.Result.AffectedRows = sql.Cmd.ExecuteNonQuery();
                        sql.Result.Success = sql.Result.AffectedRows > 0;
                    }

                    return sql.Result;
                }
                catch (Exception)
                {
                    throw;
                }
            });
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private SqlResult ExecuteInsert(SqlQuery sql)
        {
            sql.Cmd.CommandText += GetLastIDSQL();

            sql.Result = new SqlResult();

            try
            {
                using (IDataReader reader = sql.Cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    if (reader.Read())
                    {
                        sql.Result.LastId = Cast.ToInt(reader["LastID"]);
                    }

                    sql.Result.AffectedRows = reader.RecordsAffected;
                    sql.Result.Success = sql.Result.AffectedRows > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sql.Result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "none")]
        private async Task<string> ExecuteScalar(SqlQuery sql)
        {
            if (sql.Cmd == null)
            {
                return string.Empty;
            }

            sql.Cmd.CommandText = sql.Query;
            AddLog(sql);

            return await Task.Run(() =>
            {
                try
                {
                    if (sql.Cmd.Parameters.Count > 0)
                    {
                        sql.Cmd.Prepare();
                    }

                    var select = sql.Cmd.ExecuteScalar();

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