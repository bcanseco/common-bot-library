using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CommonBotLibrary.Converters
{
    /// <summary>
    ///   Used for treating an empty array as a dictionary with JSON.NET.
    ///   Can be applied with <see cref="JsonConverterAttribute"/>.
    /// </summary>
    /// <typeparam name="T">Dictionary key type.</typeparam>
    /// <typeparam name="TF">Dictionary value type.</typeparam>
    /// <seealso href="https://stackoverflow.com/q/26725138">Source</seealso>
    public class DictionaryOrEmptyArrayConverter<T, TF> : JsonConverter
    {
        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType)
            => objectType == typeof(Dictionary<T, TF>);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    reader.Read();
                    if (reader.TokenType == JsonToken.EndArray)
                        return new Dictionary<T, TF>();
                    else
                        throw new JsonSerializationException("Non-empty JSON array does not make a valid Dictionary!");
                case JsonToken.Null:
                    return null;
                case JsonToken.StartObject:
                    var tw = new System.IO.StringWriter();
                    var writer = new JsonTextWriter(tw);
                    writer.WriteStartObject();
                    var initialDepth = reader.Depth;
                    while (reader.Read() && reader.Depth > initialDepth)
                    {
                        writer.WriteToken(reader);
                    }
                    writer.WriteEndObject();
                    writer.Flush();
                    return JsonConvert.DeserializeObject<Dictionary<T, TF>>(tw.ToString());
                default:
                    throw new JsonSerializationException("Unexpected token!");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
    }
}
