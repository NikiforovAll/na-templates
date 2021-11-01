// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api.Formatters.FluentValidation;

using System.Linq.Expressions;
using System.Reflection;
using System.Text;

// Ref: https://github.com/FluentValidation/FluentValidation/issues/226
internal class SplitPascalCaseDisplayNameResolver
{
    public static string ResolvePropertyName(
#pragma warning disable IDE0060 // Remove unused parameter
            Type type, MemberInfo memberInfo, LambdaExpression expression) =>
                SplitPascalCase(memberInfo.Name);
#pragma warning restore IDE0060 // Remove unused parameter


    /// <summary>
    /// Splits pascal case, so "FooBar" would become "Foo Bar".
    /// </summary>
    /// <remarks>
    /// Pascal case strings with periods delimiting the upper case letters,
    /// such as "Address.Line1", will have the periods removed.
    /// </remarks>
    internal static string SplitPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        // Reserved buffer space to avoid StringBuilder changing size.
        // The number is based on heuristic.
        var reservedBufferSize = 5;

        var retVal = new StringBuilder(input.Length + reservedBufferSize);

        for (var i = 0; i < input.Length; ++i)
        {
            var currentChar = input[i];
            if (char.IsUpper(currentChar))
            {
                if ((i > 1 && !char.IsUpper(input[i - 1]))
                    || (i + 1 < input.Length && !char.IsUpper(input[i + 1])))
                {
                    retVal.Append(' ');
                }
            }

            if (!Equals('.', currentChar)
                    || i + 1 == input.Length
                    || !char.IsUpper(input[i + 1]))
            {
                retVal.Append(currentChar);
            }
        }

        return retVal.ToString().Trim();
    }
}
