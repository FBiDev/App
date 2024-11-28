namespace App.File.Json
{
    public class JProperty
    {
        public JProperty(string name, object content)
        {
            Name = name;
            Value = content;
        }

        public JProperty(Newtonsoft.Json.Linq.JProperty other)
        {
            Name = other.Name;
            Value = other.Value;
        }

        public string Name { get; set; }

        public object Value { get; set; }
    }
}