// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;

namespace osu.Framework.Graphics.Colour
{
    /// <summary>
    /// A colour in linearized (as in not gamma-corrected) sRGB colour space. This should only be used to do calculations on colours.
    /// Otherwise, <see cref="SRGBColour"/> should be preferred. To convert from the linearized to a gamma-corrected sRGB representation,
    /// use the <see cref="ToSRGB"/> method.
    /// </summary>
    public readonly struct LinearColour : IEquatable<LinearColour>
    {
        public readonly Colour4 Raw;

        public SRGBColour ToSRGB() => new SRGBColour(Raw.ToSRGB());

        public LinearColour(float r, float g, float b, float a)
        {
            Raw = new Colour4(r, g, b, a);
        }

        public LinearColour(Colour4 colour) => Raw = colour;

        public static LinearColour operator +(LinearColour first, LinearColour second) => new LinearColour(first.Raw + second.Raw);

        public static LinearColour operator *(LinearColour first, LinearColour second) => new LinearColour(first.Raw * second.Raw);
        public static LinearColour operator *(LinearColour first, float scalar) => new LinearColour(first.Raw * scalar);
        public static LinearColour operator *(float scalar, LinearColour first) => new LinearColour(first.Raw * scalar);

        public static LinearColour operator /(LinearColour first, float scalar) => new LinearColour(first.Raw / scalar);

        public bool Equals(LinearColour other) => Raw.Equals(other.Raw);
    }
}
