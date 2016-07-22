using System;
using System.Collections.Generic;
using System.IO;
using CocosSharp;
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
    public class LevelManager
    {
        private static readonly Lazy<LevelManager> SelfInstance = new Lazy<LevelManager>(() => new LevelManager());

        public static LevelManager Instance => SelfInstance.Value;
        public int NumberOfLevels { get; private set; }
        public int CurrentLevel{ get; set; }
        public LevelProperties CurrentLevelProperties { get; private set; }

        public LevelManager()
        {
            DetermineAvailableLevels();
        }

        public List<Brick> LoadLevel(int level, Paddle paddle, List<Ball> balls)
        {
            var bricks = new List<Brick>();

            var tileMap = new TmxMap("Content/Levels/level" + level.ToString("000") + ".tmx");

            CurrentLevelProperties = new LevelProperties
            {
                FinalBallSpeedPercentageIncrease = int.Parse(tileMap.Properties["FinalBallSpeedPercentageIncrease"])
            };

            int tileWidth = 114 + GameConstants.BrickGap;
            int tileHeight = 38 + GameConstants.BrickGap;

            int tileMapHeight = tileMap.Height * tileHeight;

            int yOffset = 300;

            int startY = tileMapHeight + yOffset;

            TmxLayer brickLayer = tileMap.Layers["Bricks"];
            TmxLayer powerupLayer = tileMap.Layers["Powerups"];

            TmxTileset brickTileset = tileMap.Tilesets["Bricks"];
            TmxTileset powerupTileset = tileMap.Tilesets["Powerups"];

            for (int t = 0; t < brickLayer.Tiles.Count; t++)
            {

                TmxLayerTile brickTile = brickLayer.Tiles[t];
                TmxLayerTile powerupTile = powerupLayer.Tiles[t];

                if (brickTile.Gid > 0)
                {
                    TmxTilesetTile brickTilesetTile = brickTileset.Tiles[brickTile.Gid - brickTileset.FirstGid];
                    string brickImageFilename = Path.GetFileName(brickTilesetTile.Image.Source);

                    int hitsToDestroy = int.Parse(brickTilesetTile.Properties["HitsToDestroy"]);

                    CCPoint brickPosition = new CCPoint(brickTile.X * tileWidth, startY - (brickTile.Y * tileHeight));

                    Powerup powerup = null;
                    if (powerupTile.Gid > 0)
                    {
                        TmxTilesetTile powerupTilesetTile = powerupTileset.Tiles[powerupTile.Gid - powerupTileset.FirstGid];
                        string powerupImageFilename = Path.GetFileName(powerupTilesetTile.Image.Source);

                        PowerupType powerupType = (PowerupType)Enum.Parse(typeof(PowerupType), powerupTilesetTile.Properties["PowerupType"]);
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

                    var brick = BrickFactory.Instance.CreateNew(brickImageFilename, brickPosition, 1, hitsToDestroy, powerup);
                    bricks.Add(brick);

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
                    using (TitleContainer.OpenStream("Content/Levels/level" + (NumberOfLevels + 1).ToString("000") + ".tmx"))
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
