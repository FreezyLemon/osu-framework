﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Shapes;

namespace osu.Framework.Graphics.Containers.Markdown
{
    /// <summary>
    /// Visualises a horizontal separator.
    /// </summary>
    /// <code>
    /// ---
    /// </code>
    public partial class MarkdownSeparator : CompositeDrawable
    {
        public MarkdownSeparator()
        {
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = CreateSeparator();
        }

        protected virtual Drawable CreateSeparator() => new Box
        {
            RelativeSizeAxes = Axes.X,
            Height = 1,
            Colour = SRGBColour.Gray,
        };
    }
}
