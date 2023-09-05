// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace osu.Framework.Testing.Drawables.Steps
{
    public abstract partial class StepButton : CompositeDrawable
    {
        public virtual int RequiredRepetitions => 1;

        protected Box Light;
        protected Box Background;
        protected SpriteText SpriteText;

        public Action Action { get; set; }

        public LocalisableString Text
        {
            get => SpriteText.Text;
            set => SpriteText.Text = value;
        }

        private SRGBColour lightColour = SRGBColour.BlueViolet;

        public SRGBColour LightColour
        {
            get => lightColour;
            set
            {
                lightColour = value;
                if (IsLoaded) Reset();
            }
        }

        public readonly bool IsSetupStep;

        protected virtual SRGBColour IdleColour => new SRGBColour(38, 38, 38, 255);

        protected virtual SRGBColour RunningColour => SRGBColour.Gray;

        protected StepButton(bool isSetupStep = false)
        {
            IsSetupStep = isSetupStep;

            InternalChildren = new Drawable[]
            {
                Background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = IdleColour,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                Light = new Box
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 5,
                },
                SpriteText = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Font = FrameworkFont.Regular.With(size: 14),
                    X = 5,
                    Padding = new MarginPadding(5)
                }
            };

            Height = 20;
            RelativeSizeAxes = Axes.X;

            BorderThickness = 1.5f;
            BorderColour = new SRGBColour(38, 38, 38, 255);

            Masking = true;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Reset();
        }

        protected override bool OnClick(ClickEvent e)
        {
            try
            {
                PerformStep(true);
            }
            catch (Exception exc)
            {
                Logging.Logger.Error(exc, $"Step {this} triggered an error");
            }

            return true;
        }

        /// <summary>
        /// Reset this step to a default state.
        /// </summary>
        public virtual void Reset()
        {
            Background.DelayUntilTransformsFinished().FadeColour(IdleColour, 1000, Easing.OutQuint);
            Light.FadeColour(lightColour);
        }

        public virtual void PerformStep(bool userTriggered = false)
        {
            Background.ClearTransforms();
            Background.FadeColour(RunningColour, 400, Easing.OutQuint);

            try
            {
                Action?.Invoke();
            }
            catch (Exception)
            {
                Failure();
                throw;
            }
        }

        protected virtual void Failure()
        {
            Background.DelayUntilTransformsFinished().FadeColour(new SRGBColour(76, 38, 38, 255), 1000, Easing.OutQuint);
            Light.FadeColour(SRGBColour.Red);
        }

        protected virtual void Success()
        {
            Background.FinishTransforms();
            Background.FadeColour(IdleColour, 1000, Easing.OutQuint);

            Light.FadeColour(SRGBColour.YellowGreen);
        }

        public override string ToString() => $@"""{Text}"" " + base.ToString();
    }
}
