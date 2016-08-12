using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that makes the paddle larger for a period of time
    /// </summary>
    public class LargerPaddlePowerup : Powerup
    {
        private readonly Paddle _paddle;

        private readonly CCFiniteTimeAction _scaleReset = new CCEaseBounceInOut(new CCScaleTo(1f, GameConstants.PaddleScaleX, GameConstants.PaddleScaleY));

        public LargerPaddlePowerup(string imageFilename, CCPoint initialPosition, Paddle paddle)
            : base(initialPosition, imageFilename)
        {
            _paddle = paddle;
        }

        public override void Activate()
        {
            //Make the paddle bigger and reset the size after a number of seconds
            CCFiniteTimeAction scaleLarger = new CCEaseBounceInOut(new CCScaleTo(1f, _paddle.ScaleX + 0.5f, GameConstants.PaddleScaleY));
            _paddle.RunActions(scaleLarger, new CCDelayTime(GameConstants.PowerupLargerPaddleSeconds), _scaleReset);
        }

        public override void Deactivate()
        {
            RunActions(_scaleReset);
        }
    }
}
