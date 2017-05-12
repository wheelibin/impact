using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Helpers;
using Impact.Game.Managers;
using Impact.Game.Scenes;

namespace Impact.Game
{
    public static class GameController
    {
        public static CCGameView GameView
        {
            get;
            private set;
        }

        public static void Initialize(CCGameView gameView)
        {

            GameView = gameView;

            var contentSearchPaths = new List<string> { "Fonts", "Sounds" };
            CCSizeI viewSize = gameView.ViewSize;

            // Set world dimensions
            gameView.DesignResolution = new CCSizeI(GameConstants.WorldWidth, GameConstants.WorldHeight);
            gameView.ResolutionPolicy = CCViewResolutionPolicy.ShowAll;

            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            //if (GameConstants.WorldWidth < viewSize.Width)
            //{
            //    contentSearchPaths.Add("Images/Hd");
            //    CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            //}
            //else
            //{
            contentSearchPaths.Add("Images/Ld");
            CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            //}
            
            gameView.ContentManager.SearchPaths = contentSearchPaths;
            gameView.Stats.Enabled = true;
            //GameStateManager.Instance.DebugMode = true;
            Settings.HighestCompletedLevel = 99;

            CCAudioEngine.SharedEngine.BackgroundMusicVolume = (float) Settings.MusicVolume/10;
            CCAudioEngine.SharedEngine.PlayBackgroundMusic("BackgroundMusic.mp3", loop: true);
            
            GameStateManager.Instance.MusicVolumeChanged += GameStateManager_MusicVolumeChanged;
            GameStateManager.Instance.SfxVolumeChanged += GameStateManager_SfxVolumeChanged;
            
            gameView.RunWithScene(new TitleScene(gameView));
        }

        public static void GoToScene(CCScene scene)
        {
            GameView.Director.ReplaceScene(new CCTransitionFade(1,scene));
        }

        private static void GameStateManager_SfxVolumeChanged(int volume)
        {
            CCAudioEngine.SharedEngine.EffectsVolume = (float)volume / 10;
            //play a sound to hear the effect of the change
            CCAudioEngine.SharedEngine.PlayEffect(GameConstants.PaddleHitSound);
        }

        private static void GameStateManager_MusicVolumeChanged(int volume)
        {
            CCAudioEngine.SharedEngine.BackgroundMusicVolume = (float)volume / 10;
        }
    }
}
