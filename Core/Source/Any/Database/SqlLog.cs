using System;
using System.Data;

namespace App.Core
{
    public class SqlLog
    {
        public int Line { get; set; }
        public DateTime Date { get; set; }
        public string Method { get; set; }
        public string Method2 { get; set; }
        public string Action { get; set; }

        public string CommandParameters { get; set; }
        public string Command { get; set; }

        public SqlLog(int index, IDbCommand cmd, DatabaseAction act = DatabaseAction.Null, string method = "", string method2 = "")
        {
            Line = index + 1;
            Date = DateTime.Now;
            Method = method;
            Method2 = method2;
            Action = act != DatabaseAction.Null ? act.ToString() : string.Empty;

            string query = string.Empty;
            if (cmd.NotNull())
            {
                CommandParameters = cmd.CommandText;
                query = cmd.CommandText;

                query = SqlParameter.Replace(query, cmd.Parameters);
            }

            //query = query.Replace("  ", "\t ");
            //query = query.Replace("\t", "\t ");
            //query = query.RemoveWhiteSpaces();

            Command = query;
        }
    }
}