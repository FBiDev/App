namespace App.Core
{
    public class SqlResult
    {
        public bool Success { get; set; }

        public int AffectedRows { get; set; }

        public int ReturnedRows { get; set; }

        public int LastId { get; set; }

        public override string ToString()
        {
            if (ReturnedRows > 0)
            {
                return Success + " ReturnedRows:" + ReturnedRows;
            }
            else
            {
                return Success + " AffectedRows:" + AffectedRows + " LastId:" + LastId;
            }
        }
    }
}