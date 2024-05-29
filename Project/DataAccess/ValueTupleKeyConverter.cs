using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

public class ValueTupleKeyConverter<TKey, TValue> : JsonConverter<Dictionary<(int, int), TValue>>
{
    public override Dictionary<(int, int), TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var dictionary = new Dictionary<(int, int), TValue>();
        
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException();
        
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                return dictionary;

            // Read the key
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            
            var propertyName = reader.GetString();
            var keyParts = propertyName.Trim('(', ')').Split(',');
            var key = (int.Parse(keyParts[0]), int.Parse(keyParts[1]));

            // Read the value
            reader.Read();
            var value = JsonSerializer.Deserialize<TValue>(ref reader, options);

            dictionary.Add(key, value);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, Dictionary<(int, int), TValue> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        
        foreach (var kvp in value)
        {
            var keyString = $"({kvp.Key.Item1},{kvp.Key.Item2})";
            writer.WritePropertyName(keyString);
            JsonSerializer.Serialize(writer, kvp.Value, options);
        }

        writer.WriteEndObject();
    }
}