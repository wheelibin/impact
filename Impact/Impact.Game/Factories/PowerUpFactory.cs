using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;

namespace Impact.Game.Factories
{
    /// <summary>
    /// Responsible for the creation of PowerUp objects
    /// </summary>
    public class PowerUpFactory
    {
        //Singleton
        private static readonly Lazy<PowerUpFactory> SelfInstance = new Lazy<PowerUpFactory>(() => new PowerUpFactory());
        public static PowerUpFactory Instance => SelfInstance.Value;

        public event Action<Powerup> PowerupDestroyed;

        /// <summary>
        /// Creates a new Powerup at the top of the screen at a random X position and gives it a random score value
        /// </summary>
        public Powerup CreateNew(PowerupType powerupType, string imageFilename, CCPoint initialPosition, Paddle paddle, List<Ball> balls)
        {

            Powerup powerup = null;

            switch (powerupType)
            {
                case PowerupType.LargerPaddle:
                    powerup = new LargerPaddlePowerup(imageFilename, initialPosition, paddle);
                    break;
                case PowerupType.Multiball:
                    powerup = new MultiBallPowerup(imageFilename, initialPosition, balls);
                    break;
                case PowerupType.FireBall:
                    powerup = new FireballPowerup(imageFilename, initialPosition, balls);
                    break;
                case PowerupType.Bullets:
                    powerup = new GunPowerup(imageFilename, initialPosition, paddle);
                    break;
                case PowerupType.Rockets:
                    powerup = new RocketLauncherPowerup(imageFilename, initialPosition, paddle);
                    break;
                case PowerupType.ExtraLife:
                    powerup = new ExtraLifePowerup(imageFilename, initialPosition);
                    break;
                case PowerupType.Grenades:
                    powerup = new GrenadeLauncherPowerup(imageFilename, initialPosition, paddle);
                    break;
            }

            return powerup;
        }

        public void DestroyPowerup(Powerup powerup)
        {
            PowerupDestroyed?.Invoke(powerup);
        }

    }
}
