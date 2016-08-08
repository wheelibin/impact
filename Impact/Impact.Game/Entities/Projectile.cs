using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public abstract class Projectile : CCNode
    {
        public float VelocityY { get; set; }

        protected Projectile(string spriteImageFilename, CCPoint position, float velocityY)
        {
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
            var sprite = new CCSprite(frame);

            AddChild(sprite);

            Position = position;
            VelocityY = velocityY;
            Schedule(ApplyVelocity);
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            PositionY += VelocityY * frameTimeInSeconds;
        }

    }
}
