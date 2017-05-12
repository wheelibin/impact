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

        /// <summary>
        /// The time the level has been running, in seconds
        /// </summary>
        public float LevelRunTime { get; set; }

        public CCSpriteSheet GameEntitiesSpriteSheet { get; set; }
        public CCSpriteSheet TitleScreenSpriteSheet { get; set; }
        public CCSpriteSheet LevelSelectScreenSpriteSheet { get; set; }

        public event Action LivesChanged;
        public event Action<int> MusicVolumeChanged;
        public event Action<int> SfxVolumeChanged;

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
            Lives = lives;
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

        public void SetMusicVolume(int volume)
        {
            MusicVolumeChanged?.Invoke(volume);
        }

        public void SetSfxVolume(int volume)
        {
            SfxVolumeChanged?.Invoke(volume);
        }

    }
}
