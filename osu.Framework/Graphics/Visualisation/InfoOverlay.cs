﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Graphics.Colour;
using osuTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Primitives;

namespace osu.Framework.Graphics.Visualisation
{
    internal partial class InfoOverlay : Container<FlashyBox>
    {
        private Drawable target;

        public Drawable Target
        {
            get => target;
            set
            {
                if (target == value) return;

                target = value;

                foreach (FlashyBox c in Children)
                    c.Target = target;

                Alpha = target != null ? 1.0f : 0.0f;

                Pulse();
            }
        }

        private static Quad quadAroundPosition(Vector2 pos, float sideLength)
        {
            Vector2 size = new Vector2(sideLength);
            return new Quad(pos.X - size.X / 2, pos.Y - size.Y / 2, size.X, size.Y);
        }

        private readonly FlashyBox layout;
        private readonly FlashyBox shape;
        private readonly FlashyBox childShape;

        public InfoOverlay()
        {
            RelativeSizeAxes = Axes.Both;

            Children = new[]
            {
                layout = new FlashyBox(d => d.ToScreenSpace(d.LayoutRectangle))
                {
                    Colour = SRGBColour.Green,
                    Alpha = 0.5f,
                },
                shape = new FlashyBox(d => d.ScreenSpaceDrawQuad)
                {
                    Colour = SRGBColour.Blue,
                    Alpha = 0.5f,
                },
                childShape = new FlashyBox(delegate(Drawable d)
                {
                    if (!(d is CompositeDrawable c))
                        return d.ScreenSpaceDrawQuad;

                    RectangleF rect = new RectangleF(c.ChildOffset, c.ChildSize);
                    return d.ToScreenSpace(rect);
                })
                {
                    Colour = SRGBColour.Red,
                    Alpha = 0.5f,
                },
                // We're adding this guy twice to get a border in a somewhat hacky way.
                new FlashyBox(d => quadAroundPosition(d.ToScreenSpace(d.OriginPosition), 5)) { Colour = SRGBColour.Blue, },
                new FlashyBox(d => quadAroundPosition(d.ToScreenSpace(d.OriginPosition), 3)) { Colour = SRGBColour.Yellow, },
            };
        }

        public void Pulse()
        {
            layout.FlashColour(SRGBColour.White, 250);
            shape.FlashColour(SRGBColour.White, 250);
            childShape.FlashColour(SRGBColour.White, 250);
        }

        protected override void Update()
        {
            base.Update();

            foreach (FlashyBox c in Children)
                c.Invalidate(Invalidation.DrawNode);
        }
    }
}
