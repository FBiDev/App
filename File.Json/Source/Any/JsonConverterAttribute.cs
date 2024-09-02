using System;
using App.Core;

namespace App.File
{
    public enum JsonType
    {
        Boolean
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
            }
        }

        public Type ConverterType { get; set; }
    }
}