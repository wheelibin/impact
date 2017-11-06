using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Weapons;

namespace Impact.Game.Entities.Powerups
{
    public class GrenadeLauncherPowerup : Powerup
    {
        private readonly Paddle _paddle;

        public GrenadeLauncherPowerup(string spriteImage, CCPoint initialPosition, Paddle paddle)
            : base(initialPosition, spriteImage)
        {
            _paddle = paddle;
        }

        /// <summary>
        /// Tells the paddle to allow the firing of rockets, and schedules it to stop after a defined amount of time
        /// </summary>
        public override void Activate()
        {
            _paddle.Weapon = new GrenadeLauncher();
            _paddle.ScheduleOnce(x => _paddle.Weapon = null, 20);
        }

        /// <summary>
        /// Tells the paddle to stop allowing the firing of rockets
        /// </summary>
        public override void Deactivate()
        {
            _paddle.Weapon = null;
        }

    }
}
