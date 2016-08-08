using System.Collections.Generic;
using CocosSharp;

namespace Impact.Game.Config
{
    public static class GameConstants
    {
        //Resources
        public const string SpriteImageBall = "Ball.png";
        public const string SpriteImagePaddle = "Paddle.png";
        public const string SpriteImagePaddleBullet = "PaddleBullet.png";
        public const string SpriteImageBullet = "Bullet.png";
        public const string TitleScreenSpriteSheet = "Spritesheets/TitleScreen.plist";
        public const string TitleScreenSpriteSheetImage = "Spritesheets/TitleScreen.png";
        public const string GameEntitiesSpriteSheet = "Spritesheets/GameEntities.plist";
        public const string GameEntitiesSpriteSheetImage = "Spritesheets/GameEntities.png";
        public const string LevelSelectScreenSpriteSheet = "Spritesheets/LevelSelectScreen.plist";
        public const string LevelSelectScreenSpriteSheetImage = "Spritesheets/LevelSelectScreen.png";

        public static Queue<string> BrickHitSounds = new Queue<string>(new List<string>
        {
            "massive-banzai23/E3.wav",
            "massive-banzai23/A3.wav",
            "massive-banzai23/F3.wav"
        });
        public const string PaddleHitSound = "massive-banzai23/D3.wav";
        public const string BrickHitButNotDestroyedSound = "massive-timpanaphone/D5-Filtered.wav";

        //Colours
        public static CCColor4B BackgroundColour = new CCColor4B(0, 32, 40);

        //Game
        public const int WorldWidth = 833;
        public const int WorldHeight = 1481;
        public const float BrickGap = 2.5f;

        //Ball
        public const float BallMaxVelocityX = 500;
        public const float BallInitialVelocityY = 750;
        public static CCPoint BallInitialPosition => new CCPoint(WorldWidth/2f, 300);
        public const int BallZOrder = 20;
        public const float BallGravityCoefficient = 1200;
        public const float GravityLevelBallSpeedBrickDampeningCoefficient = 0.80f; // 1= no dampening
        
        //Paddle
        public static CCPoint PaddleInitialPosition => new CCPoint(320, 200);
        public const float PaddleScaleX = 1;
        public const float PaddleGravityBounceVelocityY = 1000;
        public const float BulletVelocity = 750;
        
        //Powerups
        public const float PowerupVelocityY = -300;
        public const float PowerupGravity = 210;
        public const float PowerupLargerPaddleSeconds = 7;
        public const float PowerupFireballSeconds = 3;
        public const float PowerupBulletsSeconds = 5;
        public const int PowerupZOrder = 10;

        //ScoreUps
        public const float ScoreUpGravity = 100;

        //LifeUps
        public const float LifeUpGravity = 270;

    }
}
