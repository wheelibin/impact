using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class LevelSelectScene : CCScene
    {
        private readonly CCGameView _gameView;
        private static CCColor3B _buttonTextColour = new CCColor3B(178, 242,0);

        public LevelSelectScene(CCGameView gameView) : base(gameView)
        {
            _gameView = gameView;

            var layer = new CCLayer();
            AddChild(layer);

            //background
            var frame = GameManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectBackground.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            layer.AddChild(sprite);

            //var label = new CCLabel("Select level", "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            //{
            //    Color = new CCColor3B(123,129,131),
            //    PositionX = 50,
            //    PositionY = gameView.DesignResolution.Height - 50,
            //    AnchorPoint = CCPoint.AnchorUpperLeft
            //};
            //layer.AddChild(label);

            List<CCMenuItem> menuItems = new List<CCMenuItem>();
            //Back button
            CCSpriteFrame backButtonFrame = GameManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButton.png");
            MenuItemImageWithText backButton = new MenuItemImageWithText(backButtonFrame, backButtonFrame, backButtonFrame, BackButton_Action, "<-", _buttonTextColour);

            menuItems.Add(backButton);
            //Levels
            for (int l = 0; l < LevelManager.Instance.NumberOfLevels; l++)
            {
                CCSpriteFrame levelSelectButtonFrame = GameManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButton.png");
                MenuItemImageWithText levelSelectbutton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, LevelSelectButton_Action, (l+1).ToString(), _buttonTextColour)
                {
                    UserData = l + 1
                };
                menuItems.Add(levelSelectbutton);
            }

            CCMenu menu = new CCMenu(menuItems.ToArray());

            AlignItemsInGrid(menu, new CCPoint(5, 5), 5);
            float x = 50;
            float y = layer.VisibleBoundsWorldspace.MaxY - (menu.ContentSize.Height*0.5f) - 550;
            
            menu.Position = new CCPoint(x, y);

            layer.AddChild(menu);

        }

        private void BackButton_Action(object obj)
        {
            GameController.GoToScene(new TitleScene(GameView));
        }

        private void LevelSelectButton_Action(object obj)
        {
            LevelManager.Instance.CurrentLevel = (int)((CCMenuItem)obj).UserData;
            GameController.GoToScene(new GameScene(_gameView));
        }


        private void AlignItemsInGrid(CCMenu menu, CCPoint padding, int columns)
        {

            CCMenuItem firstItem = (CCMenuItem) menu.Children[0];
            float contentWidth = firstItem.ContentSize.Width*firstItem.ScaleX;
            float contentHeight = firstItem.ContentSize.Height * firstItem.ScaleY;

            int count = menu.ChildrenCount;
            int numRows = (count + columns - 1)/columns;
            int numCols = Math.Min(count, columns);
            float height = contentHeight*numRows + padding.Y*(numRows - 1);
            float width = contentWidth*numCols + padding.X*(numCols - 1);

            menu.ContentSize = new CCSize(width, height);

            int row = 0;
            int col = 0;

            foreach (CCNode item in menu.Children)
            {
                float x = (contentWidth + padding.X) * col + contentWidth * 0.5f;
                float y = height - (contentHeight + padding.Y) * row - contentHeight * 0.5f;

                item.Position = new CCPoint(x, y);

                if (col++ == columns)
                {
                    col = 0;
                    row++;
                }
            }

        }

//        (void)alignItemsInGridWithPadding:(CGPoint)padding columns:(NSInteger)columns
//{
//    CCMenuItem* item = [children_ objectAtIndex: 0];
//        CGFloat contentWidth = item.contentSize.width * item.scaleX;
//        CGFloat contentHeight = item.contentSize.height * item.scaleY;

//        // set content size
//        NSInteger count = children_.count;
//        NSInteger numRows = (count + columns - 1) / columns;
//        NSInteger numCols = MIN(count, columns);
//        CGFloat height = contentHeight * numRows + padding.y * (numRows - 1);
//        CGFloat width = contentWidth * numCols + padding.x * (numCols - 1);
//        [self setContentSize:CGSizeMake(width, height)];
 
//    // layout menu items
//    NSInteger row = 0;
//        NSInteger col = 0;
//    CCARRAY_FOREACH(children_, item)
//        {
//            CGFloat x = (contentWidth + padding.x) * col + contentWidth * 0.5f;
//            CGFloat y = height - (contentHeight + padding.y) * row - contentHeight * 0.5f;
//        [item setPosition:ccp(x, y)];
 
//        if(++col == columns) {
//            col = 0;
//            row++;
//        }
//}
//}
 


    }
}
