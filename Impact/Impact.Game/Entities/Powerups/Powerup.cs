using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// Base class for powerups.
    /// </summary>
    public abstract class Powerup : CCNode, IPowerup
    {
        private readonly CCSprite _sprite;

        public float VelocityY { get; set; }
        
        protected Powerup(CCPoint initialPosition, string spriteImage)
        {
            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImage);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };

            ContentSize = _sprite.ContentSize;
            PositionX = initialPosition.X;
            PositionY = initialPosition.Y;
        }

        /// <summary>
        /// Shows the sprite and starts the movement
        /// </summary>
        public void Drop()
        {
            AddChild(_sprite);
            Schedule(ApplyVelocity);
        }

        /// <summary>
        /// The scheduled movement function
        /// </summary>
        private void ApplyVelocity(float frameTimeInSeconds)
        {
            VelocityY += frameTimeInSeconds * -GameConstants.PowerupGravity;
            PositionY += VelocityY * frameTimeInSeconds;
        }

        /// <summary>
        /// Specific behaviour to apply when the powerup is activated
        /// </summary>
        public abstract void Activate();

        /// <summary>
        /// Specific behaviour to apply when the powerup is deactivated
        /// </summary>
        public abstract void Deactivate();
    }
}
