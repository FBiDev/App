namespace System.ComponentModel
{
    public enum IsBool
    {
        NotSet,
        No,
        Yes
    }

    public class DisplayAttribute : Attribute
    {
        public DisplayAttribute(string name = null, IsBool isBoolValue = IsBool.NotSet)
        {
            Name = name;
            AutoGenerateField = true;
            ////Order = -1;
            IsBool = isBoolValue;
        }

        public string Name { get; set; }

        public IsBool IsBool { get; set; }

        public string Description { get; set; }

        public bool AutoGenerateField { get; set; }

        ////public int Order { get; set; }

        public string Format { get; set; }
    }
}