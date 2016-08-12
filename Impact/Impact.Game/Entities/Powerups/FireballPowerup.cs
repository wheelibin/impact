using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that makes the ball cut through bricks without bouncing off them 
    /// </summary>
    public class FireballPowerup : Powerup
    {
        private readonly List<Ball> _balls;

        public FireballPowerup(string imageFilename, CCPoint initialPosition, List<Ball> balls)
            : base(initialPosition, imageFilename)
        {
            _balls = balls;
        }

        public override void Activate()
        {
            foreach (Ball ball in _balls)
            {
                ball.IsFireball = true;
                ball.ScheduleOnce(f =>
                {
                    foreach (Ball b in _balls)
                    {
                        b.IsFireball = false;
                    }
                }, GameConstants.PowerupFireballSeconds);
            }
        }

        public override void Deactivate()
        {
            foreach (Ball ball in _balls)
            {
                ball.IsFireball = false;
            }
        }
    }
}
