using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Enums;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that causes the paddle to fire bullets for a period of time
    /// </summary>
    public class BulletsPowerup : Powerup
    {
        private readonly Paddle _paddle;

        public BulletsPowerup(CCPoint initialPosition, string spriteImage, Paddle paddle) 
            : base(initialPosition, spriteImage)
        {
            _paddle = paddle;
        }

        /// <summary>
        /// Tells the paddle to begin firing bullets, and schedules it to stop after a defined amount of time
        /// </summary>
        public override void Activate()
        {
            _paddle.ProjectileType = ProjectileType.Bullet;
            ScheduleOnce(x => _paddle.ProjectileType = ProjectileType.None, GameConstants.PowerupBulletsSeconds);
        }

        /// <summary>
        /// Tells the paddle to stop firing bullets
        /// </summary>
        public override void Deactivate()
        {
            _paddle.ProjectileType = ProjectileType.None;
        }

    }
}
