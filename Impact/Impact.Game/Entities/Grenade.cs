using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Factories;

namespace Impact.Game.Entities
{
    public class Grenade : Projectile
    {
        public sealed override bool DestroysBricksOnCollision { get; set; }
        public sealed override bool IsDestroyedByBrickCollision { get; set; }
        public sealed override bool IsManuallyActivated { get; set; }

        public Grenade(CCPoint position) : base(GameConstants.SpriteImageGrenade, position, GameConstants.RocketVelocity)
        {
            DestroysBricksOnCollision = false;
            IsDestroyedByBrickCollision = false;
            IsManuallyActivated = true;
        }

        public override string ActivationSound
        {
            get => GameConstants.GrenadeSound;
        }

        public override void Activate(CCLayer gameLayer, List<Brick> bricks)
        {
            int blastRadius = 75;

            var explosion = new CCParticleExplosion(this.Position, CCEmitterMode.Radius)
            {
                StartRadius = blastRadius,
                TotalParticles = 200,
                Color = new CCColor3B(255, 255, 255),
                AutoRemoveOnFinish = true,
                StartSize = 5
            };
            gameLayer.AddChild(explosion);

            for (int i = bricks.Count - 1; i >= 0; i--)
            {
                Brick brick = bricks[i];

                CCPoint brickCenter = new CCPoint(brick.Position.X + (brick.ContentSize.Width / 2), brick.PositionY + (brick.ContentSize.Height / 2));

                var distance = CCPoint.Distance(brickCenter, this.Position);

                if (distance <= blastRadius)
                {
                    BrickFactory.Instance.DestroyBrick(brick);
                }
            }

            ProjectileFactory.Instance.DestroyProjectile(this);

        }
    }
}
