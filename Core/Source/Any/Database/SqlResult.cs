namespace App.Core
{
    public class SqlResult
    {
        public bool Success { get; set; }

        public int AffectedRows { get; set; }

        public int LastId { get; set; }
    }
}