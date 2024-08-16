using System.Data.Common;
using System.Data.SQLite;

namespace App.Data.SQLite
{
    public class SQLite
    {
        public int DefaultTimeout { get; set; }

        public SQLite()
        {
        }

        public DbConnection Connection()
        {
            return new SQLiteConnection() { DefaultTimeout = DefaultTimeout };
        }
    }
}