using Impact.Game.Enums;

namespace Impact.Game.Weapons
{
    public interface IWeapon
    {
        ProjectileType ProjectileType { get; }
        bool IsSingleShot { get; }

        /// <summary>
        /// The offset to apply to the initial position of the weapon in relation to the paddle
        /// </summary>
        int YOffset { get; }

        string FireSound { get; }
        string PaddleImage { get; }
    }
}
