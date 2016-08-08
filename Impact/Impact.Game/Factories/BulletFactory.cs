using System;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    /// <summary>
    /// Responsible for the creation of ScoreUp objects
    /// </summary>
    public class BulletFactory
    {
        //Singleton
        private static readonly Lazy<BulletFactory> SelfInstance = new Lazy<BulletFactory>(() => new BulletFactory());
        public static BulletFactory Instance => SelfInstance.Value;

        public event Action<Bullet> BulletCreated;
        public event Action<Bullet> BulletDestroyed;

        /// <summary>
        /// Creates a new Bullet
        /// </summary>
        public Bullet CreateNew(CCPoint position)
        {
            Bullet newBullet = new Bullet(position);
            BulletCreated?.Invoke(newBullet);
            return newBullet;
        }

        public void DestroyBullet(Bullet bullet)
        {
            BulletDestroyed?.Invoke(bullet);
        }

    }
}
