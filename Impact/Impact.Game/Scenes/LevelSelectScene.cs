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
        private readonly List<MenuButton> _buttons;

        public LevelSelectScene(CCGameView gameView) : base(gameView)
        {
            _gameView = gameView;

            _buttons = new List<MenuButton>();

            var layer = new CCLayerColor(GameConstants.BackgroundColour);
            AddChild(layer);

            var label = new CCLabel("Select level", "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                Color = new CCColor3B(123,129,131),
                PositionX = 50,
                PositionY = gameView.DesignResolution.Height - 50,
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            layer.AddChild(label);

            List<CCMenuItem> menuItems = new List<CCMenuItem>();
            //Back button
            CCSpriteFrame backButtonFrame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectMenuButton.png");
            MenuItemImageWithText backButton = new MenuItemImageWithText(backButtonFrame, backButtonFrame, backButtonFrame, BackButton_Action, "<-");

            menuItems.Add(backButton);
            //Levels
            for (int l = 0; l < LevelManager.Instance.NumberOfLevels; l++)
            {
                CCSpriteFrame levelSelectButtonFrame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectMenuButton.png");
                MenuItemImageWithText levelSelectbutton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, LevelSelectButton_Action, (l+1).ToString())
                {
                    UserData = l + 1
                };
                menuItems.Add(levelSelectbutton);
            }

            CCMenu menu = new CCMenu(menuItems.ToArray())
            {
                Position = new CCPoint(layer.VisibleBoundsWorldspace.Size.Width / 2, layer.VisibleBoundsWorldspace.Size.Height - 300)
            };

            const int itemsPerRow = 5;

            List<uint> itemsPerRows = new List<uint>();

            int fullRows = menuItems.Count / itemsPerRow;
            uint remainder = (uint)(menuItems.Count % itemsPerRow);

            for (int i = 0; i < fullRows; i++)
            {
                itemsPerRows.Add(itemsPerRow);
            }
            itemsPerRows.Add(remainder);

            menu.AlignItemsInColumns(itemsPerRows.ToArray());

            layer.AddChild(menu);

        }

        private void BackButton_Action(object obj)
        {
            GameController.GoToScene(GameManager.Instance.TitleScene);
        }

        private void LevelSelectButton_Action(object obj)
        {
            LevelManager.Instance.CurrentLevel = (int)((CCMenuItem)obj).UserData;

            if (GameManager.Instance.GameScene == null)
            {
                GameManager.Instance.GameScene = new GameScene(_gameView);
            }
            GameController.GoToScene(GameManager.Instance.GameScene);
        }

    }
}
