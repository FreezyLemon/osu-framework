// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace osu.Framework.Tests.Visual.Drawables
{
    public partial class TestSceneBorderSmoothing : FrameworkTestScene
    {
        public TestSceneBorderSmoothing()
        {
            Children = new Drawable[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Direction = FillDirection.Full,
                    Spacing = new Vector2(0, 10),
                    Children = new Drawable[]
                    {
                        new IssueButton
                        {
                            Text = "no fill"
                        },
                        new IssueButton
                        {
                            OverlayColour = SRGBColour.White.Opacity(0.0001f),
                            Text = "very transparent fill"
                        },
                        new IssueButton
                        {
                            OverlayColour = SRGBColour.Gray,
                            Text = "gray bg"
                        },
                        new IssueButton
                        {
                            OverlayColour = SRGBColour.White.Opacity(0.5f),
                            Text = "0.5 white bg"
                        },
                        new IssueButton
                        {
                            OverlayColour = SRGBColour.White,
                            Text = "white bg"
                        },
                        new IssueButton(false)
                        {
                            BackgroundColour = SRGBColour.Gray,
                            Text = "gray should match 1",
                        },
                        new IssueButton(false)
                        {
                            BackgroundColour = SRGBColour.White,
                            Text = "gray should match 2",
                            Alpha = 0.5f,
                        },
                        new IssueButton(borderColour: SRGBColour.Gray)
                        {
                            OverlayColour = SRGBColour.Gray,
                            Text = "gray to gray bg"
                        },
                        new IssueButton(borderColour: SRGBColour.Gray)
                        {
                            OverlayColour = SRGBColour.White.Opacity(0.5f),
                            Text = "gray to transparent white bg"
                        },
                    }
                }
            };

            AddSliderStep("adjust alpha", 0f, 1f, 1, val => Child.Alpha = val);
        }

        private partial class IssueButton : BasicButton
        {
            public SRGBColour? OverlayColour;

            public IssueButton(bool drawBorder = true, SRGBColour? borderColour = null)
            {
                AutoSizeAxes = Axes.None;
                Size = new Vector2(200);

                BackgroundColour = SRGBColour.Black;

                if (drawBorder)
                {
                    Content.Masking = true;
                    Content.MaskingSmoothness = 20;
                    Content.BorderThickness = 40;

                    Content.BorderColour = borderColour ?? SRGBColour.Red;
                }
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Add(new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        Alpha = OverlayColour.HasValue ? 1 : 0,
                        RelativeSizeAxes = Axes.Both,
                        Colour = OverlayColour ?? SRGBColour.Transparent,
                    }
                });
            }
        }
    }
}
