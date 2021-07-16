using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Molytho.Matrix.Calculation;

namespace Molytho.Matrix
{
    public class JsonMatrixBaseConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (typeToConvert.GetGenericTypeDefinition() != typeof(MatrixBase<>))
                return false;

            return typeToConvert.GetGenericArguments()[0].IsSerializable;
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type genericType = typeToConvert.GetGenericArguments()[0];

            JsonConverter ret = (JsonConverter)(Activator.CreateInstance(
                typeof(MatrixBaseJsonConverter<>).MakeGenericType(
                    genericType
                ),
                options
            ) ?? throw new Exception());

            return ret;
        }

        private class MatrixBaseJsonConverter<T> : JsonConverter<MatrixBase<T>>
            where T : notnull
        {
            private readonly JsonConverter<T> _typeConverter;
            private readonly Type _type = typeof(T);
            public MatrixBaseJsonConverter(JsonSerializerOptions options)
            {
                _typeConverter = (JsonConverter<T>)options
                    .GetConverter(_type);
            }

            public override MatrixBase<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.StartObject)
                    throw new JsonException();

                int height = 0, width = 0;
                MatrixBase<T>? ret = null;
                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        if (height == 0
                            || width == 0
                            || ret == null)
                            throw new JsonException();

                        return ret;
                    }

                    if (reader.TokenType != JsonTokenType.PropertyName)
                        throw new JsonException();
                    switch (reader.GetString())
                    {
                        case "Height":
                            reader.Read();
                            height = reader.GetInt32();
                            break;
                        case "Width":
                            reader.Read();
                            width = reader.GetInt32();
                            break;
                        case "Data":
                            if (width <= 0 || height <= 0)
                                throw new NotSupportedException("Width and Height are either not given or in the wrong way");
                            ret = width == 1
                                ? new Vector<T>(height)
                                : new Matrix<T>(height, width);

                            reader.Read();
                            if (reader.TokenType != JsonTokenType.StartArray)
                                throw new JsonException();
                            for (int y = 0; y < height; y++)
                            {
                                reader.Read();
                                if (reader.TokenType != JsonTokenType.StartArray)
                                    throw new JsonException();
                                for (int x = 0; x < width; x++)
                                {
                                    if (_typeConverter != null)
                                    {
                                        reader.Read();
                                        ret[x, y] = _typeConverter.Read(ref reader, _type, options) ?? throw new JsonException();
                                    }
                                    else
                                        ret[x, y] = JsonSerializer.Deserialize<T>(ref reader, options) ?? throw new JsonException();
                                }
                                reader.Read();
                                if (reader.TokenType != JsonTokenType.EndArray)
                                    throw new JsonException();
                            }
                            reader.Read();
                            if (reader.TokenType != JsonTokenType.EndArray)
                                throw new JsonException();

                            break;
                    }
                }
                throw new JsonException();
            }

            public override void Write(Utf8JsonWriter writer, MatrixBase<T> value, JsonSerializerOptions options)
            {
                writer.WriteStartObject();

                writer.WriteNumber(
                    options.PropertyNamingPolicy?.ConvertName("Height") ?? "Height",
                    value.Height
                );
                writer.WriteNumber(
                    options.PropertyNamingPolicy?.ConvertName("Width") ?? "Width",
                    value.Width
                );

                writer.WriteStartArray(
                    options.PropertyNamingPolicy?.ConvertName("Data") ?? "Data"
                );
                for (int y = 0; y < value.Height; y++)
                {
                    writer.WriteStartArray();
                    for (int x = 0; x < value.Width; x++)
                    {
                        if (_typeConverter != null)
                            _typeConverter.Write(writer, value[x, y], options);
                        else
                            JsonSerializer.Serialize(writer, value[x, y], options);
                    }
                    writer.WriteEndArray();
                }
                writer.WriteEndArray();

                writer.WriteEndObject();
            }
        }
    }
}