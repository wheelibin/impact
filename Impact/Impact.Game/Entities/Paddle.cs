using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Enums;
using Impact.Game.Factories;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents the paddle
    /// </summary>
    public sealed class Paddle : CCNode
    {
        /// <summary>
        /// The optional projectile type the paddle is currently capable of firing.
        /// The sprite will be changed depending on the type of projectile.
        /// </summary>
        public ProjectileType ProjectileType
        {
            get { return _projectileType; }
            set
            {
                _projectileType = value;

                switch (value)
                {
                    case ProjectileType.None:

                        var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
                        _sprite.SpriteFrame = frame;
                        break;
                    case ProjectileType.Bullet:
                        frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddleBullet);
                        _sprite.SpriteFrame = frame;
                        break;
                    case ProjectileType.Rocket:
                        frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddleRocket);
                        _sprite.SpriteFrame = frame;
                        break;
                }
                
            }
        }

        private readonly CCSprite _sprite;
        private ProjectileType _projectileType;

        public Paddle()
        {
            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            AddChild(_sprite);

            ContentSize = _sprite.ContentSize;
            PositionX = GameConstants.PaddleInitialPosition.X;
            PositionY = GameConstants.PaddleInitialPosition.Y;
            ScaleX = GameConstants.PaddleScaleX;
            AnchorPoint = CCPoint.AnchorMiddle;

            var touchListener = new CCEventListenerTouchAllAtOnce {OnTouchesMoved = HandleInput};
            AddEventListener(touchListener, this);

        }

        /// <summary>
        /// Fires a projectile of the defined type
        /// </summary>
        public void FireProjectile()
        {
            int gunHeight = ProjectileType == ProjectileType.Bullet ? 35 : 22;
            CCPoint bulletStartPosition = new CCPoint(Position.X, Position.Y + gunHeight);
            ProjectileFactory.Instance.CreateNew(ProjectileType, bulletStartPosition);
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
