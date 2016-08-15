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

            CCMenu menu = new CCMenu(playbutton, levelSelectbutton)
            {
                Position = new CCPoint(layer.VisibleBoundsWorldspace.Size.Width / 2, layer.VisibleBoundsWorldspace.Size.Height - 950)
            };
            menu.AlignItemsVertically(75);
            layer.AddChild(menu);
            
            //frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImageBall);
            //var zoomingStars = new CCParticleSystemQuad(1000, CCEmitterMode.Gravity)
            //{
            //    SourcePosition = layer.VisibleBoundsWorldspace.Center,
            //    Texture = frame.Texture,
            //    PositionVar = new CCPoint(100, 100),
            //    AngleVar = 360,
            //    Speed = 4,
            //    SpeedVar = 30,
            //    RadialAccel = 50,
            //    Life = 10,
            //    LifeVar = 2,
            //    StartSize = 1,
            //    StartSizeVar = 2,
            //    EndSize = 10,
            //    EndSizeVar = 2,
            //    Duration = -1,
            //    BlendFunc = CCBlendFunc.AlphaBlend,
            //    BlendAdditive = true,
            //    StartColor = new CCColor4F(255, 255, 255, 255),
            //    EndColor = new CCColor4F(255, 255, 255, 255)

            //};
            //layer.AddChild(zoomingStars);

        }

        private void PlayButton_Action(object arg)
        {
            GameController.GoToScene(new GameScene(_gameView));
        }

        private void LevelSelectButton_Action(object arg)
        {
            GameController.GoToScene(new LevelSelectScene(_gameView));
        }
        
    }
}
