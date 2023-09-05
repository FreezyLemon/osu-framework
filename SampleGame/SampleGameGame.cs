﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework;
using osu.Framework.Graphics;
using osuTK;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Colour;

namespace SampleGame
{
    public partial class SampleGameGame : Game
    {
        private Box box = null!;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(box = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(150, 150),
                Colour = SRGBColour.Tomato
            });
        }

        protected override void Update()
        {
            base.Update();
            box.Rotation += (float)Time.Elapsed / 10;
        }
    }
}
