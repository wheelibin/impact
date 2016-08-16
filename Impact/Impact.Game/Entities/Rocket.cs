using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a rocket projectile
    /// </summary>
    public class Rocket : Projectile
    {
        public sealed override bool IsSingleShot { get; set; }
        public sealed override bool IsDestroyedByBrickCollision { get; set; }
        public sealed override string FireSound { get; set; }

        public Rocket(CCPoint position) : base(GameConstants.SpriteImageRocket, position, GameConstants.RocketVelocity)
        {
            IsSingleShot = true;
            IsDestroyedByBrickCollision = false;
            FireSound = GameConstants.BulletSound;
        }
    }
}
