// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.SharedKernel.Events;

using System.Collections.Concurrent;

public class EventTypeMapper
{
    private static readonly EventTypeMapper Instance = new();

    private readonly ConcurrentDictionary<Type, string> typeNameMap = new();
    private readonly ConcurrentDictionary<string, Type> typeMap = new();

    public static void AddCustomMap<T>(string mappedEventTypeName) =>
        AddCustomMap(typeof(T), mappedEventTypeName);

    public static void AddCustomMap(Type eventType, string mappedEventTypeName)
    {
        Instance
            .typeNameMap
            .AddOrUpdate(eventType,
                mappedEventTypeName, (_, _) => mappedEventTypeName);

        Instance
            .typeMap
            .AddOrUpdate(mappedEventTypeName, eventType, (_, _) => eventType);
    }

    public static string ToName<TEventType>() => ToName(typeof(TEventType));

    public static string ToName(Type eventType) =>
    Instance.typeNameMap.GetOrAdd(eventType, (_) =>
    {
        var eventTypeName = eventType.FullName!.Replace(".", "_");

        Instance.typeMap.AddOrUpdate(eventTypeName, eventType, (_, _) => eventType);

        return eventTypeName;
    });

    public static Type ToType(string eventTypeName) => Instance.typeMap.GetOrAdd(eventTypeName, (_) =>
    {
        var type = TypeProvider.GetFirstMatchingTypeFromCurrentDomainAssembly(eventTypeName.Replace("_", "."))!;

        if (type == null)
        {
            throw new InvalidOperationException($"Type map for '{eventTypeName}' wasn't found!");
        }

        Instance.typeNameMap.AddOrUpdate(type, eventTypeName, (_, _) => eventTypeName);

        return type;
    });
}
