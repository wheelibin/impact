using System;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    /// <summary>
    /// Responsible for the creation of LifeUp objects
    /// </summary>
    public class LifeUpFactory
    {
        //Singleton
        private static readonly Lazy<LifeUpFactory> SelfInstance = new Lazy<LifeUpFactory>(() => new LifeUpFactory());
        public static LifeUpFactory Instance => SelfInstance.Value;

        public event Action<LifeUp> LifeUpCreated;

        /// <summary>
        /// Creates a new LifeUp at the top of the screen at a random X position
        /// </summary>
        public LifeUp CreateNew()
        {
            var rnd = new Random();
            int randomX = rnd.Next(50, GameConstants.WorldWidth - 50);

            LifeUp newlifeUp = new LifeUp(new CCPoint(randomX, GameConstants.WorldHeight));
            LifeUpCreated?.Invoke(newlifeUp);
            return newlifeUp;
        }
    }
}
