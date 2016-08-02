using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities.Powerups
{
    public abstract class Powerup : CCNode, IPowerup
    {
        private readonly CCSprite _sprite;

        public float VelocityY { get; set; }
        
        protected Powerup(CCPoint initialPosition, string spriteImage)
        {
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImage);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };

            ContentSize = _sprite.ContentSize;
            PositionX = initialPosition.X;
            PositionY = initialPosition.Y;
        }

        public void Drop()
        {
            AddChild(_sprite);
            Schedule(ApplyVelocity);
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            VelocityY += frameTimeInSeconds * -GameConstants.PowerupGravity;
            PositionY += VelocityY * frameTimeInSeconds;
        }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}
