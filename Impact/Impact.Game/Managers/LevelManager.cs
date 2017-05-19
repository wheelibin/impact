using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;
using Impact.Game.Extensions;
using Impact.Game.Factories;
using Impact.Game.Helpers;
using Impact.Game.Models;
using Microsoft.Xna.Framework;
using TiledSharp;

namespace Impact.Game.Managers
{
    /// <summary>
    /// Contains level related functions such as level loading
    /// </summary>
    public class LevelManager
    {
        //Singleton
        private static readonly Lazy<LevelManager> SelfInstance = new Lazy<LevelManager>(() => new LevelManager());
        public static LevelManager Instance => SelfInstance.Value;

        //Properties specified in the .tmx file
        private const string FinalBallSpeedPercentageIncreasePropertyName = "FinalBallSpeedPercentageIncrease";
        private const string HitsToDestroyPropertyName = "HitsToDestroy";
        private const string PowerupTypePropertyName = "PowerupType";
        private const string GravityPropertyName = "Gravity";
        private const string BrickTypePropertyName = "BrickType";
        private const string BounceFactorPropertyName = "BounceFactor";
        private const string WormholeTypePropertyName = "WormholeType";
        private const string WormholeExitNamePropertyName = "ExitName";
        private const string WormholeExitDirectionPropertyName = "ExitDirection";

        private const string BrickLayer = "Bricks";
        private const string BrickTileset = "Bricks";
        private const string PowerupLayer = "Powerups";
        private const string PowerupTileset = "Powerups";
        private const string WormholeObjectGroup = "Wormholes";
        private const string WormholeTileset = "Wormholes";
        private const string PaddlesLayer = "Paddles";
        private const string PaddlesObjectGroup = "Paddles";

        private const string BaseLevelFolder = "Content/Levels/";
        private const string LevelFilenameFormatString = "{0}/level{1}.tmx";

        public int NumberOfLevels { get; private set; }
        public int CurrentLevel { get; set; }
        public LevelProperties CurrentLevelProperties { get; private set; }


        public LevelManager()
        {
            DetermineAvailableLevels();
            CurrentLevel = GetHighestPlayableLevel();
        }

        public int GetNextLevel(int currentLevel)
        {
            return currentLevel + 1;
        }

        public int GetHighestPlayableLevel()
        {
            //Choose the next level to play, if the game is complete then just replay the last level
            if (Settings.HighestCompletedLevel + 1 > NumberOfLevels)
            {
                return NumberOfLevels;
            }
            return Settings.HighestCompletedLevel + 1;
        }

