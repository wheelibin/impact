using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public class Bullet : Projectile
    {
        public override sealed bool IsSingleShot { get; set; }
        public override sealed bool IsDestroyedByBrickCollision { get; set; }
        
        public Bullet(CCPoint position) : base(GameConstants.SpriteImageBullet, position, GameConstants.BulletVelocity)
        {
            IsSingleShot = false;
            IsDestroyedByBrickCollision = true;
        }
        
    }
}
