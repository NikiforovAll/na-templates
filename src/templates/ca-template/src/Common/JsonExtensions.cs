// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Common;

using System;
using System.Text.Json;
using Dahomey.Json.Util;

public static class JsonExtensions
{
    public static JsonDocument JsonDocumentFromObject(
        this object value,
        JsonSerializerOptions options = default)
    {
        if (value is string valueStr)
        {
            try
            { return JsonDocument.Parse(valueStr); }
            catch { }
        }

        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, options);
        return JsonDocument.Parse(bytes);
    }

    public static JsonElement JsonElementFromObject(
        this object value,
        JsonSerializerOptions options = default)
    {
        JsonElement result;
        using (var doc = JsonDocumentFromObject(value, options))
        {
            result = doc.RootElement.Clone();
        }
        return result;
    }

    public static T ToObject<T>(
        this JsonElement element, JsonSerializerOptions options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
        {
            element.WriteTo(writer);
        }

        return JsonSerializer.Deserialize<T>(bufferWriter.WrittenSpan, options);
    }

    public static T ToObject<T>(
        this JsonDocument document,
        JsonSerializerOptions options = null)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.RootElement.ToObject<T>(options);
    }

    public static object ToObject(
        this JsonElement element, Type returnType,
        JsonSerializerOptions options = null)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(bufferWriter))
        {
            element.WriteTo(writer);
        }

        return JsonSerializer.Deserialize(bufferWriter.WrittenSpan, returnType, options);
    }

    public static object ToObject(
        this JsonDocument document,
        Type returnType,
        JsonSerializerOptions options = null)
    {
        if (document == null)
        {
            throw new ArgumentNullException(nameof(document));
        }

        return document.RootElement.ToObject(returnType, options);
    }
}
