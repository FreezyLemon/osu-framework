// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using osu.Framework.Graphics.Colour;

namespace osu.Framework.Benchmarks
{
    [MemoryDiagnoser]
    public class BenchmarkColourInfo
    {
        [ParamsSource(nameof(ColourParams))]
        public ColourInfo Colour { get; set; }

        public IEnumerable<ColourInfo> ColourParams
        {
            get
            {
                yield return ColourInfo.SingleColour(SRGBColour.Transparent);
                yield return ColourInfo.SingleColour(SRGBColour.Cyan);
                yield return ColourInfo.SingleColour(SRGBColour.DarkGray);
            }
        }

        [Benchmark]
        public SRGBColour ConvertToSRGBColour() => Colour;

        [Benchmark]
        public LinearColour ConvertToLinear() => ((SRGBColour)Colour).ToLinear();

        [Benchmark]
        public LinearColour ExtractAndConvertToLinear()
        {
            Colour.TryExtractSingleColour(out SRGBColour colour);
            return colour.ToLinear();
        }
    }
}
