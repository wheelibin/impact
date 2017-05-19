using System;
using CocosSharp;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    public class PaddleFactory
    {
        //Singleton
        private static readonly Lazy<PaddleFactory> SelfInstance = new Lazy<PaddleFactory>(() => new PaddleFactory());
        public static PaddleFactory Instance => SelfInstance.Value;

        public event Action<Paddle> PaddleCreated;

        public Paddle CreateNew(CCPoint position)
        {
            Paddle newPaddle = new Paddle(position);
            PaddleCreated?.Invoke(newPaddle);
            return newPaddle;
        }
    }
}
