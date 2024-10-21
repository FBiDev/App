namespace System.ComponentModel
{
    public class FieldAttribute : Attribute
    {
        public FieldAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }
}