using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    public class BallFactory
    {
        private static readonly Lazy<BallFactory> SelfInstance = new Lazy<BallFactory>(() => new BallFactory());

        // simple singleton implementation
        public static BallFactory Instance => SelfInstance.Value;

        public event Action<Ball> BallCreated;

        public Ball CreateNew(float? velocityX = null, float? velocityY = null, CCPoint? initialPosition = null)
        {
            Ball ball = new Ball(velocityY, initialPosition)
            {
                VelocityX = velocityX ?? 0
            };

            BallCreated?.Invoke(ball);

            return ball;
        }

        public void ResetBalls(List<Ball> balls)
        {
            foreach (Ball ball in balls)
            {
                ball.VelocityY = GameConstants.BallInitialVelocityY;
                ball.PositionX = GameConstants.BallInitialPosition.X;
                ball.PositionY = GameConstants.BallInitialPosition.Y;
            }
        }
    }
}
