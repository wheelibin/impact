using System;
using System.Collections.Generic;
using System.IO;
using CocosSharp;
using Impact.Enums;
using Impact.Game.Config;
using Impact.Game.Entities;
using Impact.Game.Entities.Powerups;
using Impact.Game.Enums;
using Impact.Game.Factories;
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
        private const string FinalBallSpeedPercentageIncreaseProperty = "FinalBallSpeedPercentageIncrease";
        private const string HitsToDestroyProperty = "HitsToDestroy";
        private const string PowerupTypeProperty = "PowerupType";
        private const string BrickLayer = "Bricks";
        private const string BrickTileset = "Bricks";
        private const string PowerupLayer = "Powerups";
        private const string PowerupTileset = "Powerups";
        private const string WormholeObjectGroup = "Wormholes";
        private const string WormholeTileset = "Wormholes";

        private const string BaseLevelFolder = "Content/Levels/level";

        public int NumberOfLevels { get; private set; }
        public int CurrentLevel { get; set; }
        public LevelProperties CurrentLevelProperties { get; private set; }

        public LevelManager()
        {
            DetermineAvailableLevels();
            CurrentLevel = 1;
        }

        public List<Brick> LoadLevel(int level, Paddle paddle, List<Ball> balls)
        {
            var bricks = new List<Brick>();

            //Load the level
            var tileMap = new TmxMap(BaseLevelFolder + level.ToString("000") + ".tmx");

            //Set the level properties

            bool gravity = false;
            if (tileMap.Properties.ContainsKey("Gravity"))
            {
                gravity = bool.Parse(tileMap.Properties["Gravity"]);
            }

            CurrentLevelProperties = new LevelProperties
            {
                FinalBallSpeedPercentageIncrease = int.Parse(tileMap.Properties[FinalBallSpeedPercentageIncreaseProperty]),
                Gravity = gravity
            };

            //Layout config
            int tileWidth = 114 + GameConstants.BrickGap;
            int tileHeight = 38 + GameConstants.BrickGap;
            int tileMapHeight = tileMap.Height * tileHeight;
            int yOffset = 450;
            int startY = tileMapHeight + yOffset;

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
                    TmxTilesetTile brickTilesetTile = brickTileset.Tiles[brickTile.Gid - brickTileset.FirstGid];
                    string brickImageFilename = Path.GetFileName(brickTilesetTile.Image.Source);

                    //Get brick properties
                    int hitsToDestroy = int.Parse(brickTilesetTile.Properties[HitsToDestroyProperty]);

                    BrickType brickType;
                    if (brickTilesetTile.Properties.ContainsKey("BrickType"))
                    {
                        brickType = (BrickType)Enum.Parse(typeof(BrickType), brickTilesetTile.Properties["BrickType"]);
                    }
                    else
                    {
                        brickType = BrickType.NotSet;
                    }

                    CCPoint brickPosition = new CCPoint(brickTile.X * tileWidth, startY - (brickTile.Y * tileHeight));

                    //Create a powerup to add to the brick if one is defined
                    Powerup powerup = null;
                    if (powerupTile.Gid > 0)
                    {
                        //Get the powerup image filename from the tile image 
                        TmxTilesetTile powerupTilesetTile = powerupTileset.Tiles[powerupTile.Gid - powerupTileset.FirstGid];
                        string powerupImageFilename = Path.GetFileName(powerupTilesetTile.Image.Source);

                        //Determine which powerup to create and initialse the appropriate powerup entity
                        PowerupType powerupType = (PowerupType)Enum.Parse(typeof(PowerupType), powerupTilesetTile.Properties[PowerupTypeProperty]);
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
                        }
                    }

                    float bounceFactor = 0;
                    if (brickTilesetTile.Properties.ContainsKey("BounceFactor"))
                    {
                        bounceFactor = float.Parse(brickTilesetTile.Properties["BounceFactor"]);
                    }

                    //Add the brick
                    var brick = BrickFactory.Instance.CreateNew(brickImageFilename, brickPosition, 1, hitsToDestroy, powerup, bounceFactor, brickType);
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
                    TmxTilesetTile wormholeTilesetTile = wormholeTileset.Tiles[wormhole.Tile.Gid - wormholeTileset.FirstGid];
                    string wormholeImageFilename = Path.GetFileName(wormholeTilesetTile.Image.Source);

                    //Because we draw the bricks manually, with a gap, the coordinates for objects on the map need adjusting to our level coordinates.
                    //The coordinates will be out by the number of brick gaps between the point and 0,0
                    float xAdjustment = (int)(wormhole.X / tileMap.TileWidth) * GameConstants.BrickGap;
                    float yAdjustment = (int)(wormhole.Y / tileMap.TileHeight) * GameConstants.BrickGap;

                    CCPoint position = new CCPoint((float)wormhole.X + xAdjustment, (startY + tileHeight) - ((float)wormhole.Y + yAdjustment));

                    WormholeType wormholeType = (WormholeType)Enum.Parse(typeof(WormholeType), wormhole.Properties["WormholeType"]);

                    string exitName = string.Empty;
                    if (wormhole.Properties.ContainsKey("ExitName"))
                    {
                        exitName = wormhole.Properties["ExitName"];
                    }
                    WormholeFactory.Instance.CreateNew(wormholeImageFilename, position, wormholeType, wormhole.Name, exitName);
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
                    using (TitleContainer.OpenStream(BaseLevelFolder + (NumberOfLevels + 1).ToString("000") + ".tmx"))
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
