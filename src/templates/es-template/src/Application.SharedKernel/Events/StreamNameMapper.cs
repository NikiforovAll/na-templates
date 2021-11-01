// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.SharedKernel.Events;

using System.Collections.Concurrent;

public class StreamNameMapper
{
    private static readonly StreamNameMapper Instance = new();

    private readonly ConcurrentDictionary<Type, string> typeNameMap = new();

    public static void AddCustomMap<TStream>(string mappedStreamName) =>
        AddCustomMap(typeof(TStream), mappedStreamName);

    public static void AddCustomMap(Type streamType, string mappedStreamName) =>
        Instance
            .typeNameMap
            .AddOrUpdate(streamType, mappedStreamName, (_, _) => mappedStreamName);

    public static string ToStreamPrefix<TStream>() => ToStreamPrefix(typeof(TStream));

    public static string ToStreamPrefix(Type streamType) =>
        Instance.typeNameMap.GetOrAdd(streamType, (_) =>
        {
            var modulePrefix = streamType.Namespace!.Split(".").First();
            return $"{modulePrefix}_{streamType.Name}";
        });

#pragma warning disable IDE0060 // Remove unused parameter
    public static string ToStreamId<TStream>(object aggregateId, object tenantId = null) =>
#pragma warning restore IDE0060 // Remove unused parameter
        ToStreamId(typeof(TStream), aggregateId);

    public static string ToStreamId(Type streamType, object aggregateId, object tenantId = null)
    {
        var tenantPrefix = tenantId != null ? $"{tenantId}_" : "";

        return $"{tenantPrefix}{ToStreamPrefix(streamType)}-{aggregateId}";
    }

}
