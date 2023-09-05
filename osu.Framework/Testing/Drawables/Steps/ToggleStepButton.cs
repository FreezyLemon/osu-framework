﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;

namespace osu.Framework.Testing.Drawables.Steps
{
    public partial class ToggleStepButton : StepButton
    {
        private readonly Action<bool>? reloadCallback;
        private static readonly SRGBColour off_colour = SRGBColour.Red;
        private static readonly SRGBColour on_colour = SRGBColour.YellowGreen;

        public bool State;

        public override int RequiredRepetitions => 2;

        public ToggleStepButton(Action<bool>? reloadCallback)
        {
            this.reloadCallback = reloadCallback;
            Action = clickAction;
            LightColour = off_colour;
        }

        private void clickAction()
        {
            State = !State;
            Light.FadeColour(State ? on_colour : off_colour);
            reloadCallback?.Invoke(State);

            if (!State)
                Success();
        }

        public override string ToString() => $"Toggle: {base.ToString()} ({(State ? "on" : "off")})";
    }
}
