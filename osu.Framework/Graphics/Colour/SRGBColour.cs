﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osuTK;
using osuTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using System;
using System.Diagnostics.Contracts;

namespace osu.Framework.Graphics.Colour
{
    /// <summary>
    /// A wrapper struct around Color4 that takes care of converting between sRGB and linear colour spaces.
    /// Internally this struct stores the colour in sRGB space, which is exposed by the <see cref="SRGB"/> member.
    /// This struct converts to linear space by using the <see cref="Linear"/> member.
    /// </summary>
    public struct SRGBColour : IEquatable<SRGBColour>
    {
        /// <summary>
        /// A <see cref="Color4"/> representation of this colour in the sRGB space.
        /// </summary>
        public Color4 SRGB;

        /// <summary>
        /// A <see cref="Color4"/> representation of this colour in the linear space.
        /// </summary>
        public Color4 Linear => SRGB.ToLinear();

        /// <summary>
        /// Create a <see cref="SRGBColour"/> from a <see cref="Colour4"/>, treating
        /// the contained colour components as being in gamma-corrected sRGB colour space.
        /// </summary>
        /// <param name="colour">The raw colour values to store.</param>
        public SRGBColour(Colour4 colour) => SRGB = colour;

        /// <summary>
        /// Convert this <see cref="SRGBColour"/> into a <see cref="LinearColour"/> by removing
        /// the gamma correction of the chromatic (RGB) components. The alpha component is untouched.
        /// </summary>
        /// <returns>A <see cref="LinearColour"/> struct containing the converted values.</returns>
        [Pure]
        public readonly LinearColour ToLinear() => new LinearColour(SRGB.ToLinear());

        /// <summary>
        /// The alpha component of this colour.
        /// </summary>
        public float Alpha => SRGB.A;

        // todo: these implicit operators should be replaced with explicit static methods (https://github.com/ppy/osu-framework/issues/5714).
        public static implicit operator SRGBColour(Color4 value) => new SRGBColour { SRGB = value };
        public static implicit operator Color4(SRGBColour value) => value.SRGB;

        public static implicit operator SRGBColour(Colour4 value) => new SRGBColour { SRGB = value };
        public static implicit operator Colour4(SRGBColour value) => value.SRGB;

        public static SRGBColour operator *(SRGBColour first, SRGBColour second)
        {
            if (isWhite(first))
            {
                if (first.Alpha == 1)
                    return second;

                return new SRGBColour
                {
                    SRGB = new Color4(
                        second.SRGB.R,
                        second.SRGB.G,
                        second.SRGB.B,
                        first.Alpha * second.Alpha)
                };
            }

            if (isWhite(second))
            {
                if (second.Alpha == 1)
                    return first;

                return new SRGBColour
                {
                    SRGB = new Color4(
                        first.SRGB.R,
                        first.SRGB.G,
                        first.SRGB.B,
                        first.Alpha * second.Alpha)
                };
            }

            var firstLinear = first.Linear;
            var secondLinear = second.Linear;

            return new SRGBColour
            {
                SRGB = new Color4(
                    firstLinear.R * secondLinear.R,
                    firstLinear.G * secondLinear.G,
                    firstLinear.B * secondLinear.B,
                    firstLinear.A * secondLinear.A).ToSRGB(),
            };
        }

        public static SRGBColour operator *(SRGBColour first, float second)
        {
            if (second == 1)
                return first;

            var firstLinear = first.Linear;

            return new SRGBColour
            {
                SRGB = new Color4(
                    firstLinear.R * second,
                    firstLinear.G * second,
                    firstLinear.B * second,
                    firstLinear.A * second).ToSRGB(),
            };
        }

        public static SRGBColour operator /(SRGBColour first, float second) => first * (1 / second);

        public static SRGBColour operator +(SRGBColour first, SRGBColour second)
        {
            var firstLinear = first.Linear;
            var secondLinear = second.Linear;

            return new SRGBColour
            {
                SRGB = new Color4(
                    firstLinear.R + secondLinear.R,
                    firstLinear.G + secondLinear.G,
                    firstLinear.B + secondLinear.B,
                    firstLinear.A + secondLinear.A).ToSRGB(),
            };
        }

        public readonly Vector4 ToVector() => new Vector4(SRGB.R, SRGB.G, SRGB.B, SRGB.A);
        public static SRGBColour FromVector(Vector4 v) => new SRGBColour { SRGB = new Color4(v.X, v.Y, v.Z, v.W) };

        /// <summary>
        /// Returns a new <see cref="SRGBColour"/> with the same RGB components, but multiplying the current
        /// alpha component by a scalar value. The final alpha is clamped to the 0-1 range.
        /// </summary>
        /// <param name="scalar">The value that the existing alpha will be multiplied by.</param>
        [Pure]
        public SRGBColour MultiplyAlpha(float scalar) => new SRGBColour(SRGB.MultiplyAlpha(scalar));

        private static bool isWhite(SRGBColour colour) => colour.SRGB.R == 1 && colour.SRGB.G == 1 && colour.SRGB.B == 1;

        public readonly bool Equals(SRGBColour other) => SRGB.Equals(other.SRGB);
        public override string ToString() => $"srgb: {SRGB}, linear: {Linear}";
    }
}
