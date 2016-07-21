using CocosSharp;

namespace Impact.Game.Config
{
    public static class GameConstants
    {
        //Ball
        public const float BallMaxVelocityX = 500;
        public const float BallInitialVelocityY = 600;
        public static CCPoint BallInitialPosition => new CCPoint(320, 112);
        public const int BallZOrder = 20;
        public const float BallGravityCoefficient = 750;
        
        //Paddle
        public static CCPoint PaddleInitialPosition => new CCPoint(320, 100);
        public const float PaddleScaleX = 1;
        public const float PaddleGravityBounceVelocityY = 1200;

        //Powerups
        public const float PowerupVelocityY = -300;
        public const float PowerupGravity = 210;
        public const float PowerupLargerPaddleSeconds = 10;
        public const float PowerupFireballSeconds = 10;
        public const int PowerupZOrder = 10;

        public const string SpriteImageBall = "Ball.png";
        public const string SpriteImagePaddle = "Paddle.png";

        public static CCColor4B BackgroundColour = new CCColor4B(0, 32, 40);
    }
}
