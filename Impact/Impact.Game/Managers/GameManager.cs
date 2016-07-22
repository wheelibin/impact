﻿using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Scenes;

namespace Impact.Game.Managers
{
    public class GameManager
    {
        private static readonly Lazy<GameManager> SelfInstance = new Lazy<GameManager>(() => new GameManager());
        public static GameManager Instance => SelfInstance.Value;

        public int Score { get; set; }
        public bool LevelHasStarted { get; private set; }
        public bool CheatModeEnabled { get; set; }
        public CCSpriteSheet GameEntitiesSpriteSheet { get; set; }
        public CCSpriteSheet TitleScreenSpriteSheet { get; set; }
        public LevelSelectScene LevelSelectScene { get; set; }
        public GameScene GameScene { get; set; }
        public List<string> BrickSounds { get; set; }

        public GameManager()
        {
            LevelHasStarted = false;
            GameEntitiesSpriteSheet = new CCSpriteSheet("Spritesheets/GameEntities.plist", "Spritesheets/GameEntities.png");
            TitleScreenSpriteSheet = new CCSpriteSheet("Spritesheets/TitleScreen.plist", "Spritesheets/TitleScreen.png");

            BrickSounds = new List<string>
            {
                "massive-banzai23/D3.mp3",
                "massive-banzai23/E3.mp3",
                "massive-banzai23/F3.mp3",
                "massive-banzai23/G3.mp3",
                "massive-banzai23/A3.mp3",
                "massive-banzai23/As3.mp3",
                "massive-banzai23/C3.mp3",
                "massive-banzai23/D4.mp3"
            };

        }

        public event Action<bool> LevelStarted;

        public void StartStopLevel(bool start)
        {
            LevelStarted?.Invoke(start);
            LevelHasStarted = start;
        }

    }
}
