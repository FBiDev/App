using System;

namespace App.File.Json
{
    public enum JsonType
    {
        Boolean,
        Date
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonConverterAttribute : Attribute
    {
        public JsonConverterAttribute(JsonType converterType)
        {
            //// for Runtime types
            //// var type = converterType.TypeCode();

            switch (converterType)
            {
                case JsonType.Boolean:
                    ConverterType = typeof(BoolConverter);
                    break;
                case JsonType.Date:
                    ConverterType = typeof(DateConverter);
                    break;
            }
        }

        public Type ConverterType { get; set; }
    }
}