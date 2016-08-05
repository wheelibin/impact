﻿using System;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;
using Impact.Game.Scenes;

namespace Impact.Game.Layers
{
    public class NewLevelLayer : CCLayerColor
    {
        public event Action PlayButtonPressed;

        public NewLevelLayer(int lives) : base(new CCColor4B(0, 0, 0, 128))
        {
            //background
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "NewLevelPopupBackground.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };

            float popupTop = GameConstants.WorldHeight - 410;
            float popupX = (float)(GameConstants.WorldWidth * 0.5);
            float popupY = (float)(popupTop - (sprite.ContentSize.Height * 0.5));

            sprite.Position = new CCPoint(popupX, popupY);

            AddChild(sprite);

            CCLabel levelLabel = new CCLabel($"LEVEL {LevelManager.Instance.CurrentLevel}", "visitor1.ttf", 96, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddle,
                PositionX = popupX,
                PositionY = popupTop - 50
            };
            AddChild(levelLabel);
            //HIGH SCORE: 12345
            //LIVES:      1
            CCLabel scoreLabel = new CCLabel($"HIGH SCORE: 000000", "visitor1.ttf", 40, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = (float) (popupX - (sprite.ContentSize.Width) * 0.5 + 150),
                PositionY = popupTop - 150,
                HorizontalAlignment = CCTextAlignment.Left
            };
            AddChild(scoreLabel);

            CCLabel livesLabel = new CCLabel($"LIVES:      {lives}", "visitor1.ttf", 40, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = (float)(popupX - (sprite.ContentSize.Width) * 0.5 + 150),
                PositionY = popupTop - 200,
                HorizontalAlignment = CCTextAlignment.Left
            };
            AddChild(livesLabel);

            //Buttons
            CCSpriteFrame playButtonFrame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "NewLevelPopupPlayButton.png");
            CCMenuItemImage playbutton = new CCMenuItemImage(playButtonFrame, playButtonFrame, playButtonFrame, PlayButton_Action);

            CCSpriteFrame levelSelectButtonFrame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "NewLevelPopupMainMenuButton.png");
            CCMenuItemImage levelSelectbutton = new CCMenuItemImage(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, MainMenuButton_Action);

            CCMenu menu = new CCMenu(playbutton, levelSelectbutton)
            {
                Position = new CCPoint(popupX, popupY - 125)
            };
            menu.AlignItemsVertically(25);
            AddChild(menu);

        }

        private void MainMenuButton_Action(object obj)
        {
            GameController.GoToScene(new TitleScene(GameView));
        }

        private void PlayButton_Action(object obj)
        {
            PlayButtonPressed?.Invoke();
        }
    }
}
