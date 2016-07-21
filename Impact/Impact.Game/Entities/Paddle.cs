using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public sealed class Paddle : CCNode
    {
        public Paddle()
        {
            var frame = GameManager.Instance.SpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            //AddChild(new CCLayerColor(CCColor4B.Yellow));
            AddChild(sprite);

            ContentSize = sprite.ContentSize;
            PositionX = GameConstants.PaddleInitialPosition.X;
            PositionY = GameConstants.PaddleInitialPosition.Y;
            ScaleX = GameConstants.PaddleScaleX;
            AnchorPoint = CCPoint.AnchorMiddle;

            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesMoved = HandleInput};
            AddEventListener(touchListener, this);

        }

        private void HandleInput(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch firstTouch = touches[0];
                PositionX = firstTouch.Location.X;
            }
        }
    }
}
