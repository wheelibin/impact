using System.Collections.Generic;
using CocosSharp;

namespace Impact.Scenes
{
    public class TitleScene : CCScene
    {
        readonly CCLayer _layer;

        public TitleScene(CCGameView gameView) : base(gameView)
        {
            _layer = new CCLayer();
            this.AddChild(_layer);

            CreateText();

            CreateTouchListener();

        }

        private void CreateText()
        {
            var label = new CCLabel("Tap to begin", "Arial", 30, CCLabelFormat.SystemFont)
            {
                PositionX = _layer.ContentSize.Width/2.0f,
                PositionY = _layer.ContentSize.Height/2.0f
            };

            _layer.AddChild(label);
        }

        private void CreateTouchListener()
        {
            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesBegan = HandleTouchesBegan};
            _layer.AddEventListener(touchListener);
        }

        private void HandleTouchesBegan(List<CCTouch> arg1, CCEvent arg2)
        {
            var newScene = new GameScene(GameController.GameView);
            GameController.GoToScene(newScene);
        }

    }
}
