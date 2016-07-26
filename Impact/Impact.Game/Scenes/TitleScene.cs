using System.Collections.Generic;
using CocosSharp;
using Impact.Entities;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Scenes
{
    public class TitleScene : CCScene
    {
        readonly CCLayer _layer;
        private readonly List<MenuButton> _buttons = new List<MenuButton>();

        public TitleScene(CCGameView gameView) : base(gameView)
        {
            _layer = new CCLayer();
            AddChild(_layer);

            //Preload the entire spritesheet
            CCSpriteFrameCache.SharedSpriteFrameCache.AddSpriteFrames(GameConstants.TitleScreenSpriteSheet, GameConstants.TitleScreenSpriteSheetImage);

            //background
            var frame = GameManager.Instance.TitleScreenSpriteSheet.Frames.Find(item => item.TextureFilename == "Impact-TitleScreen.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            _layer.AddChild(sprite);

            //buttons
            MenuButton playbutton = new MenuButton(GameManager.Instance.TitleScreenSpriteSheet, "PlayButton.png", "",
                () =>
                {
                    GameManager.Instance.GameScene = new GameScene(gameView);
                    GameController.GoToScene(GameManager.Instance.GameScene);
                })
            {
                Position = new CCPoint(150, GameConstants.WorldHeight - 800),
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _layer.AddChild(playbutton);
            _buttons.Add(playbutton);

            MenuButton levelSelectbutton = new MenuButton(GameManager.Instance.TitleScreenSpriteSheet, "LevelSelectButton.png", "",
                () =>
                {
                    GameManager.Instance.LevelSelectScene = new LevelSelectScene(gameView);
                    GameController.GoToScene(GameManager.Instance.LevelSelectScene);
                })
            {
                Position = new CCPoint(150, GameConstants.WorldHeight - 950),
                AnchorPoint = CCPoint.AnchorUpperLeft
            };
            _layer.AddChild(levelSelectbutton);
            _buttons.Add(levelSelectbutton);

            CreateTouchListener();

        }



        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesBegan = OnTouchesBegan };
            _layer.AddEventListener(touchListener);
        }

        private void OnTouchesBegan(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count == 1)
            {
                CCTouch touch = touches[0];
                foreach (MenuButton button in _buttons)
                {
                    if (button.BoundingBoxTransformedToWorld.ContainsPoint(touch.Location))
                    {
                        button.Click();
                    }
                }
            }
        }

    }
}
