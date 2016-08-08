using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities
{
    public class Rocket : Projectile
    {
        public Rocket(CCPoint position) : base(GameConstants.SpriteImageBullet, position, GameConstants.BulletVelocity)
        {
        }
    }
}
