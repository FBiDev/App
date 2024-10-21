using System.Data.Common;
using System.Data.SQLite;

namespace App.Data.SQLite
{
    public class SQLite
    {
        public SQLite()
        {
        }

        public int DefaultTimeout { get; set; }

        public DbConnection Connection()
        {
            return new SQLiteConnection() { DefaultTimeout = DefaultTimeout };
        }
    }
}