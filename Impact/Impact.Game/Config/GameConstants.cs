using System.Collections.Generic;
using CocosSharp;

namespace Impact.Game.Config
{
    public static class GameConstants
    {
        //Resources
        public const string SpriteImageBall = "Ball.png";
        public const string SpriteImagePaddle = "Paddle.png";
        public const string SpriteImagePaddleGun = "PaddleBullet.png";
        public const string SpriteImagePaddleRocketLauncher = "PaddleRocket.png";
        public const string SpriteImagePaddleGrenadeLauncher = "PaddleGrenade.png";
        public const string SpriteImageBullet = "Bullet.png";
        public const string SpriteImageRocket = "Rocket.png";
        public const string SpriteImageGrenade = "Grenade.png";
        public const string TitleScreenSpriteSheet = "Spritesheets/TitleScreen.plist";
        public const string TitleScreenSpriteSheetImage = "Spritesheets/TitleScreen.png";
        public const string GameEntitiesSpriteSheet = "Spritesheets/GameEntities.plist";
        public const string GameEntitiesSpriteSheetImage = "Spritesheets/GameEntities.png";
        public const string LevelSelectScreenSpriteSheet = "Spritesheets/LevelSelectScreen.plist";
        public const string LevelSelectScreenSpriteSheetImage = "Spritesheets/LevelSelectScreen.png";

        public static Queue<string> BrickHitSounds = new Queue<string>(new List<string>
        {
            "massive-timpanaphone/D5.wav",
            "massive-timpanaphone/G5.wav"
        });
        public const string PaddleHitSound = "massive-banzai23/D3.wav";
        public const string BrickHitButNotDestroyedSound = "massive-timpanaphone/D5-Filtered.wav";
        public const string ScoreUpSound = "ScoreUp.wav";
        public const string BulletSound = "Bullet.wav";
        public const string GrenadeSound = "bomb.wav";
        public const string SwitchHitSound = "switch.wav";

        //Colours
        public static CCColor4B BackgroundColour = new CCColor4B(0, 32, 40); //new CCColor4B(45, 61, 0); //new CCColor4B(49, 0, 34); 
        public static CCColor3B ImpactGreen = new CCColor3B(178, 242, 0);
        public static CCColor3B ImpactYellow = new CCColor3B(255, 244, 0);

        ////Toying with the idea of changing background colours per level
        //public static CCColor4B Colour1 = new CCColor4B(21, 24, 24);
        //public static CCColor4B Colour2 = new CCColor4B(30, 26, 29);
        //public static CCColor4B Colour3 = new CCColor4B(36, 37, 33);
        //public static CCColor4B Colour4 = new CCColor4B(39, 37, 34);
        //public static Queue<CCColor4B> BackgroundColours = new Queue<CCColor4B>(new List<CCColor4B> { Colour1, Colour2, Colour3, Colour4 });

        //Game
        public const int WorldWidth = 833;
        public const int WorldHeight = 1481;
        public const int WorldTop = 1312;
        public const float BrickGap = 2.5f;
        public const int BricksMinY = 380;

        //Ball
        public const float BallMaxVelocityX = 500;
        public const float BallInitialVelocityY = 750;
        public static CCPoint BallInitialPosition => new CCPoint((WorldWidth/2f), 224);
        public const int BallZOrder = 20;
        public const float BallGravityCoefficient = 1200;
        public const float GravityLevelBallSpeedBrickDampeningCoefficient = 0.80f; // 1= no dampening
        
        //Paddle
        public static CCPoint PaddleInitialPosition => new CCPoint(WorldWidth / 2f, 200);
        public const float PaddleScaleX = 1.2f;
        public const float PaddleScaleY = 1;
        public const float PaddleGravityBounceVelocityY = 1000;
        public const float BulletVelocity = 750;
        public const float RocketVelocity = 400;
        public const float GrenadeVelocity = 350;

        //Powerups
        public const float PowerupVelocityY = -300;
        public const float PowerupGravity = 210;
        public const float PowerupLargerPaddleSeconds = 10;
        public const float PowerupFireballSeconds = 4;
        public const float PowerupBulletsSeconds = 7;
        public const float PowerupRocketsSeconds = 7;
        public const int PowerupZOrder = 10;

        //ScoreUps
        public const float ScoreUpGravity = 100;

        //LifeUps
        public const float LifeUpGravity = 270;

        //Settings
        public const int MusicVolumeDefault = 3;
        public const int SfxVolumeDefault = 10;
        public const int MusicVolumeMax = 10;
        public const int SfxVolumeMax = 10;

        //Score related
        public const float MaxLevelTimeForBonus = 120;
        public const float MaxTimeBonus = 200;
        public const int ScoreBonusForDestroyingBrick = 10;
        public const int ScoreBonusForCollectingPowerUp = 20;
        public const int ScoreBonusPerRemainingLife = 50;

    }
}
