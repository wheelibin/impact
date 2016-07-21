using System.Collections.Generic;
using CocosSharp;
using Impact.Entities;
using Impact.Game;
using Impact.Game.Config;

namespace Impact.Scenes
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

            for (int l = 0; l < LevelManager.Instance.NumberOfLevels; l++)
            {
                var level = l + 1;
                var button = new MenuButton(level, "LevelSelectMenuButton.png", level.ToString(), () =>
                {
                    LevelManager.Instance.CurrentLevel = level;
                    GameController.GoToScene(new GameScene(_gameView));
                })
                {
                    PositionX = level * 90,
                    PositionY = gameView.DesignResolution.Height - 300,
                    AnchorPoint = CCPoint.AnchorUpperLeft
                };

                layer.AddChild(button);
                _buttons.Add(button);
            }

            var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesBegan = OnTouchesBegan };
            AddEventListener(touchListener, layer);

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
