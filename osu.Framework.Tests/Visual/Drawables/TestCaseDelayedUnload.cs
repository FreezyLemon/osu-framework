// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using osuTK;
using osuTK.Graphics;

namespace osu.Framework.Tests.Visual.Drawables
{
    public class TestCaseDelayedUnload : TestCase
    {
        private const int panel_count = 1024;

        public TestCaseDelayedUnload()
        {
            FillFlowContainer<Container> flow;
            ScrollContainer<Drawable> scroll;

            Children = new Drawable[]
            {
                scroll = new BasicScrollContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        flow = new FillFlowContainer<Container>
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                        }
                    }
                }
            };

            for (int i = 1; i < panel_count; i++)
                flow.Add(new Container
                {
                    Size = new Vector2(128),
                    Children = new Drawable[]
                    {
                        new DelayedLoadUnloadWrapper(() => new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new TestBox { RelativeSizeAxes = Axes.Both }
                            }
                        }, 500, 2000),
                        new SpriteText { Text = i.ToString() },
                    }
                });

            var childrenWithAvatarsLoaded = flow.Children.Where(c => c.Children.OfType<DelayedLoadWrapper>().First().Content?.IsLoaded ?? false);

            int loadedCountInitial = 0;
            int loadedCountSecondary = 0;

            AddUntilStep("wait some loaded", () => (loadedCountInitial = childrenWithAvatarsLoaded.Count()) > 5);

            AddStep("scroll down", () => scroll.ScrollToEnd());

            AddUntilStep("wait more loaded", () => (loadedCountSecondary = childrenWithAvatarsLoaded.Count()) > loadedCountInitial);

            AddAssert("not too many loaded", () => childrenWithAvatarsLoaded.Count() < panel_count / 4);

            AddUntilStep("wait some unloaded", () => childrenWithAvatarsLoaded.Count() < loadedCountSecondary);
        }

        public class TestBox : Container
        {
            public TestBox()
            {
                RelativeSizeAxes = Axes.Both;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Child = new SpriteText
                {
                    Colour = Color4.Yellow,
                    Text = @"loaded",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                };
            }
        }
    }
}
