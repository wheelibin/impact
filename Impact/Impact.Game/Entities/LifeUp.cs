using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public class LifeUp : CCNode
    {
        public float VelocityY { get; set; }

        public LifeUp(CCPoint initialPosition)
        {
            CCSpriteFrame frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "LifeUp.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };
            
            ContentSize = sprite.ContentSize;
            PositionX = initialPosition.X;
            PositionY = initialPosition.Y;

            AddChild(sprite);
            Schedule(ApplyVelocity);
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            VelocityY += frameTimeInSeconds * -GameConstants.LifeUpGravity;
            PositionY += VelocityY * frameTimeInSeconds;
        }
        
    }
}
