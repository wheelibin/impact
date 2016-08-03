﻿namespace Impact.Game.Managers
{
    public class ScoreManager : IScoreManager
    {
        public int Score { get; private set; }

        public ScoreManager()
        {
            Score = 0;
        }

        public void BrickDestroyed()
        {
            Score += 10;
        }

        public void PowerupCollected()
        {
            Score += 20;
        }

        public void ScoreUpCollected(int scoreInc)
        {
            Score += scoreInc;
        }
    }
}