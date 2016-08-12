using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Enums;
using Impact.Game.Factories;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public sealed class Paddle : CCNode
    {
        public ProjectileType ProjectileType
        {
            get { return _projectileType; }
            set
            {
                _projectileType = value;

                switch (value)
                {
                    case ProjectileType.None:

                        var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
                        _sprite.SpriteFrame = frame;
                        break;
                    case ProjectileType.Bullet:
                        frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddleBullet);
                        _sprite.SpriteFrame = frame;
                        break;
                }
                
            }
        }

        private readonly CCSprite _sprite;
        private ProjectileType _projectileType;

        public Paddle()
        {
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            //AddChild(new CCLayerColor(CCColor4B.Yellow));
            AddChild(_sprite);

            ContentSize = _sprite.ContentSize;
            PositionX = GameConstants.PaddleInitialPosition.X;
            PositionY = GameConstants.PaddleInitialPosition.Y;
            ScaleX = GameConstants.PaddleScaleX;
            AnchorPoint = CCPoint.AnchorMiddle;

            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesMoved = HandleInput};
            AddEventListener(touchListener, this);

        }

        public void FireProjectile()
        {
            const int gunHeight = 35;
            CCPoint bulletStartPosition = new CCPoint(Position.X, Position.Y + gunHeight);
            ProjectileFactory.Instance.CreateNew(ProjectileType, bulletStartPosition);
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

    }
}
