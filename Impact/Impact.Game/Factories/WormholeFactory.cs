﻿using System;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Enums;

namespace Impact.Game.Factories
{
    public class WormholeFactory
    {
        //Singleton
        private static readonly Lazy<WormholeFactory> SelfInstance = new Lazy<WormholeFactory>(() => new WormholeFactory());
        public static WormholeFactory Instance => SelfInstance.Value;

        public event Action<Wormhole> WormholeCreated;

        public Wormhole CreateNew(string spriteImageFilename, CCPoint position, WormholeType wormholeType, string objectName, string exitName, WormholeExitDirection exitDirection)
        {
            Wormhole newWormhole = new Wormhole(spriteImageFilename, position, wormholeType, objectName, exitName, exitDirection);
            WormholeCreated?.Invoke(newWormhole);
            return newWormhole;
        }
        
    }
}
