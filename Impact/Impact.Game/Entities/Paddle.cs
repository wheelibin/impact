using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Factories;
using Impact.Game.Managers;
using Impact.Game.Weapons;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents the paddle
    /// </summary>
    public sealed class Paddle : CCNode
    {
        public IWeapon Weapon
        {
            get { return _weapon; }
            set
            {
                _weapon = value;

                string paddleImageTexture = GameConstants.SpriteImagePaddle;
                if (_weapon != null)
                {
                    paddleImageTexture = _weapon.PaddleImage;
                }

                CCSpriteFrame frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == paddleImageTexture);
                _sprite.SpriteFrame = frame;
            }
        }

        private readonly CCSprite _sprite;
        private IWeapon _weapon;

        public Paddle(CCPoint position)
        {
            CCSpriteFrame frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImagePaddle);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            AddChild(_sprite);

            ContentSize = _sprite.ContentSize;
            Position = position;
            ScaleX = GameConstants.PaddleScaleX;
            AnchorPoint = CCPoint.AnchorMiddle;
        }

        /// <summary>
        /// Fires a weapon of the defined type
        /// </summary>
        public void FireProjectile()
        {
            CCPoint bulletStartPosition = new CCPoint(Position.X, Position.Y + Weapon.YOffset);
            ProjectileFactory.Instance.CreateNew(Weapon.ProjectileType, bulletStartPosition);
        }

    }
}
