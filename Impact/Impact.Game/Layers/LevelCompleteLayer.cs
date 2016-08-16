using CocosSharp;
using Impact.Game.Config;
using Impact.Game.Managers;

namespace Impact.Game.Layers
{
    public class LevelCompleteLayer : CCLayerColor
    {
        public LevelCompleteLayer(int timeBonus, int livesBonus, ScoreManager scoreManager) : base(new CCColor4B(0, 0, 0, 205))
        {
            const float leftX = 100;
            const string numberFormat = "000000";
            float middleX = (float)(GameConstants.WorldWidth * 0.5);
            float screenTop = GameConstants.WorldHeight;

            string timeBonusDisplay = timeBonus.ToString(numberFormat);
            string livesBonusDisplay = livesBonus.ToString(numberFormat);
            string scoreDisplay = scoreManager.Score.ToString(numberFormat);

            AddChild(new CCLabel("LEVEL COMPLETE", "visitor1.ttf", 84, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddle,
                PositionX = middleX,
                PositionY = screenTop - 450,
                Color = GameConstants.ImpactGreen
            });

            AddChild(new CCLabel($"TIME BONUS:     {timeBonusDisplay}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = leftX,
                PositionY = screenTop - 600,
                Color = GameConstants.ImpactYellow
            });

            AddChild(new CCLabel($"LIVES BONUS:    {livesBonusDisplay}", "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddleLeft,
                PositionX = leftX,
                PositionY = screenTop - 675,
                Color = GameConstants.ImpactYellow
            });

            AddChild(new CCLabel($"SCORE:    {scoreDisplay}", "visitor1.ttf", 72, CCLabelFormat.SystemFont)
            {
                AnchorPoint = CCPoint.AnchorMiddle,
                PositionX = middleX,
                PositionY = screenTop - 825,
                Color = GameConstants.ImpactYellow
            });

        }
    }
}
