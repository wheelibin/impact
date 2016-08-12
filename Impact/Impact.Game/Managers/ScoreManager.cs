using System;
using System.Collections.Generic;
using Impact.Game.Helpers;

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

        public int GetHighScoreForLevel(int level)
        {
            int highScore = 0;
            if (!string.IsNullOrEmpty(Settings.HighScores))
            {
                Dictionary<int, int> scores = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, int>>(Settings.HighScores);
                if (scores.ContainsKey(level))
                {
                    highScore = scores[level];
                }
            }
            return highScore;
        }

        public void SaveCurrentLevelHighScore()
        {
            int currentLevel = LevelManager.Instance.CurrentLevel;

            Dictionary<int, int> scores;
            if (string.IsNullOrEmpty(Settings.HighScores))
            {
                scores = new Dictionary<int, int>();
            }
            else
            {
                scores = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int, int>>(Settings.HighScores);
            }
            
            if (scores.ContainsKey(currentLevel))
            {
                if (scores[currentLevel] < Score)
                {
                    scores[currentLevel] = Score;
                }
            }
            else
            {
                scores.Add(currentLevel, Score);
            }

            Settings.HighScores = Newtonsoft.Json.JsonConvert.SerializeObject(scores);
        }

        public void ResetScore()
        {
            Score = 0;
            ScoreUpdated?.Invoke();
        }
    }
}
