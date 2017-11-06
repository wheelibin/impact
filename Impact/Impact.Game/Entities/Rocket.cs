using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public class Rocket : Projectile
    {
        public sealed override bool DestroysBricksOnCollision { get; set; }
        public sealed override bool IsDestroyedByBrickCollision { get; set; }
        public sealed override bool IsManuallyActivated { get; set; }

        public Rocket(CCPoint position) : base(GameConstants.SpriteImageRocket, position, GameConstants.RocketVelocity)
        {
            DestroysBricksOnCollision = true;
            IsDestroyedByBrickCollision = false;
            IsManuallyActivated = false;
        }

    }
}
