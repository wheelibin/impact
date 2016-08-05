using System;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    /// <summary>
    /// Responsible for the creation of ScoreUp objects
    /// </summary>
    public class ScoreUpFactory
    {
        //Singleton
        private static readonly Lazy<ScoreUpFactory> SelfInstance = new Lazy<ScoreUpFactory>(() => new ScoreUpFactory());
        public static ScoreUpFactory Instance => SelfInstance.Value;

        public event Action<ScoreUp> ScoreUpCreated;

        /// <summary>
        /// Creates a new ScoreUp at the top of the screen at a random X position and gives it a random score value
        /// </summary>
        public ScoreUp CreateNew()
        {
            var rnd = new Random();
            int randomX = rnd.Next(50, GameConstants.WorldWidth-50);

            //Multiple of ten between 10 and 100
            int randomScore = rnd.Next(1, 10)*10;

            ScoreUp newScoreUp = new ScoreUp(new CCPoint(randomX, GameConstants.WorldHeight), randomScore);
            ScoreUpCreated?.Invoke(newScoreUp);
            return newScoreUp;
        }

    }
}
