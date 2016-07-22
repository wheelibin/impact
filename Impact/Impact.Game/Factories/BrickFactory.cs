using System;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;

namespace Impact.Game.Factories
{
    public class BrickFactory
    {
        private static readonly Lazy<BrickFactory> SelfInstance = new Lazy<BrickFactory>(() => new BrickFactory());

        // simple singleton implementation
        public static BrickFactory Instance => SelfInstance.Value;

        public event Action<Brick> BrickCreated;
        public event Action<Brick> BrickDestroyed;

        public Brick CreateNew(string spriteImageFilename, CCPoint position, float scale, int hitsToDestroy, Powerup powerup)
        {
            Brick newBrick = new Brick(spriteImageFilename, position, scale, hitsToDestroy, powerup);
            BrickCreated?.Invoke(newBrick);
            return newBrick;
        }
        
        public void DestroyBrick(Brick brick)
        {
            BrickDestroyed?.Invoke(brick);
        }
    }
}
