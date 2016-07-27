using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Scenes
{
    public class TitleScene : CCScene
    {
        private readonly CCGameView _gameView;

        public TitleScene(CCGameView gameView) : base(gameView)
        {
            _gameView = gameView;
            var layer = new CCLayer();
            AddChild(layer);

            //Preload the entire spritesheet
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(GameConstants.TitleScreenSpriteSheet, GameConstants.TitleScreenSpriteSheetImage);

            //background
            var frame = GameManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "Impact-TitleScreen.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            layer.AddChild(sprite);

            //Buttons
            CCSpriteFrame playButtonFrame = GameManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "PlayButton.png");
            CCMenuItemImage playbutton = new CCMenuItemImage(playButtonFrame, playButtonFrame, playButtonFrame, PlayButton_Action);

            CCSpriteFrame levelSelectButtonFrame = GameManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "LevelSelectButton.png");
            CCMenuItemImage levelSelectbutton = new CCMenuItemImage(levelSelectButtonFrame, levelSelectButtonFrame, levelSelectButtonFrame, LevelSelectButton_Action);

            CCMenu menu = new CCMenu(playbutton, levelSelectbutton)
            {
                Position = new CCPoint(layer.VisibleBoundsWorldspace.Size.Width / 2, layer.VisibleBoundsWorldspace.Size.Height - 950)
            };
            menu.AlignItemsVertically(75);
            layer.AddChild(menu);
            
        }

        private void PlayButton_Action(object arg)
        {
            if (GameManager.Instance.GameScene == null)
            {
                GameManager.Instance.GameScene = new GameScene(_gameView);
            }
            GameController.GoToScene(GameManager.Instance.GameScene);
        }

        private void LevelSelectButton_Action(object arg)
        {
            if (GameManager.Instance.LevelSelectScene == null)
            {
                GameManager.Instance.LevelSelectScene = new LevelSelectScene(_gameView);
            }
            GameController.GoToScene(GameManager.Instance.LevelSelectScene);
        }
        
    }
}
