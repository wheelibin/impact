using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Factories;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public sealed class Paddle : CCNode
    {
        public Paddle()
        {
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
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

        public void EnableBullets()
        {
            Schedule(FireBullet, 0.35f);
        }

        public void DisableBullets()
        {
            Unschedule(FireBullet);
        }

        private void HandleInput(List<CCTouch> touches, CCEvent touchEvent)
        {
            if (touches.Count > 0)
            {
                CCTouch firstTouch = touches[0];
              
                PositionX = firstTouch.Location.X;

                //Don't allow paddle to go off the left side of the screen
                if (PositionX - (ContentSize.Width/2) <= 0)
                {
                    PositionX = ContentSize.Width/2;
                }
                //or the right
                if (PositionX + (ContentSize.Width / 2) >= GameConstants.WorldWidth)
                {
                    PositionX = GameConstants.WorldWidth - ContentSize.Width / 2;
                }

            }
        }

        private void FireBullet(float frameTime)
        {
            BulletFactory.Instance.CreateNew(Position);
        }
    }
}
