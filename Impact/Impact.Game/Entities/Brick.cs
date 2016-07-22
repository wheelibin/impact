using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;
using Impact.Game.Factories;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public sealed class Brick : CCNode
    {
        private readonly CCSprite _sprite;

        public BrickType BrickType { get; set; }
        public Powerup Powerup { get; set; }
        public int HitsToDestroy { get; set; }
        public int HitsTaken { get; set; }

        public Brick(string spriteImageFilename, CCPoint position, float scale, int hitsToDestroy, Powerup powerup)
        {

            BrickType = hitsToDestroy == -1 ? BrickType.Indistructible : BrickType.Normal;

            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            AddChild(_sprite);

            if (BrickType == BrickType.Indistructible)
            {
                //hack to prevent balls from squeezing through indestructable brick gaps
                var size = _sprite.ContentSize;
                size.Width += GameConstants.BrickGap;
                ContentSize = size;
            }
            else
            {
                ContentSize = _sprite.ContentSize;
            }
            
            AnchorPoint = CCPoint.AnchorLowerLeft;
            PositionX = position.X;
            PositionY = position.Y;
            ScaleX = scale;
            ScaleY = scale;

            HitsToDestroy = hitsToDestroy;
            Powerup = powerup;
            HitsTaken = 0;
        }

        public bool Hit()
        {
            if (BrickType == BrickType.Indistructible)
            {
                return false;
            }

            HitsTaken += 1;

            double opacity = 1 - HitsTaken / (double)HitsToDestroy;

            if (HitsTaken >= HitsToDestroy)
            {
                BrickFactory.Instance.DestroyBrick(this);
                return true;
            }

            _sprite.Opacity = (byte)(255 * opacity);

            return false;
        }
        
    }
}
