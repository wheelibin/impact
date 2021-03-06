﻿using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a ScoreUp, a collectible item that falls from the top of the screen
    /// </summary>
    public class ScoreUp : CCNode
    {
        public int Score { get; set; }
        public float VelocityY { get; set; }

        public ScoreUp(CCPoint initialPosition, int score)
        {
            Score = score;
            CCSpriteFrame frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == "ScoreUp.png");
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorMiddle
            };
            
            ContentSize = sprite.ContentSize;
            PositionX = initialPosition.X;
            PositionY = initialPosition.Y;

            AddChild(sprite);

            CCLabel label = new CCLabel(score.ToString(), "visitor1.ttf", 28, CCLabelFormat.SystemFont)
            {
                HorizontalAlignment = CCTextAlignment.Center,
                PositionY = sprite.PositionY,
                Color = CCColor3B.Black,
                ContentSize = sprite.ContentSize
            };

            AddChild(label);

            Schedule(ApplyVelocity);
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            VelocityY += frameTimeInSeconds * -GameConstants.ScoreUpGravity;
            PositionY += VelocityY * frameTimeInSeconds;
        }
        
    }
}
