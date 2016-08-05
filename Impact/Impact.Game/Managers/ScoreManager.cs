using System;

namespace Impact.Game.Managers
{
    public class ScoreManager : IScoreManager
    {
        public int Score { get; private set; }
        public event Action ScoreUpdated;

        public ScoreManager()
        {
            Score = 0;
            ScoreUpdated?.Invoke();
        }

        public void BrickDestroyed()
        {
            Score += 10;
            ScoreUpdated?.Invoke();
        }

        public void PowerupCollected()
        {
            Score += 20;
            ScoreUpdated?.Invoke();
        }

        public void ScoreUpCollected(int scoreInc)
        {
            Score += scoreInc;
            ScoreUpdated?.Invoke();
        }
    }
}
