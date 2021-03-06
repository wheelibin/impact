﻿using System;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;

namespace Impact.Game.Factories
{
    public class BrickFactory
    {
        //Singleton
        private static readonly Lazy<BrickFactory> SelfInstance = new Lazy<BrickFactory>(() => new BrickFactory());
        public static BrickFactory Instance => SelfInstance.Value;

        public event Action<Brick> BrickCreated;
        public event Action<Brick> BrickDestroyed;

        public Brick CreateNew(string spriteImageFilename, CCPoint position, float scale, int hitsToDestroy, Powerup powerup, float bounceFactor, BrickType brickType, bool doubleSizeBrick)
        {
            Brick newBrick = new Brick(spriteImageFilename, position, scale, hitsToDestroy, powerup, bounceFactor, brickType, doubleSizeBrick);
            BrickCreated?.Invoke(newBrick);
            return newBrick;
        }
        
        public void DestroyBrick(Brick brick)
        {
            BrickDestroyed?.Invoke(brick);
        }

    }
}
