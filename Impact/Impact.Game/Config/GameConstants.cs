using CocosSharp;

namespace Impact.Game.Config
{
    public static class GameConstants
    {

        //Game
        public const int WorldWidth = 833;
        public const int WorldHeight = 1481;
        public const int BrickGap = 5;

        //Ball
        public const float BallMaxVelocityX = 500;
        public const float BallInitialVelocityY = 750;
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

        public const string TitleScreenSpriteSheet = "Spritesheets/TitleScreen.plist";
        public const string TitleScreenSpriteSheetImage = "Spritesheets/TitleScreen.png";

        public const string GameEntitiesSpriteSheet = "Spritesheets/GameEntities.plist";
        public const string GameEntitiesSpriteSheetImage = "Spritesheets/GameEntities.png";


    }
}
