// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application;

using System.Reflection;
using AutoMapper;
using NikiforovAll.CA.Template.Application.SharedKernel.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile() => this.ApplyMappingsFromAssemblies(Assembly.GetExecutingAssembly());

    private void ApplyMappingsFromAssemblies(params Assembly[] assemblies)
    {
        foreach (var ass in assemblies)
        {
            this.ApplyMappingsFromAssembly(ass);
        }
    }
    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
            .ToList();

        foreach (var type in types)
        {
            var instance = Activator.CreateInstance(type);

            var methodInfo = type.GetMethod("Mapping")
                ?? type?.GetInterface("IMapFrom`1")?.GetMethod("Mapping");

            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
