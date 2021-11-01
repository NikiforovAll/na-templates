// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Common.Converters;

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

// This class is used to convert DateTime in JSON.

public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        DateTime.Parse(reader.GetString());

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        var jsonDateTimeFormat = DateTimeToString(value);

        writer.WriteStringValue(jsonDateTimeFormat);
    }

    private static string DateTimeToString(DateTime value)
    {
        var jsonDateTimeFormat = DateTime.SpecifyKind(value, DateTimeKind.Utc)
            .ToString("o", System.Globalization.CultureInfo.InvariantCulture);

        return jsonDateTimeFormat;
    }
}
