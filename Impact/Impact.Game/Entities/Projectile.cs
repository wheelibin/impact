using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Base class for projectiles
    /// </summary>
    public abstract class Projectile : CCNode
    {
        public float VelocityY { get; set; }

        // Is the projectile destroyed when it hits a brick
        public abstract bool IsDestroyedByBrickCollision { get; set; }

        // Is a brick destroyed when the projectile hits it
        public abstract bool DestroysBricksOnCollision { get; set; }

        // Does the projectile get activated (fired/triggered) manually, i.e. with a tap
        public abstract bool IsManuallyActivated { get; set; }

        public virtual string ActivationSound { get; }

        public virtual void Activate(CCLayer gameLayer, List<Brick> bricks) {
            //activate the projectile, e.g. explode a grenade
            //virtual rather than abstract because it's optional
            //game layer passed in to add effects if needed
        }


        protected Projectile(string spriteImageFilename, CCPoint position, float velocityY)
        {
            CCSpriteFrame frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
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
