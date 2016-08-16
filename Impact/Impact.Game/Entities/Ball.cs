﻿using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a ball in the game
    /// </summary>
    public sealed class Ball : CCNode
    {
        public bool ApplyGravity { get; set; }
        private bool _isFireball;
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        private CCParticleSun _fireball;

        /// <summary>
        /// If set, a fireball effect is added
        /// </summary>
        public bool IsFireball
        {
            get { return _isFireball; }
            set
            {
                _isFireball = value;
                if (value)
                {
                    _fireball =
                        new CCParticleSun(new CCPoint(0, 0), CCEmitterMode.Radius)
                        {
                            StartRadius = 0,
                            EndRadius = 12,
                            AnchorPoint = CCPoint.AnchorLowerLeft
                        };
                    AddChild(_fireball);
                }
                else
                {
                    if (_fireball != null)
                    {
                        _fireball.RemoveFromParent();
                        _fireball.Dispose();
                    }
                }
                
            }
        }

        public Ball(float? velocityY = GameConstants.BallInitialVelocityY, CCPoint? initialPosition = null, bool applyGravity = false)
        {
            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImageBall);
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };

            AddChild(sprite);

            ContentSize = sprite.ContentSize;
            
            PositionX = initialPosition.HasValue ? initialPosition.Value.X : GameConstants.BallInitialPosition.X;
            PositionY = initialPosition.HasValue ? initialPosition.Value.Y : GameConstants.BallInitialPosition.Y;
            VelocityY = velocityY ?? GameConstants.BallInitialVelocityY;
            ApplyGravity = applyGravity;

            //If the level is already running, activate the ball, otherwise wait for an event
            if (GameStateManager.Instance.LevelHasStarted)
            {
                Schedule(ApplyVelocity);
            }

            GameStateManager.Instance.LevelStarted += GameManager_LevelStarted;

            if (GameStateManager.Instance.DebugMode)
            {
                var drawNode = new CCDrawNode();
                AddChild(drawNode);
                drawNode.DrawRect(new CCRect(0, 0, ContentSize.Width, ContentSize.Height), CCColor4B.Transparent, 1, CCColor4B.Yellow);
            }

        }

        public void Reset()
        {
            VelocityY = GameConstants.BallInitialVelocityY;
        }

        /// <summary>
        /// Start and stop the ball movement if the game is running or not
        /// </summary>
        private void GameManager_LevelStarted(bool started)
        {
            if (started)
            {
                Schedule(ApplyVelocity);
            }
            else
            {
                Unschedule(ApplyVelocity);
            }
        }

        private void ApplyVelocity(float frameTimeInSeconds)
        {
            if (ApplyGravity)
            {
                // This is a linear approximation, so not 100% accurate
                VelocityY += frameTimeInSeconds * -GameConstants.BallGravityCoefficient;
            }

            PositionX += VelocityX * frameTimeInSeconds;
            PositionY += VelocityY * frameTimeInSeconds;
        }

    }
}
