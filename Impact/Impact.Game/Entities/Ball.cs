using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{

    public sealed class Ball : CCNode
    {
        public bool ApplyGravity { get; set; }
        private bool _isFireball;
        public float VelocityX { get; set; }
        public float VelocityY { get; set; }

        private CCParticleSun _fireball;

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
                            EndRadius = 24,
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
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == GameConstants.SpriteImageBall);
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
            if (GameManager.Instance.LevelHasStarted)
            {
                Schedule(ApplyVelocity);
            }

            GameManager.Instance.LevelStarted += GameManager_LevelStarted;

            if (GameManager.Instance.DebugMode)
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
