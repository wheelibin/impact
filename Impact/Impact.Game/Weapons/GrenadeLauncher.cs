using Impact.Game.Config;
using Impact.Game.Enums;

namespace Impact.Game.Weapons
{
    public class GrenadeLauncher : IWeapon
    {
        public ProjectileType ProjectileType => ProjectileType.Grenade;
        public bool IsSingleShot => true;
        public int YOffset => 22;
        public string FireSound => GameConstants.BulletSound;
        public string PaddleImage => GameConstants.SpriteImagePaddleGrenadeLauncher;
        public string InfoText => "Tap to fire, release to explode!";
    }
}
