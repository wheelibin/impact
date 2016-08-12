using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public class Rocket : Projectile
    {
        public override sealed bool IsSingleShot { get; set; }
        public override sealed bool IsDestroyedByBrickCollision { get; set; }

        public Rocket(CCPoint position) : base(GameConstants.SpriteImageRocket, position, GameConstants.RocketVelocity)
        {
            IsSingleShot = true;
            IsDestroyedByBrickCollision = false;
        }
    }
}
