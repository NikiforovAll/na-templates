// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Api.Formatters.FluentValidation;

using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using global::FluentValidation.Internal;

// Ref: https://github.com/FluentValidation/FluentValidation/issues/226
internal class CamelCasePropertyNameResolver
{
    public static string? ResolvePropertyName(
        Type type, MemberInfo memberInfo, LambdaExpression expression) =>
            ToCamelCase(DefaultPropertyNameResolver(type, memberInfo, expression));

    private static string? DefaultPropertyNameResolver(
        Type _, MemberInfo memberInfo, LambdaExpression expression)
    {
        if (expression != null)
        {
            var chain = PropertyChain.FromExpression(expression);
            if (chain.Count > 0)
            {
                return chain.ToString();
            }
        }

        if (memberInfo != null)
        {
            return memberInfo.Name;
        }

        return null;
    }

    private static string? ToCamelCase(string? s)
    {
        if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        {
            return s;
        }

        var chars = s.ToCharArray();

        for (var i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
            {
                break;
            }

            var hasNext = i + 1 < chars.Length;
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                break;
            }

            chars[i] = char.ToLower(chars[i], CultureInfo.InvariantCulture);
        }

        return new string(chars);
    }
}
