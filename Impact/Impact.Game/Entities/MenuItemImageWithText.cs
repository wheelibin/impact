using System;
using System.Collections.Generic;
using System.Text;
using CocosSharp;

namespace Impact.Entities
{
    public class MenuItemImageWithText : CCMenuItemImage
    {

        public MenuItemImageWithText(CCSpriteFrame normalSpriteframe, CCSpriteFrame selectedSpriteframe, CCSpriteFrame disabledSpriteframe, Action<object> callback, string text)
            : base(normalSpriteframe, selectedSpriteframe, disabledSpriteframe, callback)
        {

            var label = new CCLabel(text, "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                Color = new CCColor3B(123, 129, 131),
                PositionX = ContentSize.Width/2,
                PositionY = ContentSize.Height/2
            };
            AddChild(label);

        }

    }
}
