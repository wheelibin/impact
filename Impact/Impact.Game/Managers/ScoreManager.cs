using System;
using System.Collections.Generic;
using Impact.Game.Config;
using Impact.Game.Helpers;

namespace Impact.Game.Managers
{
    /// <summary>
    /// Handles everything score related
    /// </summary>
    public sealed class ScoreManager
    {
        public int Score { get; private set; }
        public event Action ScoreUpdated;

        public ScoreManager()
        {
            Score = 0;
            ScoreUpdated?.Invoke();
        }

        /// <summary>
        /// Update the score for a brick being destroyed
        /// </summary>
        public void BrickDestroyed()
        {
            Score += GameConstants.ScoreBonusForDestroyingBrick;
            ScoreUpdated?.Invoke();
        }

        /// <summary>
        /// Update the score for a powerup having been collected
        /// </summary>
        public void PowerupCollected()
        {
            Score += GameConstants.ScoreBonusForCollectingPowerUp;
            ScoreUpdated?.Invoke();
        }

        /// <summary>
        /// Update the score with the collected ScoreUp's value
        /// </summary>
        public void ScoreUpCollected(int scoreInc)
        {
            Score += scoreInc;
            ScoreUpdated?.Invoke();
        }

        /// <summary>
        /// Add a bonus to the score based on how quickly the player completed the level
        /// </summary>
        /// <returns>The bonus amount</returns>
        public int AddTimeBonus(float levelCompletedTime)
        {
            const float maxLevelTime = GameConstants.MaxLevelTimeForBonus;
            const float totalAvailableBonus = GameConstants.MaxTimeBonus;

            var percOfBonus = 1 - (levelCompletedTime / maxLevelTime);

            int bonus = percOfBonus > 0 ? (int)(totalAvailableBonus * percOfBonus) : 0;
            Score += bonus;
            return bonus;
        }

        /// <summary>
        /// Add a bonus to the score based on how many remaining lives the player has
        /// </summary>
        /// <returns>The bonus amount</returns>
        public int AddLivesBonus(int playerLives, int startingLives)
        {
            int bonus = playerLives * GameConstants.ScoreBonusPerRemainingLife;
            Score += bonus;
            return bonus;
        }

        /// <summary>
        /// Get the saved high score for the specified level
        /// </summary>
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

        /// <summary>
        /// Saves the high score for the current level
        /// </summary>
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

        /// <summary>
        /// Reset to 0
        /// </summary>
        public void ResetScore()
        {
            Score = 0;
            ScoreUpdated?.Invoke();
        }
        
    }
}
