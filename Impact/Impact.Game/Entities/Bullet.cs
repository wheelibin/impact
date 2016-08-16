using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a bullet projectile
    /// </summary>
    public class Bullet : Projectile
    {
        public sealed override bool IsSingleShot { get; set; }
        public sealed override bool IsDestroyedByBrickCollision { get; set; }
        public sealed override string FireSound { get; set; }
        
        public Bullet(CCPoint position) : base(GameConstants.SpriteImageBullet, position, GameConstants.BulletVelocity)
        {
            IsSingleShot = false;
            IsDestroyedByBrickCollision = true;
            FireSound = GameConstants.BulletSound;
        }
        
    }
}
