// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;

namespace osu.Framework.Tests.Visual.Drawables
{
    public partial class TestSceneColourGradient : GridTestScene
    {
        public TestSceneColourGradient()
            : base(4, 2)
        {
            SRGBColour transparentBlack = new SRGBColour(0, 0, 0, 0);

            ColourInfo[] colours =
            {
                ColourInfo.GradientHorizontal(forceLinear(SRGBColour.Pink), forceLinear(SRGBColour.SkyBlue)),
                ColourInfo.GradientHorizontal(SRGBColour.Pink, SRGBColour.SkyBlue),
                ColourInfo.GradientHorizontal(forceLinear(SRGBColour.White), forceLinear(SRGBColour.Black)),
                ColourInfo.GradientHorizontal(SRGBColour.White, SRGBColour.Black),
                ColourInfo.GradientHorizontal(forceLinear(SRGBColour.White), forceLinear(SRGBColour.Transparent)),
                ColourInfo.GradientHorizontal(SRGBColour.White, SRGBColour.Transparent),
                ColourInfo.GradientHorizontal(forceLinear(SRGBColour.White), forceLinear(transparentBlack)),
                ColourInfo.GradientHorizontal(SRGBColour.White, transparentBlack),
            };

            string[] labels =
            {
                "Colours (Linear)",
                "Colours (sRGB)",
                "White to black (Linear brightness gradient)",
                "White to black (sRGB brightness gradient)",
                "White to transparent white (Linear brightness gradient)",
                "White to transparent white (sRGB brightness gradient)",
                "White to transparent black (Linear brightness gradient)",
                "White to transparent black (sRGB brightness gradient)",
            };

            for (int i = 0; i < Rows * Cols; ++i)
            {
                Cell(i).AddRange(new Drawable[]
                {
                    new SpriteText
                    {
                        Text = labels[i],
                        Font = new FontUsage(size: 20),
                    },
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(0.5f),
                        Colour = colours[i],
                    },
                });
            }
        }

        private SRGBColour forceLinear(SRGBColour srgb) => new SRGBColour(srgb.Raw.ToLinear());
    }
}
