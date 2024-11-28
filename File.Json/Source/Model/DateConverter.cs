using System;
using Newtonsoft.Json;

namespace App.File.Json
{
    public class DateConverter : JsonConverter
    {
        // Specify the format for serialization
        private const string DateFormat = "yyyy-MM-dd";

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Format the DateTime as a date string
            writer.WriteValue(((DateTime)value).ToString(DateFormat));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Parse the date string back to DateTime
            if (reader.TokenType == JsonToken.String && reader.Value != null)
            {
                return DateTime.Parse(reader.Value.ToString());
            }

            throw new JsonSerializationException("Expected date string.");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}
