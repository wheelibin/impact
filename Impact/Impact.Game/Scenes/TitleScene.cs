using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Scenes
{
    public class TitleScene : CCScene
    {
        private readonly CCGameView _gameView;

        public TitleScene(CCGameView gameView) : base(gameView)
        {
            _gameView = gameView;
            var layer = new CCLayer();
            AddChild(layer);

            //background
            var frame = GameStateManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "Impact-TitleScreen.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            layer.AddChild(sprite);

            //Buttons
            CCSpriteFrame playButtonFrame = GameStateManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "PlayButton.png");
            CCMenuItemImage playbutton = new CCMenuItemImage(playButtonFrame, playButtonFrame, playButtonFrame, PlayButton_Action);

            CCSpriteFrame levelSelectButtonFrame = GameStateManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "TitleScreenLevelSelectButton.png");
            CCMenuItemImage levelSelectbutton = new CCMenuItemImage(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, LevelSelectButton_Action);

            CCSpriteFrame settingsButtonFrame = GameStateManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "SettingsButton.png");
            CCMenuItemImage settingsbutton = new CCMenuItemImage(settingsButtonFrame, settingsButtonFrame, settingsButtonFrame, SettingsButton_Action);

            CCMenu menu = new CCMenu(playbutton, levelSelectbutton, settingsbutton)
            {
                Position = new CCPoint(layer.VisibleBoundsWorldspace.Size.Width / 2, layer.VisibleBoundsWorldspace.Size.Height - 975)
            };
            menu.AlignItemsVertically(37);
            layer.AddChild(menu);
            
        }

        private void PlayButton_Action(object arg)
        {
            GameController.GoToScene(new GameScene(_gameView));
        }

        private void LevelSelectButton_Action(object arg)
        {
            GameController.GoToScene(new LevelSelectScene(_gameView));
        }

        private void SettingsButton_Action(object arg)
        {
            GameController.GoToScene(new SettingsScene(_gameView));
        }

    }
}
