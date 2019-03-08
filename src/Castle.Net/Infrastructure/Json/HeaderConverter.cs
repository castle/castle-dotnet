using System;
using Newtonsoft.Json;

namespace Castle.Infrastructure.Json
{
    internal class HeaderConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var stringValue = value as string;

            var propertyName = GetPropertyName(writer.Path);

            if (propertyName.Equals("cookie", StringComparison.OrdinalIgnoreCase) 
                && string.IsNullOrEmpty(stringValue))
            {
                writer.WriteValue(value);
            }
            else
            {
                writer.WriteValue(true);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException("Only for serialization");
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        private static string GetPropertyName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;

            var propertyIndex = path.LastIndexOf(".");
            return propertyIndex >= 0
                ? path.Substring(propertyIndex + 1)
                : path;
        }
    }
}
