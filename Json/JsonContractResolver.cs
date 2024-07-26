using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Json
{
    class JsonContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttributes(true).Any(attr => attr.GetType() == typeof(JsonIgnoreAttribute)))
            {
                property.ShouldSerialize = instance => false;
            }
            else
            {
                var attribute = member.GetCustomAttributes(true).OfType<JsonConverterAttribute>().FirstOrDefault();

                if (attribute != null)
                {
                    var converter = (JsonConverter)Activator.CreateInstance(attribute.ConverterType);
                    property.Converter = converter;
                }
            }

            return property;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            // Include static properties
            var staticProperties = type.GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(p => !p.GetCustomAttributes(typeof(JsonIgnoreAttribute), true).Any())
                .Select(p => CreateProperty(p, memberSerialization))
                .ToList();

            properties = properties.Concat(staticProperties).ToList();

            return properties;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            var attribute = objectType.GetCustomAttributes(true).OfType<JsonConverterAttribute>().FirstOrDefault();
            if (attribute != null)
            {
                var converter = (JsonConverter)Activator.CreateInstance(attribute.ConverterType);
                contract.Converter = converter;
            }

            return contract;
        }
    }
}
