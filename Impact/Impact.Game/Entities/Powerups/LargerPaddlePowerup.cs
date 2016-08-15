using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that makes the paddle larger for a period of time
    /// </summary>
    public class LargerPaddlePowerup : Powerup
    {
        private const float PaddleScaleAdjustment = 0.5f;
        private readonly Paddle _paddle;

        public LargerPaddlePowerup(string imageFilename, CCPoint initialPosition, Paddle paddle)
            : base(initialPosition, imageFilename)
        {
            _paddle = paddle;
        }

        public override void Activate()
        {
            float previousPaddleScaleX = _paddle.ScaleX;

            //Make the paddle bigger and reset the size after a number of seconds
            CCFiniteTimeAction scaleLarger = new CCEaseBounceInOut(new CCScaleTo(1f, _paddle.ScaleX + PaddleScaleAdjustment, GameConstants.PaddleScaleY));
            _paddle.RunActions(scaleLarger, new CCDelayTime(GameConstants.PowerupLargerPaddleSeconds), GetResetAction(previousPaddleScaleX));
        }

        public override void Deactivate()
        {
            RunActions(GetResetAction(GameConstants.PaddleScaleX));
        }

        /// <summary>
        /// Get the scale reset action.
        /// Reduces the supplied original scale by PaddleScaleAdjustment (unless the paddle is already at it's smallest scale).
        /// By doing this we can collect multiple copies of this powerup and scale up and down accordingly
        /// </summary>
        private CCFiniteTimeAction GetResetAction(float previousPaddleScaleX)
        {
            float newScaleX = previousPaddleScaleX > GameConstants.PaddleScaleX
                ? previousPaddleScaleX - PaddleScaleAdjustment
                : GameConstants.PaddleScaleX;
            
            return new CCEaseBounceInOut(new CCScaleTo(1f, newScaleX, GameConstants.PaddleScaleY));
        }
    }
}
