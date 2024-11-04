using System;

namespace App.File
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class JsonPropertyAttribute : Attribute
    {
        public JsonPropertyAttribute(string propertyName, Required required = Required.Default, object defaultValue = null)
        {
            PropertyName = propertyName;
            Required = required;
            DefaultValue = defaultValue;
        }

        public string PropertyName { get; set; }

        public Required Required { get; set; }

        public object DefaultValue { get; set; }
    }
}
