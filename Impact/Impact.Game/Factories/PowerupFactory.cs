//using System;
//using CocosSharp;
//using Impact.Game.Config;
//using Impact.Game.Entities;
//using Impact.Game.Enums;

//namespace Impact.Game.Factories
//{
//    public class PowerupFactory
//    {
//        private static readonly Lazy<PowerupFactory> SelfInstance = new Lazy<PowerupFactory>(() => new PowerupFactory());

//        // simple singleton implementation
//        public static PowerupFactory Instance => SelfInstance.Value;

//        public event Action<Brick> BrickCreated;
//        public event Action<Brick> BrickDestroyed;

//        public T CreateNew<T>(CCPoint position) where T:Powerup
//        {
//            Brick newBrick = new Brick(spriteImageFilename, position, scale, brickType, powerupType);
//            BrickCreated?.Invoke(newBrick);
//            return newBrick;
//        }

//        public void DestroyBrick(Brick brick)
//        {
//            BrickDestroyed?.Invoke(brick);
//        }
//    }
//}
