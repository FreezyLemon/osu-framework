// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace osu.Framework.Graphics.UserInterface
{
    public partial class BasicTextBox : TextBox
    {
        protected virtual float CaretWidth => 2;

        private const float caret_move_time = 60;

        protected virtual SRGBColour SelectionColour => FrameworkColour.YellowGreen;

        protected SRGBColour BackgroundCommit { get; set; } = FrameworkColour.Green;

        private SRGBColour backgroundFocused = new SRGBColour(100, 100, 100, 255);
        private SRGBColour backgroundUnfocused = new SRGBColour(100, 100, 100, 120);

        private readonly Box background;

        protected SRGBColour BackgroundFocused
        {
            get => backgroundFocused;
            set
            {
                backgroundFocused = value;
                if (HasFocus)
                    background.Colour = value;
            }
        }

        protected SRGBColour BackgroundUnfocused
        {
            get => backgroundUnfocused;
            set
            {
                backgroundUnfocused = value;
                if (!HasFocus)
                    background.Colour = value;
            }
        }

        protected virtual SRGBColour InputErrorColour => SRGBColour.Red;

        public BasicTextBox()
        {
            Add(background = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Depth = 1,
                Colour = BackgroundUnfocused,
            });

            BackgroundFocused = FrameworkColour.BlueGreen;
            BackgroundUnfocused = FrameworkColour.BlueGreenDark;
            TextContainer.Height = 0.75f;
        }

        protected override void NotifyInputError() => background.FlashColour(InputErrorColour, 200);

        protected override void OnTextCommitted(bool textChanged)
        {
            base.OnTextCommitted(textChanged);

            background.Colour = ReleaseFocusOnCommit ? BackgroundUnfocused : BackgroundFocused;
            background.ClearTransforms();
            background.FlashColour(BackgroundCommit, 400);
        }

        protected override void OnFocusLost(FocusLostEvent e)
        {
            base.OnFocusLost(e);

            background.ClearTransforms();
            background.Colour = BackgroundFocused;
            background.FadeColour(BackgroundUnfocused, 200, Easing.OutExpo);
        }

        protected override void OnFocus(FocusEvent e)
        {
            base.OnFocus(e);

            background.ClearTransforms();
            background.Colour = BackgroundUnfocused;
            background.FadeColour(BackgroundFocused, 200, Easing.Out);
        }

        protected override Drawable GetDrawableCharacter(char c) => new FallingDownContainer
        {
            AutoSizeAxes = Axes.Both,
            Child = new SpriteText { Text = c.ToString(), Font = FrameworkFont.Condensed.With(size: CalculatedTextSize) }
        };

        protected override SpriteText CreatePlaceholder() => new FadingPlaceholderText
        {
            Colour = FrameworkColour.YellowGreen,
            Font = FrameworkFont.Condensed,
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft,
            X = CaretWidth,
        };

        public partial class FallingDownContainer : Container
        {
            public override void Show()
            {
                var col = (SRGBColour)Colour;
                this.FadeColour(col.Opacity(0)).FadeColour(col, caret_move_time * 2, Easing.Out);
            }

            public override void Hide()
            {
                this.FadeOut(200);
                this.MoveToY(DrawSize.Y, 200, Easing.InExpo);
            }
        }

        public partial class FadingPlaceholderText : SpriteText
        {
            public override void Show() => this.FadeIn(200);

            public override void Hide() => this.FadeOut(200);
        }

        protected override Caret CreateCaret() => new BasicCaret
        {
            CaretWidth = CaretWidth,
            SelectionColour = SelectionColour,
        };

        public partial class BasicCaret : Caret
        {
            public BasicCaret()
            {
                RelativeSizeAxes = Axes.Y;
                Size = new Vector2(1, 0.9f);

                Colour = SRGBColour.Transparent;
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;

                Masking = true;
                CornerRadius = 1;

                InternalChild = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = SRGBColour.White,
                };
            }

            public override void Hide() => this.FadeOut(200);

            public float CaretWidth { get; set; }

            public SRGBColour SelectionColour { get; set; }

            public override void DisplayAt(Vector2 position, float? selectionWidth)
            {
                if (selectionWidth != null)
                {
                    this.MoveTo(new Vector2(position.X, position.Y), 60, Easing.Out);
                    this.ResizeWidthTo(selectionWidth.Value + CaretWidth / 2, caret_move_time, Easing.Out);
                    this
                        .FadeTo(0.5f, 200, Easing.Out)
                        .FadeColour(SelectionColour, 200, Easing.Out);
                }
                else
                {
                    this.MoveTo(new Vector2(position.X - CaretWidth / 2, position.Y), 60, Easing.Out);
                    this.ResizeWidthTo(CaretWidth, caret_move_time, Easing.Out);
                    this
                        .FadeColour(SRGBColour.White, 200, Easing.Out)
                        .Loop(c => c.FadeTo(0.7f).FadeTo(0.4f, 500, Easing.InOutSine));
                }
            }
        }
    }
}