        public List<Brick> LoadLevel(int level, Paddle paddle, List<Ball> balls, ScoreManager scoreManager)
        {

            var bricks = new List<Brick>();

            //Load the level
            string levelFilename = string.Format(LevelFilenameFormatString, BaseLevelFolder, level.ToString("000"));
            var tileMap = new TmxMap(levelFilename);

            //Set the level properties

            bool gravity = tileMap.Properties.GetPropertyValue(GravityPropertyName, bool.Parse);
            int finalBallSpeedPercentageIncrease = tileMap.Properties.GetPropertyValue(FinalBallSpeedPercentageIncreasePropertyName, int.Parse);

            //Get high score
            int highScore = scoreManager.GetHighScoreForLevel(level);

            CurrentLevelProperties = new LevelProperties
            {
                HighScore = highScore,
                FinalBallSpeedPercentageIncrease = finalBallSpeedPercentageIncrease,
                Gravity = gravity,
                Lives = 2
            };

            //Layout config
            float tileWidth = tileMap.TileWidth + GameConstants.BrickGap;
            float tileHeight = tileMap.TileHeight + GameConstants.BrickGap;
            float tileMapHeight = tileMap.Height * tileHeight;
            float startY = tileMapHeight + GameConstants.BricksMinY;

            //Get the layers and tilesets
            TmxLayer brickLayer = tileMap.Layers[BrickLayer];
            TmxLayer powerupLayer = tileMap.Layers[PowerupLayer];
            TmxTileset brickTileset = tileMap.Tilesets[BrickTileset];
            TmxTileset powerupTileset = tileMap.Tilesets[PowerupTileset];

            //Loop through tiles
            for (int t = 0; t < brickLayer.Tiles.Count; t++)
            {

                TmxLayerTile brickTile = brickLayer.Tiles[t];
                TmxLayerTile powerupTile = powerupLayer.Tiles[t];

                if (brickTile.Gid > 0)
                {
                    //Get the image filename from the tile image (regardless of the path it's set to), will be used to pick a texture out of a spritesheet
                    TmxTilesetTile brickTilesetTile = brickTileset.Tiles.Single(tile => tile.Id == brickTile.Gid - brickTileset.FirstGid);
                    string brickImageFilename = Path.GetFileName(brickTilesetTile.Image.Source);

                    //Get brick properties
                    int hitsToDestroy = brickTilesetTile.Properties.GetPropertyValue(HitsToDestroyPropertyName, int.Parse);
                    BrickType brickType = brickTilesetTile.Properties.GetPropertyValue(BrickTypePropertyName, s => (BrickType)Enum.Parse(typeof(BrickType), s));

                    CCPoint brickPosition = new CCPoint(brickTile.X * tileWidth, startY - (brickTile.Y * tileHeight));

                    //Create a powerup to add to the brick if one is defined
                    Powerup powerup = null;
                    if (powerupTile.Gid > 0)
                    {
                        //Get the powerup image filename from the tile image 
                        TmxTilesetTile powerupTilesetTile = powerupTileset.Tiles[powerupTile.Gid - powerupTileset.FirstGid];
                        string powerupImageFilename = Path.GetFileName(powerupTilesetTile.Image.Source);

                        //Determine which powerup to create and initialise the appropriate powerup entity
                        PowerupType powerupType = powerupTilesetTile.Properties.GetPropertyValue(PowerupTypePropertyName, s => (PowerupType)Enum.Parse(typeof(PowerupType), s));
                        switch (powerupType)
                        {
                            case PowerupType.LargerPaddle:
                                powerup = new LargerPaddlePowerup(powerupImageFilename, brickPosition, paddle);
                                break;
                            case PowerupType.Multiball:
                                powerup = new MultiBallPowerup(powerupImageFilename, brickPosition, balls);
                                break;
                            case PowerupType.FireBall:
                                powerup = new FireballPowerup(powerupImageFilename, brickPosition, balls);
                                break;
                            case PowerupType.Bullets:
                                powerup = new GunPowerup(powerupImageFilename, brickPosition, paddle);
                                break;
                            case PowerupType.Rockets:
                                powerup = new RocketLauncherPowerup(powerupImageFilename, brickPosition, paddle);
                                break;
                            case PowerupType.ExtraLife:
                                powerup = new ExtraLifePowerup(powerupImageFilename, brickPosition);
                                break;
                        }
                    }

                    float bounceFactor = brickTilesetTile.Properties.GetPropertyValue(BounceFactorPropertyName, float.Parse);
                    bool doubleSizeBrick = brickTilesetTile.Image.Width == (tileMap.TileWidth * 2);

                    //Add the brick
                    var brick = BrickFactory.Instance.CreateNew(brickImageFilename, brickPosition, 1, hitsToDestroy, powerup, bounceFactor, brickType, doubleSizeBrick);
                    bricks.Add(brick);

                }
            }

            //Wormholes
            if (tileMap.ObjectGroups.Contains(WormholeObjectGroup))
            {
                TmxTileset wormholeTileset = tileMap.Tilesets[WormholeTileset];
                TmxObjectGroup wormholes = tileMap.ObjectGroups[WormholeObjectGroup];

                foreach (TmxObject wormhole in wormholes.Objects)
                {
                    TmxTilesetTile wormholeTilesetTile = wormholeTileset.Tiles.Single(tile => tile.Id == wormhole.Tile.Gid - wormholeTileset.FirstGid);
                    string wormholeImageFilename = Path.GetFileName(wormholeTilesetTile.Image.Source);

                    //Because we draw the bricks manually, with a gap, the coordinates for objects on the map need adjusting to our level coordinates.
                    //The coordinates will be out by the number of brick gaps between the point and 0,0
                    float xAdjustment = (int)(wormhole.X / tileMap.TileWidth) * GameConstants.BrickGap;
                    float yAdjustment = (int)(wormhole.Y / tileMap.TileHeight) * GameConstants.BrickGap;

                    CCPoint position = new CCPoint((float)wormhole.X + xAdjustment, (startY + tileHeight) - ((float)wormhole.Y + yAdjustment));
                    WormholeType wormholeType = wormhole.Properties.GetPropertyValue(WormholeTypePropertyName, s => (WormholeType)Enum.Parse(typeof(WormholeType), s));
                    string exitName = wormhole.Properties.GetPropertyValue(WormholeExitNamePropertyName, s => s.ToString());
                    WormholeExitDirection exitDirection = wormhole.Properties.GetPropertyValue(WormholeExitDirectionPropertyName, s => (WormholeExitDirection)Enum.Parse(typeof(WormholeExitDirection), s));

                    WormholeFactory.Instance.CreateNew(wormholeImageFilename, position, wormholeType, wormhole.Name, exitName, exitDirection);
                }
            }

            if (tileMap.ObjectGroups.Contains(PaddlesObjectGroup))
            {
                TmxObjectGroup paddles = tileMap.ObjectGroups[PaddlesObjectGroup];

                foreach (TmxObject extraPaddle in paddles.Objects)
                {
                    //Because we draw the bricks manually, with a gap, the coordinates for objects on the map need adjusting to our level coordinates.
                    //The coordinates will be out by the number of brick gaps between the point and 0,0
                    float xAdjustment = (int)(extraPaddle.X / tileMap.TileWidth) * GameConstants.BrickGap;
                    float yAdjustment = (int)(extraPaddle.Y / tileMap.TileHeight) * GameConstants.BrickGap;

                    CCPoint position = new CCPoint((float)extraPaddle.X + xAdjustment, (startY + tileHeight) - ((float)extraPaddle.Y + yAdjustment));
                    PaddleFactory.Instance.CreateNew(position);
                }
            }

            return bricks;

        }

        private void DetermineAvailableLevels()
        {
            // This game relies on levels being named "levelx.tmx" where x is an integer beginning with
            // 1. We have to rely on MonoGame's TitleContainer which doesn't give us a GetFiles method - we simply
            // have to check if a file exists, and if we get an exception on the call then we know the file doesn't
            // exist. 
            NumberOfLevels = 0;
            while (true)
            {
                bool fileExists = false;
                try
                {
                    string filename = string.Format(LevelFilenameFormatString, BaseLevelFolder, (NumberOfLevels + 1).ToString("000"));
                    using (TitleContainer.OpenStream(filename))
                    {
                    }
                    // if we got here then the file exists!
                    fileExists = true;
                }
                catch
                {
                    // do nothing, fileExists will remain false
                }
                if (!fileExists)
                {
                    break;
                }
                NumberOfLevels++;
            }
        }

    }
}
