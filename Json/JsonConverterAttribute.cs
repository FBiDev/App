using System;
using App.Core;

namespace App.Json
{
    public enum JsonType
    {
        Boolean
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct)]
    public class JsonConverterAttribute : Attribute
    {
        public Type ConverterType { get; set; }

        public JsonConverterAttribute(JsonType converterType)
        {
            //var type = converterType.TypeCode();//for Runtime types

            switch (converterType)
            {
                case JsonType.Boolean:
                    ConverterType = typeof(BoolConverter);
                    break;
            }
        }
    }
}