using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace App.Core
{
    public class SqlQuery
    {
        public SqlQuery(string query, DatabaseAction action = DatabaseAction.Select, params KeyValuePair<string, object>[] parameters)
        {
            Action = action;

            Query = query;
            QueryValues = string.Empty;
            StoredProcedure = string.Empty;

            Parameters = parameters.Select(kv => new SqlParameter(kv.Key, kv.Value)).ToList();

            Result = new SqlResult();
            ResultSelect = string.Empty;

            OrderBy = "1";
        }

        public DatabaseAction Action { get; private set; }

        public string Query { get; set; }

        public string QueryValues { get; private set; }

        public string StoredProcedure { get; set; }

        public List<SqlParameter> Parameters { get; private set; }

        public IDbCommand Cmd { get; set; }

        public SqlResult Result { get; set; }

        public string ResultSelect { get; set; }

        public string OrderBy { get; private set; }

        public void SetQueryValues(string commandValues)
        {
            if (string.IsNullOrWhiteSpace(Query) == false)
            {
                QueryValues = commandValues;
            }
        }

        public void SetOrderBy(params SqlOrder[] order)
        {
            var columnsList = order.Select(o => string.Join(", ", o.Columns) + " " + o.Direction.ToString());
            OrderBy = string.Join(", ", columnsList);

            Query = Query.Replace("ORDER BY @OrderBy", "ORDER BY " + OrderBy);
        }
    }
}