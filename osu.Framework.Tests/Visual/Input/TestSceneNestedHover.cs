﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK;

namespace osu.Framework.Tests.Visual.Input
{
    public partial class TestSceneNestedHover : FrameworkTestScene
    {
        public TestSceneNestedHover()
        {
            HoverBox box1;
            Add(box1 = new HoverBox(SRGBColour.Gray, SRGBColour.White)
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(300, 300),
                CornerRadius = 100,
                CornerExponent = 5,
                Masking = true,
            });

            HoverBox box2;
            box1.Add(box2 = new HoverBox(SRGBColour.Pink, SRGBColour.Red)
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Position = new Vector2(0.2f, 0.2f),
                Size = new Vector2(0.6f, 0.6f)
            });

            box2.Add(new HoverBox(SRGBColour.LightBlue, SRGBColour.Blue, false)
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Position = new Vector2(0.2f, 0.2f),
                Size = new Vector2(0.6f, 0.6f)
            });
        }

        private partial class HoverBox : Container
        {
            private readonly SRGBColour normalColour;
            private readonly SRGBColour hoveredColour;

            private readonly Box box;
            private readonly bool propagateHover;

            public HoverBox(SRGBColour normalColour, SRGBColour hoveredColour, bool propagateHover = true)
            {
                this.normalColour = normalColour;
                this.hoveredColour = hoveredColour;
                this.propagateHover = propagateHover;

                Children = new Drawable[]
                {
                    box = new Box
                    {
                        Colour = normalColour,
                        RelativeSizeAxes = Axes.Both
                    }
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                box.Colour = hoveredColour;
                return !propagateHover;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                box.Colour = normalColour;
            }
        }
    }
}
