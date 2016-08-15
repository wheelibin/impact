using System;
using CocosSharp;
using Impact.Game.Config;

namespace Impact.Game.Managers
{
    /// <summary>
    /// Stuff and things relating to the game state
    /// </summary>
    public class GameStateManager
    {
        private static readonly Lazy<GameStateManager> SelfInstance = new Lazy<GameStateManager>(() => new GameStateManager());
        public static GameStateManager Instance => SelfInstance.Value;

        public bool DebugMode { get; set; }
        public bool LevelHasStarted { get; private set; }
        public bool CheatModeEnabled { get; set; }
        public int Lives { get; private set; }

        public CCSpriteSheet GameEntitiesSpriteSheet { get; set; }
        public CCSpriteSheet TitleScreenSpriteSheet { get; set; }
        public CCSpriteSheet LevelSelectScreenSpriteSheet { get; set; }

        public event Action LivesChanged;

        public GameStateManager()
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

        public void SetLives(int lives)
        {
            Lives = 2;
            LivesChanged?.Invoke();
        }

        public void AddLife()
        {
            Lives += 1;
            LivesChanged?.Invoke();
        }

        public void LoseLife()
        {
            Lives -= 1;
            LivesChanged?.Invoke();
        }

    }
}
