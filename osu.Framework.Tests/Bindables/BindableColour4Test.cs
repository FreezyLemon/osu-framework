// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using NUnit.Framework;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;

namespace osu.Framework.Tests.Bindables
{
    [TestFixture]
    public class BindableColour4Test
    {
        [TestCase(0, 0, 0, 255)]
        [TestCase(255, 255, 255, 255)]
        [TestCase(255, 102, 170, 128)]
        public void TestSet(byte r, byte g, byte b, byte a)
        {
            var value = new Colour4(r, g, b, a);

            var bindable = new BindableColour4 { Value = value };
            Assert.AreEqual(value, bindable.Value);
        }

        private static readonly object[][] hex_parsed_colours =
        {
            new object[] { "#fff", SRGBColour.White },
            new object[] { "#ff0000", SRGBColour.Red },
            new object[] { "ffff0080", SRGBColour.Yellow.Opacity(half_alpha) },
            new object[] { "00ff0080", SRGBColour.Lime.Opacity(half_alpha) },
            new object[] { "123", new Colour4(17, 34, 51, 255), },
            new object[] { "#123", new Colour4(17, 34, 51, 255) },
            new object[] { "1234", new Colour4(17, 34, 51, 68) },
            new object[] { "#1234", new Colour4(17, 34, 51, 68) },
            new object[] { "123456", new Colour4(18, 52, 86, 255) },
            new object[] { "#123456", new Colour4(18, 52, 86, 255) },
            new object[] { "12345678", new Colour4(18, 52, 86, 120) },
            new object[] { "#12345678", new Colour4(18, 52, 86, 120) }
        };

        [TestCaseSource(nameof(hex_parsed_colours))]
        public void TestParsingString(string value, Colour4 expected)
        {
            var bindable = new BindableColour4();
            bindable.Parse(value);

            Assert.AreEqual(expected, bindable.Value);
        }

        private static readonly object[][] hex_serialized_colours =
        {
            new object[] { SRGBColour.Black, "#000000" },
            new object[] { SRGBColour.White, "#FFFFFF" },
            new object[] { SRGBColour.Tan, "#D2B48C" },
            new object[] { SRGBColour.CornflowerBlue.Opacity(half_alpha), "#6495ED80" }
        };

        [TestCaseSource(nameof(hex_serialized_colours))]
        public void TestToString(Colour4 value, string expected)
        {
            var bindable = new BindableColour4 { Value = value };
            Assert.AreEqual(expected, bindable.ToString());
        }

        // 0x80 alpha is slightly more than half
        private const float half_alpha = 128f / 255f;
    }
}
