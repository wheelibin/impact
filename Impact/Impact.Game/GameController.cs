using System.Collections.Generic;
using CocosSharp;
using Impact.Game.Config;
using Impact.Scenes;

namespace Impact
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
            gameView.ResolutionPolicy = CCViewResolutionPolicy.ExactFit;

            // Determine whether to use the high or low def versions of our images
            // Make sure the default texel to content size ratio is set correctly
            // Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            if (GameConstants.WorldWidth < viewSize.Width)
            {
                contentSearchPaths.Add("Images/Hd");
                CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            }
            else
            {
                contentSearchPaths.Add("Images/Ld");
                CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            }

            gameView.ContentManager.SearchPaths = contentSearchPaths;
            //gameView.Stats.Enabled = true;
            gameView.RunWithScene(new TitleScene(gameView));

        }

        public static void GoToScene(CCScene scene)
        {
            GameView.Director.ReplaceScene(scene);
        }
    }
}
