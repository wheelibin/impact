using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities.Powerups
{
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
            _paddle.ScaleX += 0.5f;
            ScheduleOnce(f => _paddle.ScaleX = GameConstants.PaddleScaleX, GameConstants.PowerupLargerPaddleSeconds);
        }

        public override void Deactivate()
        {
            _paddle.ScaleX = GameConstants.PaddleScaleX;
        }
    }
}
