﻿using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Factories;

namespace Impact.Game.Entities.Powerups
{
    public class MultiBallPowerup : Powerup
    {
        private readonly List<Ball> _balls;

        public MultiBallPowerup(string imageFilename, CCPoint initialPosition, List<Ball> balls)
            : base(initialPosition, imageFilename)
        {
            _balls = balls;
        }

        public override void Activate()
        {
            Ball firstBall = _balls.First();

            //create some balls
            for (int i = 0; i < 3; i++)
            {
                BallFactory.Instance.CreateNew(CCRandom.GetRandomFloat(firstBall.VelocityX - 100, firstBall.VelocityX + 100), GameConstants.BallInitialVelocityY * -1, Position);
            }
        }

        public override void Deactivate()
        {
            for (int b = _balls.Count - 1; b >= 1; b--)
            {
                _balls[b].RemoveFromParent();
                _balls.Remove(_balls[b]);
            }
        }
    }
}
