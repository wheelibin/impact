using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Helpers;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class SettingsScene : CCScene
    {
        private readonly CCGameView _gameView;
        private static readonly CCColor3B ButtonTextColour = GameConstants.ImpactGreen;

        private readonly CCLabel _musicVolumeLabel;
        private readonly CCLabel _sfxVolumeLabel;
        private readonly MenuItemImageWithText _musicVolumeMinusButton;
        private readonly MenuItemImageWithText _musicVolumePlusButton;
        private readonly MenuItemImageWithText _sfxVolumeMinusButton;
        private readonly MenuItemImageWithText _sfxVolumePlusButton;

        public SettingsScene(CCGameView gameView) : base(gameView)
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

            CCSpriteFrame levelSelectButtonFrame = GameStateManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButton.png");
            CCSpriteFrame levelSelectButtonDisabledFrame = GameStateManager.Instance.LevelSelectScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButtonDisabled.png");

            //Music Volume
            CCLabel backgroundMusicLabel = new CCLabel("MUSIC VOLUME:", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                HorizontalAlignment = CCTextAlignment.Left,
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = 50,
                PositionY = GameConstants.WorldTop - 400
            };
            layer.AddChild(backgroundMusicLabel);

            _musicVolumeMinusButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonDisabledFrame, MusicVolumeMinusButton_Action, "-", ButtonTextColour)
            {
                PositionX = 75,
                PositionY = 175,
                Enabled = (Settings.MusicVolume > 1)
            };

            _musicVolumeLabel = new CCLabel(Settings.MusicVolume.ToString(), "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                PositionX = 600,
                PositionY = GameConstants.WorldTop - 400
            };
            layer.AddChild(_musicVolumeLabel);
            
            _musicVolumePlusButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonDisabledFrame, MusicVolumePlusButton_Action, "+", ButtonTextColour)
            {
                PositionX = 300,
                PositionY = 175,
                Enabled = (Settings.MusicVolume < GameConstants.MusicVolumeMax)
            };

            //Sfx Volume
            CCLabel sfxLabel = new CCLabel("SFX VOLUME:", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                HorizontalAlignment = CCTextAlignment.Left,
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = 50,
                PositionY = GameConstants.WorldTop - 600
            };
            layer.AddChild(sfxLabel);

            _sfxVolumeMinusButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonDisabledFrame, SfxVolumeMinusButton_Action, "-", ButtonTextColour)
            {
                PositionX = 75,
                PositionY = -25,
                Enabled = (Settings.SfxVolume > 1)
            };

            _sfxVolumeLabel = new CCLabel(Settings.SfxVolume.ToString(), "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                PositionX = 600,
                PositionY = GameConstants.WorldTop - 600
            };
            layer.AddChild(_sfxVolumeLabel);

            _sfxVolumePlusButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonDisabledFrame, SfxVolumePlusButton_Action, "+", ButtonTextColour)
            {
                PositionX = 300,
                PositionY = -25,
                Enabled = (Settings.SfxVolume < GameConstants.SfxVolumeMax)
            };

            //Back button
            MenuItemImageWithText backButton = new MenuItemImageWithText(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, BackButton_Action, "<-", ButtonTextColour)
            {
                PositionX = -375,
                PositionY = -200,
                AnchorPoint = CCPoint.AnchorMiddleLeft
            };
            

            CCMenu menu = new CCMenu(_musicVolumeMinusButton, _musicVolumePlusButton, _sfxVolumeMinusButton, _sfxVolumePlusButton, backButton);
            
            layer.AddChild(menu);

        }

        private void BackButton_Action(object obj)
        {
            GameController.GoToScene(new TitleScene(GameView));
        }

        private void MusicVolumeMinusButton_Action(object obj)
        {
            int volume = Settings.MusicVolume;
            volume -= 1;

            SetMusicVolume(volume);
        }

        private void MusicVolumePlusButton_Action(object obj)
        {
            int volume = Settings.MusicVolume;
            volume += 1;

            SetMusicVolume(volume);
        }

        private void SfxVolumeMinusButton_Action(object obj)
        {
            int volume = Settings.SfxVolume;
            volume -= 1;

            SetSfxVolume(volume);
        }

        private void SfxVolumePlusButton_Action(object obj)
        {
            int volume = Settings.SfxVolume;
            volume += 1;

            SetSfxVolume(volume);
        }

        private void SetMusicVolume(int volume)
        {
            Settings.MusicVolume = volume;
            _musicVolumeLabel.Text = volume.ToString();

            _musicVolumeMinusButton.Enabled = (volume > 1);
            _musicVolumePlusButton.Enabled = (volume < GameConstants.MusicVolumeMax);
        }

        private void SetSfxVolume(int volume)
        {
            Settings.SfxVolume = volume;
            _sfxVolumeLabel.Text = volume.ToString();

            _sfxVolumeMinusButton.Enabled = (volume > 1);
            _sfxVolumePlusButton.Enabled = (volume < GameConstants.SfxVolumeMax);
        }

    }
}
