// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.ValueObjects;

using NikiforovAll.CA.Template.Domain.Exceptions;
using NikiforovAll.CA.Template.Domain.SharedKernel;

public class Colour : ValueObject
{
    static Colour()
    {
    }

    private Colour() => this.Code = White.Code;

    private Colour(string code) => this.Code = code;

    public static Colour From(string code)
    {
        var colour = new Colour { Code = code };

        if (!SupportedColours.Contains(colour))
        {
            throw new UnsupportedColourException(code);
        }

        return colour;
    }

    public static Colour White => new("#FFFFFF");

    public static Colour Red => new("#FF5733");

    public static Colour Orange => new("#FFC300");

    public static Colour Yellow => new("#FFFF66");

    public static Colour Green => new("#CCFF99 ");

    public static Colour Blue => new("#6666FF");

    public static Colour Purple => new("#9966CC");

    public static Colour Grey => new("#999999");

    public string Code { get; private set; }

    public static implicit operator string(Colour colour) => colour.ToString();

    public static explicit operator Colour(string code) => From(code);

    public override string ToString() => this.Code;

    public static IEnumerable<Colour> SupportedColours
    {
        get
        {
            yield return White;
            yield return Red;
            yield return Orange;
            yield return Yellow;
            yield return Green;
            yield return Blue;
            yield return Purple;
            yield return Grey;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Code;
    }
}
