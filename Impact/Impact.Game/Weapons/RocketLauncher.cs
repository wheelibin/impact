using Impact.Game.Config;
using Impact.Game.Enums;

namespace Impact.Game.Weapons
{
    public class RocketLauncher : IWeapon
    {
        public ProjectileType ProjectileType => ProjectileType.Rocket;
        public bool IsSingleShot => true;
        public int YOffset => 22;
        public string FireSound => GameConstants.BulletSound;
        public string PaddleImage => GameConstants.SpriteImagePaddleRocketLauncher;
        public string InfoText => "Tap to fire!";
    }
}
