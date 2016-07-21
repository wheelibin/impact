using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;

namespace Impact.Game
{
    public class CollisionManager
    {
        //Singleton
        private static readonly Lazy<CollisionManager> SelfInstance = new Lazy<CollisionManager>(() => new CollisionManager());
        public static CollisionManager Instance => SelfInstance.Value;

        public event Action PaddleHit;
        public event Action BrickHitButNotDestroyed;
        
        public void HandleCollisions(CCLayer layer, Paddle paddle, List<Ball> balls, List<Brick> bricks, List<Powerup> powerups, List<Powerup> activatedPowerups)
        {

            CCRect paddleBoundingBox = paddle.BoundingBoxTransformedToWorld;

            for (int ballIndex = balls.Count - 1; ballIndex >= 0; ballIndex--)
            {
                Ball ball = balls[ballIndex];

                CCRect ballBoundingBox = ball.BoundingBoxTransformedToWorld;

                //Bounce off paddle
                
                bool ballHitPaddle = ballBoundingBox.IntersectsRect(paddleBoundingBox);
                bool isMovingDownward = ball.VelocityY < 0;

                if (ballHitPaddle && isMovingDownward)
                {
                    // Y velocity
                    if (ball.ApplyGravity)
                    {
                        //if gravity is applied then bounce off the paddle at a constant velocity (or the ball will eventually come to rest)
                        ball.VelocityY = GameConstants.PaddleGravityBounceVelocityY;
                    }
                    else
                    {
                        ball.VelocityY *= -1;
                    }

                    // Bounce off the paddle based on how far from the centre the ball hit
                    float distanceFromCentre = (ballBoundingBox.Center.X -
                                                paddleBoundingBox.Center.X) /
                                               (paddle.BoundingBox.Size.Width / 2);

                    ball.VelocityX = GameConstants.BallMaxVelocityX * distanceFromCentre;

                    PaddleHit?.Invoke();

                    return;
                }

                // First let’s get the ball position:   
                float ballRight = ballBoundingBox.MaxX;
                float ballLeft = ballBoundingBox.MinX;
                float ballTop = ballBoundingBox.MaxY;

                // Then let’s get the screen edges
                float screenRight = layer.VisibleBoundsWorldspace.MaxX;
                float screenLeft = layer.VisibleBoundsWorldspace.MinX;
                float screenTop = layer.VisibleBoundsWorldspace.MaxY;
                float screenBottom = layer.VisibleBoundsWorldspace.MinY;

                // Check if the ball is either too far to the right or left:    
                bool shouldReflectXVelocity =
                    (ballRight > screenRight && ball.VelocityX > 0) ||
                    (ballLeft < screenLeft && ball.VelocityX < 0);

                if (shouldReflectXVelocity)
                {
                    ball.VelocityX *= -1;
                    return;
                }

                // Check if the ball is either too far to the top (or the player missed it)
                bool shouldReflectYVelocity = (ballTop > screenTop && ball.VelocityY > 0);

                if (GameManager.Instance.CheatModeEnabled)
                {
                    shouldReflectYVelocity = (ballTop > screenTop && ball.VelocityY > 0) || (ballTop < screenBottom && ball.VelocityY < 0);
                }
                else
                {
                    if (ballTop < screenBottom && ball.VelocityY < 0)
                    {
                        //missed the ball so remove it
                        ball.RemoveFromParent();
                        balls.RemoveAt(ballIndex);
                        return;
                    }
                }

                if (shouldReflectYVelocity)
                {
                    ball.VelocityY *= -1;
                    return;
                }


                //have we hit a brick?
                for (int i = bricks.Count - 1; i >= 0; i--)
                {

                    Brick brick = bricks[i];
                    CCRect brickBoundingBox = brick.BoundingBoxTransformedToWorld;

                    bool hitBrick = ballBoundingBox.IntersectsRect(brickBoundingBox);

                    if (hitBrick)
                    {
                        CCVector2 separatingVector = GetSeparatingVector(ballBoundingBox, brickBoundingBox);

                        if (!ball.IsFireball)
                        {
                            //Bounce off the brick
                            ball.PositionX += separatingVector.X;
                            ball.PositionY += separatingVector.Y;
                            if (separatingVector.X < 0 || separatingVector.X > 0)
                            {
                                ball.VelocityX *= -1;
                            }
                            if (separatingVector.Y < 0 || separatingVector.Y > 0)
                            {
                                ball.VelocityY *= -1;
                            }
                        }


                        int remainingBricks = bricks.Count(b => b.BrickType != BrickType.Indistructible);
                        float ballSpeedPercentageIncreaseFactor = (LevelManager.Instance.CurrentLevelProperties.FinalBallSpeedPercentageIncrease / remainingBricks) / 100;

                        float newBallSpeed = Math.Abs(GameConstants.BallInitialVelocityY + (GameConstants.BallInitialVelocityY * ballSpeedPercentageIncreaseFactor));

                        //slightly speed the ball up if brick destroyed
                        if (ball.VelocityY < 0)
                        {
                            ball.VelocityY = -newBallSpeed;
                        }
                        else
                        {
                            ball.VelocityY = +newBallSpeed;
                        }

                        bool brickDestroyed = brick.Hit();

                        if (brickDestroyed)
                        {
                            brick.Powerup?.Drop();
                        }
                        else
                        {
                            BrickHitButNotDestroyed?.Invoke();
                        }

                        return;

                    }

                }
            }

            //Have we caught a powerup?
            for (int p = powerups.Count - 1; p >= 0; p--)
            {
                Powerup powerup = powerups[p];
                bool powerupHitPaddle = powerup.BoundingBoxTransformedToWorld.IntersectsRect(paddleBoundingBox);
                if (powerupHitPaddle)
                {
                    powerup.Activate();

                    powerup.RemoveFromParent();
                    powerups.Remove(powerup);
                    activatedPowerups.Add(powerup);
                }
            }

        }



        // Returns the vector that the 'first' should be moved by
        // to separate the objects. 
        private CCVector2 GetSeparatingVector(CCRect first, CCRect second)
        {
            // Default to no separation
            CCVector2 separation = CCVector2.Zero;

            // Only calculate separation if the rectangles intersect
            if (first.IntersectsRect(second))
            {
                // The intersectionRect returns the rectangle produced
                // by overlapping the two rectangles
                var intersectionRect = first.Intersection(second);

                //Workaround CCRECT bug
                if (intersectionRect.Size.Height == second.MinY)
                {
                    intersectionRect.Size.Height = 0.1f;
                }

                // Separation should occur by moving the minimum distance
                // possible. We do this by checking which is smaller: width or height?
                bool separateHorizontally = intersectionRect.Size.Width < intersectionRect.Size.Height;

                if (separateHorizontally)
                {
                    separation.X = intersectionRect.Size.Width;
                    // Since separation is from the perspective
                    // of 'first', the value should be negative if
                    // the first is to the left of the second.
                    if (first.Center.X < second.Center.X)
                    {
                        separation.X *= -1;
                    }
                    separation.Y = 0;
                }
                else
                {
                    separation.X = 0;

                    separation.Y = intersectionRect.Size.Height;
                    if (first.Center.Y < second.Center.Y)
                    {
                        separation.Y *= -1;
                    }
                }
            }

            return separation;
        }

        
    }
}
