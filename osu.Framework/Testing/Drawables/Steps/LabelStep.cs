// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Colour;

namespace osu.Framework.Testing.Drawables.Steps
{
    public partial class LabelStep : StepButton
    {
        protected override SRGBColour IdleColour => new SRGBColour(77, 77, 77, 255);

        protected override SRGBColour RunningColour => new SRGBColour(128, 128, 128, 255);

        public LabelStep()
        {
            Light.Hide();
            Height = 30;
        }
    }
}
