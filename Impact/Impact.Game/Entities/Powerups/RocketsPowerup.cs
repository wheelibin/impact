using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Enums;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that causes the paddle to fire rockets for a period of time
    /// </summary>
    public class RocketsPowerup : Powerup
    {
        private readonly Paddle _paddle;

        public RocketsPowerup(string spriteImage, CCPoint initialPosition, Paddle paddle)
            : base(initialPosition, spriteImage)
        {
            _paddle = paddle;
        }

        /// <summary>
        /// Tells the paddle to allow the firing of rockets, and schedules it to stop after a defined amount of time
        /// </summary>
        public override void Activate()
        {
            _paddle.ProjectileType = ProjectileType.Rocket;
            _paddle.ScheduleOnce(x => _paddle.ProjectileType = ProjectileType.None, GameConstants.PowerupRocketsSeconds);
        }

        /// <summary>
        /// Tells the paddle to stop allowing the firing of rockets
        /// </summary>
        public override void Deactivate()
        {
            _paddle.ProjectileType = ProjectileType.None;
        }

    }
}
