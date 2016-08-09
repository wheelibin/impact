﻿using System;
using CocosSharp;
using Impact.Game.Entities;
using Impact.Game.Enums;

namespace Impact.Game.Factories
{
    /// <summary>
    /// Responsible for the creation of Projectiles
    /// </summary>
    public class ProjectileFactory
    {
        //Singleton
        private static readonly Lazy<ProjectileFactory> SelfInstance = new Lazy<ProjectileFactory>(() => new ProjectileFactory());
        public static ProjectileFactory Instance => SelfInstance.Value;

        public event Action<Projectile> ProjectileCreated;
        public event Action<Projectile> ProjectileDestroyed;

        /// <summary>
        /// Creates a new projectile of the specified type
        /// </summary>
        public Projectile CreateNew(ProjectileType projectileType, CCPoint position)
        {

            Projectile projectile;

            switch (projectileType)
            {
                case ProjectileType.Bullet:
                    projectile = new Bullet(position);
                    break;
                case ProjectileType.Rocket:
                    projectile = new Rocket(position);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ProjectileCreated?.Invoke(projectile);
            return projectile;
        }

        public void DestroyBullet(Projectile bullet)
        {
            ProjectileDestroyed?.Invoke(bullet);
        }

    }
}