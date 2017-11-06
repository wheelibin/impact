using Impact.Game.Config;
using Impact.Game.Enums;

namespace Impact.Game.Weapons
{
    public class Gun : IWeapon
    {
        public ProjectileType ProjectileType => ProjectileType.Bullet;
        public bool IsSingleShot => false;
        public int YOffset => 35;
        public string FireSound => GameConstants.BulletSound;
        public string PaddleImage => GameConstants.SpriteImagePaddleGun;
        public string InfoText => "Tap to fire!";
    }
}
