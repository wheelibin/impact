using System;
using CocosSharp;
using Impact.Game;
using Impact.Game.Managers;

namespace Impact.Entities
{
    public class MenuButton : CCNode
    {
        private readonly Action _clickCallback;

        public MenuButton(CCSpriteSheet spriteSheet, string textureFilename, string text, Action clickCallback)
        {
            _clickCallback = clickCallback;

            var frame = spriteSheet.Frames.Find(item => item.TextureFilename == textureFilename);
            var button = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            AddChild(button);
            ContentSize = button.ContentSize;

            if (!string.IsNullOrEmpty(text))
            {
                var label = new CCLabel(text, "visitor1.ttf", 48, CCLabelFormat.SystemFont)
                {
                    Position = ContentSize.Center,
                    AnchorPoint = CCPoint.AnchorMiddle
                };
                AddChild(label);
            }

        }

        public void Click()
        {
            _clickCallback();
        }

    }
}
