﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Bindables;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Localisation;

namespace osu.Framework.Graphics.UserInterface
{
    public partial class BasicButton : Button
    {
        public LocalisableString Text
        {
            get => SpriteText.Text;
            set => SpriteText.Text = value;
        }

        public SRGBColour BackgroundColour
        {
            get => Background.Colour;
            set => Background.FadeColour(value);
        }

        private SRGBColour? flashColour;

        /// <summary>
        /// The colour the background will flash with when this button is clicked.
        /// </summary>
        public SRGBColour FlashColour
        {
            get => flashColour ?? BackgroundColour;
            set => flashColour = value;
        }

        /// <summary>
        /// The additive colour that is applied to the background when hovered.
        /// </summary>
        public SRGBColour HoverColour
        {
            get => Hover.Colour;
            set => Hover.FadeColour(value);
        }

        private SRGBColour disabledColour = SRGBColour.Gray;

        /// <summary>
        /// The additive colour that is applied to this button when disabled.
        /// </summary>
        public SRGBColour DisabledColour
        {
            get => disabledColour;
            set
            {
                if (disabledColour == value)
                    return;

                disabledColour = value;
                Enabled.TriggerChange();
            }
        }

        /// <summary>
        /// The duration of the transition when hovering.
        /// </summary>
        public double HoverFadeDuration { get; set; } = 200;

        /// <summary>
        /// The duration of the flash when this button is clicked.
        /// </summary>
        public double FlashDuration { get; set; } = 200;

        /// <summary>
        /// The duration of the transition when toggling the Enabled state.
        /// </summary>
        public double DisabledFadeDuration { get; set; } = 200;

        protected Box Hover;
        protected Box Background;
        protected SpriteText SpriteText;

        public BasicButton()
        {
            AddRange(new Drawable[]
            {
                Background = new Box
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = FrameworkColour.BlueGreen
                },
                Hover = new Box
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = SRGBColour.White.Opacity(.1f),
                    Blending = BlendingParameters.Additive
                },
                SpriteText = CreateText()
            });

            Enabled.BindValueChanged(enabledChanged, true);
        }

        protected virtual SpriteText CreateText() => new SpriteText
        {
            Depth = -1,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            Font = FrameworkFont.Regular,
            Colour = FrameworkColour.Yellow
        };

        protected override bool OnClick(ClickEvent e)
        {
            if (Enabled.Value)
                Background.FlashColour(FlashColour, FlashDuration);

            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Enabled.Value)
                Hover.FadeIn(HoverFadeDuration);

            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            Hover.FadeOut(HoverFadeDuration);
        }

        private void enabledChanged(ValueChangedEvent<bool> e)
        {
            this.FadeColour(e.NewValue ? SRGBColour.White : DisabledColour, DisabledFadeDuration, Easing.OutQuint);
        }
    }
}
