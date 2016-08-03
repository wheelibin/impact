using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that makes the paddle larger for an amount of time
    /// </summary>
    public class LargerPaddlePowerup : Powerup
    {
        private readonly Paddle _paddle;

        public LargerPaddlePowerup(string imageFilename, CCPoint initialPosition, Paddle paddle) 
            : base(initialPosition, imageFilename)
        {
            _paddle = paddle;
        }

        public override void Activate()
        {
            //Make the paddle bigger
            _paddle.ScaleX += 0.5f;

            //Reset the size after a number of seconds
            ScheduleOnce(f => _paddle.ScaleX = GameConstants.PaddleScaleX, GameConstants.PowerupLargerPaddleSeconds);
        }

        public override void Deactivate()
        {
            //Reset the paddle size
            _paddle.ScaleX = GameConstants.PaddleScaleX;
        }
    }
}
