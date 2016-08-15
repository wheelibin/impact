using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;
using Impact.Game.Extensions;
using Impact.Game.Factories;

namespace Impact.Game.Managers
{
    /// <summary>
    /// Contains the collision logic for the game, fires various events when collisions happen
    /// </summary>
    public class CollisionManager
    {
        public event Action PaddleHit;
        public event Action<int> BricksHit;
        public event Action BrickHitButNotDestroyed;
        public event Action<Powerup> PowerupCollected;
        public event Action<ScoreUp> ScoreUpCollected;
        public event Action<Ball> MissedBall;

        public void HandleCollisions(CCLayer layer, Paddle paddle, List<Ball> balls, List<Brick> bricks, List<Powerup> powerups, List<Wormhole> wormholes, List<ScoreUp> scoreUps, List<Projectile> projectiles)
        {

            CCRect paddleBoundingBox = paddle.BoundingBoxTransformedToWorld;

            //Projectile collisions
            for (int b = projectiles.Count - 1; b >= 0; b--)
            {
                Projectile projectile = projectiles[b];
                List<Brick> bricksHitByProjectile = projectile.BricksHitByEntity(bricks);

                foreach (Brick brick in bricksHitByProjectile)
                {
                    bool brickDestroyed = brick.Hit();
                    if (!brickDestroyed)
                    {
                        BrickHitButNotDestroyed?.Invoke();
                    }
                }

                if (bricksHitByProjectile.Any() && projectile.IsDestroyedByBrickCollision)
                {
                    ProjectileFactory.Instance.DestroyBullet(projectile);
                }

            }

            //Ball collisions
            for (int ballIndex = balls.Count - 1; ballIndex >= 0; ballIndex--)
            {
                Ball ball = balls[ballIndex];

                CCRect ballBoundingBox = ball.BoundingBoxTransformedToWorld;
                float ballRight = ballBoundingBox.MaxX;
                float ballLeft = ballBoundingBox.MinX;
                float ballTop = ballBoundingBox.MaxY;
                float screenRight = layer.VisibleBoundsWorldspace.MaxX;
                float screenLeft = layer.VisibleBoundsWorldspace.MinX;
                float screenTop = GameConstants.WorldTop;
                float screenBottom = layer.VisibleBoundsWorldspace.MinY;
                bool isMovingDownward = ball.VelocityY < 0;

                //Bounce off paddle
                bool ballHitPaddle = ballBoundingBox.IntersectsRect(paddleBoundingBox);

                if (ballHitPaddle && isMovingDownward)
                {
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

                //Bounce off the walls
                bool shouldReflectXVelocity =
                    (ballRight > screenRight && ball.VelocityX > 0) ||
                    (ballLeft < screenLeft && ball.VelocityX < 0);

                if (shouldReflectXVelocity)
                {
                    ball.VelocityX = ball.VelocityX.ApplyVariation();
                    ball.VelocityX *= -1;
                    return;
                }

                bool shouldReflectYVelocity = (ballTop > screenTop && ball.VelocityY > 0);
                if (GameStateManager.Instance.CheatModeEnabled)
                {
                    shouldReflectYVelocity = (ballTop > screenTop && ball.VelocityY > 0) || (ballTop < screenBottom && ball.VelocityY < 0);
                }
                else
                {
                    if (ballTop < screenBottom && ball.VelocityY < 0)
                    {
                        //missed the ball lose a life
                        MissedBall?.Invoke(ball);
                        return;
                    }
                }

                if (shouldReflectYVelocity)
                {
                    ball.VelocityY *= -1;
                    return;
                }

                //Have we hit any bricks
                //If the ball also intersects other bricks, group them into a single brick
                List<Brick> bricksHitByBall = ball.BricksHitByEntity(bricks);

                bool hitAnyBricksWithBall = bricksHitByBall.Count > 0;

                //Bounce logic (using all the hit bricks as a group)
                if (hitAnyBricksWithBall)
                {
                    CCRect groupedBrick = bricksHitByBall.GetGroupedBrickBounds();

                    CCVector2 separatingVector = GetSeparatingVector(ballBoundingBox, groupedBrick, layer);

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
                }

                //Individual brick hit functions
                foreach (Brick brick in bricksHitByBall)
                {

                    if (brick.BrickType == BrickType.Bouncey)
                    {
                        ball.VelocityY = GameConstants.PaddleGravityBounceVelocityY * brick.BounceFactor;
                    }

                    bool brickDestroyed = brick.Hit();

                    if (brickDestroyed)
                    {
                        if (!ball.ApplyGravity)
                        {
                            //slightly speed the ball up if brick destroyed
                            int remainingBricks = bricks.Count(b => !b.IsIndestructible);
                            if (remainingBricks > 0)
                            {
                                float ballSpeedPercentageIncreaseFactor = (LevelManager.Instance.CurrentLevelProperties.FinalBallSpeedPercentageIncrease / (float)remainingBricks) / 100;
                                float newBallSpeed = Math.Abs(GameConstants.BallInitialVelocityY + (GameConstants.BallInitialVelocityY * ballSpeedPercentageIncreaseFactor));

                                if (ball.VelocityY < 0)
                                {
                                    ball.VelocityY = -newBallSpeed;
                                }
                                else
                                {
                                    ball.VelocityY = +newBallSpeed;
                                }
                            }
                        }
                        else
                        {
                            ball.VelocityY *= GameConstants.GravityLevelBallSpeedBrickDampeningCoefficient;
                        }

                        //Drop powerup if there is one
                        brick.Powerup?.Drop();

                    }
                    else
                    {
                        BrickHitButNotDestroyed?.Invoke();
                    }
                }

                if (bricksHitByBall.Any())
                {
                    BricksHit?.Invoke(bricksHitByBall.Count);
                }

                //Wormholes
                foreach (Wormhole wormhole in wormholes.Where(w => w.WormholeType == WormholeType.In || w.WormholeType == WormholeType.InOut))
                {

                    CCRect wormholeBoundingBox = wormhole.BoundingBoxTransformedToWorld;
                    bool enteredWormhole = ballBoundingBox.IntersectsRect(wormholeBoundingBox);

                    if (enteredWormhole)
                    {
                        //Get the exit wormhole
                        Wormhole exit = wormholes.First(w => w.ObjectName == wormhole.ExitName);

                        ball.Position = exit.BoundingBoxTransformedToWorld.Center;

                        //todo: in order to properly handle InOut wormholes, we would need to handle the fact that once a ball has 
                        //todo: been moved to the target one, it will still be classed as being inside one, which will transport it again...in and endless loop
                        //if (!isMovingDownward)
                        //{
                        //    ball.PositionY -= exit.ContentSize.Height/2; 
                        //}
                        //else
                        //{
                        //    ball.PositionY += exit.ContentSize.Height/2;
                        //}

                        //if (isMovingLeft)
                        //{
                        //    ball.PositionX -= exit.ContentSize.Width/2;
                        //}
                        //else
                        //{
                        //    ball.PositionX += exit.ContentSize.Width/2;
                        //}

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
                    PowerupCollected?.Invoke(powerup);
                }
            }

            //Have we caught a scoreUp?
            for (int s = scoreUps.Count - 1; s >= 0; s--)
            {
                ScoreUp scoreUp = scoreUps[s];
                bool scoreUpHitPaddle = scoreUp.BoundingBoxTransformedToWorld.IntersectsRect(paddleBoundingBox);
                if (scoreUpHitPaddle)
                {
                    ScoreUpCollected?.Invoke(scoreUp);
                }
            }

        }
        
        /// <summary>
        /// Returns the vector that the 'ball' should be moved by to separate the objects. 
        /// </summary>
        public CCVector2 GetSeparatingVector(CCRect ball, CCRect brick, CCLayer layer)
        {
            // Default to no separation
            CCVector2 separation = CCVector2.Zero;

            // Only calculate separation if the rectangles intersect
            if (ball.IntersectsRect(brick))
            {
                // The intersectionRect returns the rectangle produced
                // by overlapping the two rectangles
                var intersectionRect = ball.Intersection(brick);

                //Workaround CCRECT bug
                if (Math.Abs(intersectionRect.Size.Height - brick.MinY) < 1)
                {
                    intersectionRect.Size.Height = 1f;
                }
                if (Math.Abs(intersectionRect.Size.Width - brick.MinX) < 1)
                {
                    intersectionRect.Size.Width = 1f;
                }
                if (Math.Abs(intersectionRect.Size.Width) < 1)
                {
                    intersectionRect.Size.Width = 1;
                }
                if (Math.Abs(intersectionRect.Size.Height) < 1)
                {
                    intersectionRect.Size.Height = 1;
                }

                if (GameStateManager.Instance.DebugMode)
                {
                    var drawNode = new CCDrawNode();
                    layer.AddChild(drawNode);
                    drawNode.DrawRect(intersectionRect, CCColor4B.Transparent, 1, CCColor4B.Red);
                }

                // Separation should occur by moving the minimum distance
                // possible. We do this by checking which is smaller: width or height?
                bool separateHorizontally = intersectionRect.Size.Width < intersectionRect.Size.Height;

                if (separateHorizontally)
                {
                    separation.X = intersectionRect.Size.Width;

                    // Since separation is from the perspective
                    // of 'ball', the value should be negative if
                    // the ball is to the left of the brick.
                    if (ball.Center.X < brick.Center.X)
                    {
                        separation.X *= -1;
                    }
                    separation.Y = 0;
                }
                else
                {
                    separation.X = 0;

                    separation.Y = intersectionRect.Size.Height;

                    if (ball.Center.Y < brick.Center.Y)
                    {
                        separation.Y *= -1;
                    }
                }
            }

            return separation;
        }


    }
}
