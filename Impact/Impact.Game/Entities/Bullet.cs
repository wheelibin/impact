using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public class Bullet : CCNode
    {
        public float VelocityY { get; set; }

        public Bullet(CCPoint position)
        {
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImageBullet);
            var sprite = new CCSprite(frame);

            AddChild(sprite);

            Position = position;
            VelocityY = GameConstants.BulletVelocity;
            Schedule(ApplyVelocity);
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            PositionY += VelocityY * frameTimeInSeconds;
        }

    }
}
