using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Scenes;

namespace Impact.Game.Managers
{
    public class GameManager
    {
        private static readonly Lazy<GameManager> SelfInstance = new Lazy<GameManager>(() => new GameManager());

        // simple singleton implementation
        public static GameManager Instance => SelfInstance.Value;

        public int Score { get; set; }
        public bool LevelHasStarted { get; private set; }
        public bool CheatModeEnabled { get; set; }
        public CCSpriteSheet SpriteSheet { get; set; }
        public LevelSelectScene LevelSelectScene { get; set; }
        public List<string> BrickSounds { get; set; }

        public GameManager()
        {
            LevelHasStarted = false;
            SpriteSheet = new CCSpriteSheet("Spritesheets/GameEntities.plist", "Spritesheets/GameEntities.png");

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
