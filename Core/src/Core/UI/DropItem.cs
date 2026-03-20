namespace App.Core
{
    public class DropItem
    {
        public DropItem()
        {
            Value = 0;
            Text = string.Empty;
        }

        public DropItem(int value, string text)
        {
            Value = value;
            Text = text;
        }

        public int Value { get; set; }

        public string Text { get; set; }

        public override string ToString()
        {
            return Text + " - " + Value.ToString();
        }
    }
}