using System;
using CocosSharp;

namespace Impact.Game.Entities
{
    public class MenuItemImageWithText : CCMenuItemImage
    {

        public MenuItemImageWithText(CCSpriteFrame normalSpriteframe, CCSpriteFrame selectedSpriteframe, CCSpriteFrame disabledSpriteframe, Action<object> callback, string text, CCColor3B textColour)
            : base(normalSpriteframe, selectedSpriteframe, disabledSpriteframe, callback)
        {

            var label = new CCLabel(text, "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                Color = textColour,
                PositionX = ContentSize.Width/2,
                PositionY = ContentSize.Height/2
            };
            AddChild(label);

        }

    }
}
