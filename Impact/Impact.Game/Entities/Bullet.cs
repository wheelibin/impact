using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public class Bullet : Projectile
    {
        public sealed override bool IsDestroyedByBrickCollision { get; set; }

        public Bullet(CCPoint position) : base(GameConstants.SpriteImageBullet, position, GameConstants.BulletVelocity)
        {
            IsDestroyedByBrickCollision = true;
        }
    }
}
