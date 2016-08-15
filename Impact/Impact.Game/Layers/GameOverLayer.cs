using System;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;
using Impact.Game.Scenes;

namespace Impact.Game.Layers
{
    public class GameOverLayer : CCLayerColor
    {
        public event Action PlayButtonPressed;
        public event Action MainMenuButtonPressed;

        public GameOverLayer() : base(new CCColor4B(0, 0, 0, 128))
        {
            //background
            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "GameOverPopupBackground.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };

            float popupTop = GameConstants.WorldHeight - 410;
            float popupX = (float)(GameConstants.WorldWidth * 0.5);
            float popupY = (float)(popupTop - (sprite.ContentSize.Height * 0.5));

            sprite.Position = new CCPoint(popupX, popupY);

            AddChild(sprite);

            CCLabel levelLabel = new CCLabel("GAME OVER", "visitor1.ttf", 96, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddle,
                PositionX = popupX,
                PositionY = popupTop - 50
            };
            AddChild(levelLabel);
            
            //Buttons
            CCSpriteFrame playButtonFrame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "GameOverPopupPlayButton.png");
            CCMenuItemImage playbutton = new CCMenuItemImage(playButtonFrame, playButtonFrame, playButtonFrame, PlayButton_Action);

            CCSpriteFrame levelSelectButtonFrame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "NewLevelPopupMainMenuButton.png");
            CCMenuItemImage levelSelectbutton = new CCMenuItemImage(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, MainMenuButton_Action);

            CCMenu menu = new CCMenu(playbutton, levelSelectbutton)
            {
                Position = new CCPoint(popupX, popupY - 50)
            };

            menu.AlignItemsVertically(25);
            AddChild(menu);

        }

        private void MainMenuButton_Action(object obj)
        {
            MainMenuButtonPressed?.Invoke();
        }

        private void PlayButton_Action(object obj)
        {
            PlayButtonPressed?.Invoke();
        }
    }
}
