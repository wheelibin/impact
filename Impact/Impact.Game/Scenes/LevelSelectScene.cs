using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Helpers;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class LevelSelectScene : CCScene
    {
        private readonly CCGameView _gameView;
        private static readonly CCColor3B ButtonTextColour = new CCColor3B(178, 242,0); //green

        public LevelSelectScene(CCGameView gameView) : base(gameView)
        {
            _gameView = gameView;

            var layer = new CCLayer();
            AddChild(layer);

            //background
            var frame = GameStateManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectBackground.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            layer.AddChild(sprite);

            List<CCMenuItem> menuItems = new List<CCMenuItem>();

            CCSpriteFrame levelSelectButtonFrame = GameStateManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButton.png");
            CCSpriteFrame levelSelectButtonDisabledFrame = GameStateManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButtonDisabled.png");
            
            //Back button
            MenuItemImageWithText backButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, BackButton_Action, "<-", ButtonTextColour);
            menuItems.Add(backButton);

            //Levels
            for (int l = 1; l <= LevelManager.Instance.NumberOfLevels; l++)
            {
                MenuItemImageWithText levelSelectbutton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonDisabledFrame, LevelSelectButton_Action, l.ToString(), ButtonTextColour)
                {
                    UserData = l,
                    Enabled = (l <= Settings.HighestCompletedLevel+1)
                };
                menuItems.Add(levelSelectbutton);
            }

            CCMenu menu = new CCMenu(menuItems.ToArray());

            AlignItemsInGrid(menu, new CCPoint(5, 5), 5);

            const float x = 50;
            float y = (layer.VisibleBoundsWorldspace.MaxY - (menu.ContentSize.Height*0.75f)) - 550;
            
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

    }
}
