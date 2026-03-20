using System.Collections.Generic;
using System.Linq;

namespace App.Core
{
    public class SqlOrder
    {
        public SqlOrder(SqlDirection direction, params string[] columns)
        {
            Direction = direction;
            Columns = columns.Select(c => c).ToList();
        }

        public SqlDirection Direction { get; set; }

        public List<string> Columns { get; set; }
    }
}