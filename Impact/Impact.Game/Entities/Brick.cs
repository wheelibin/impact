using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;
using Impact.Game.Factories;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a brick in the game
    /// </summary>
    public sealed class Brick : CCNode
    {
        private readonly CCSprite _sprite;

        public BrickType BrickType { get; set; }
        public Powerup Powerup { get; set; }
        public int HitsToDestroy { get; set; }
        public int HitsTaken { get; set; }
        public float BounceFactor { get; set; }
        public bool IsIndestructible { get; set; }

        public Brick(string spriteImageFilename, CCPoint position, float scale, int hitsToDestroy, Powerup powerup, float bounceFactor, BrickType brickType, bool doubleSizeBrick = false)
        {
            if (brickType != BrickType.NotSet)
            {
                BrickType = brickType;
            }
            else
            {
                BrickType = hitsToDestroy == -1 ? BrickType.Indistructible : BrickType.Normal;
            }

            IsIndestructible = BrickType == BrickType.Indistructible || BrickType == BrickType.Bouncey;

            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
            _sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            if (doubleSizeBrick)
            {
                _sprite.ContentSize = new CCSize(_sprite.ContentSize.Width + GameConstants.BrickGap, _sprite.ContentSize.Height + GameConstants.BrickGap);
            }
            AddChild(_sprite);

            ContentSize = _sprite.ContentSize;
            AnchorPoint = CCPoint.AnchorLowerLeft;
            PositionX = position.X;
            PositionY = position.Y;
            ScaleX = scale;
            ScaleY = scale;

            HitsToDestroy = hitsToDestroy;
            Powerup = powerup;
            BounceFactor = bounceFactor;
            HitsTaken = 0;
        }

        /// <summary>
        /// Fired when the brick is hit
        /// </summary>
        /// <returns>Returns whether or not the brick has been destroyed</returns>
        public bool Hit()
        {
            if (IsIndestructible)
            {
                return false;
            }

            HitsTaken += 1;
            
            if (HitsTaken >= HitsToDestroy)
            {
                BrickFactory.Instance.DestroyBrick(this);
                return true;
            }

            //Fade the brick slightly if hit but not destroyed
            double opacity = 1 - HitsTaken / (double)HitsToDestroy;
            _sprite.Opacity = (byte)(255 * opacity);

            return false;
        }
        
    }
}
