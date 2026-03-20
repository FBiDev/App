namespace System.ComponentModel
{
    public enum ColumnAutoSizeMode
    {
        NotSet = 0,
        None = 1,
        ColumnHeader = 2,
        AllCellsExceptHeader = 4,
        AllCells = 6,
        DisplayedCellsExceptHeader = 8,
        DisplayedCells = 10,
        Fill = 16,
    }

    public enum ColumnAlign
    {
        NotSet = 0,
        Left = 16,
        Center = 32,
        Right = 64
    }

    public enum ColumnFormat
    {
        NotSet,
        StringCenter,
        Number,
        NumberCenter,
        Date,
        DateCenter,
        Image
    }

    public class StyleAttribute : Attribute
    {
        public StyleAttribute()
        {
            Width = -1;
        }

        public ColumnAutoSizeMode AutoSizeMode { get; set; }

        public ColumnAlign Align { get; set; }

        public ColumnFormat Format { get; set; }

        public string FontName { get; set; }

        public int Width { get; set; }
    }
}