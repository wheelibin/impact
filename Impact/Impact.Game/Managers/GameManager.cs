using System;
using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Scenes;

namespace Impact.Game.Managers
{
    public class GameManager
    {
        private static readonly Lazy<GameManager> SelfInstance = new Lazy<GameManager>(() => new GameManager());
        public static GameManager Instance => SelfInstance.Value;

        public bool DebugMode { get; set; }
        public bool LevelHasStarted { get; private set; }
        public bool CheatModeEnabled { get; set; }
        public CCSpriteSheet GameEntitiesSpriteSheet { get; set; }
        public CCSpriteSheet TitleScreenSpriteSheet { get; set; }
        public CCSpriteSheet LevelSelectScreenSpriteSheet { get; set; }

        public GameManager()
        {
            LevelHasStarted = false;
            GameEntitiesSpriteSheet = new CCSpriteSheet(GameConstants.GameEntitiesSpriteSheet, GameConstants.GameEntitiesSpriteSheetImage);
            TitleScreenSpriteSheet = new CCSpriteSheet(GameConstants.TitleScreenSpriteSheet, GameConstants.TitleScreenSpriteSheetImage);
            LevelSelectScreenSpriteSheet = new CCSpriteSheet(GameConstants.LevelSelectScreenSpriteSheet, GameConstants.LevelSelectScreenSpriteSheetImage);
        }

        public event Action<bool> LevelStarted;

        public void StartStopLevel(bool start)
        {
            LevelStarted?.Invoke(start);
            LevelHasStarted = start;
        }

    }
}
