namespace Impact.Game.Managers
{
    public interface IScoreManager
    {
        int Score { get; }
        void BrickDestroyed();
        void PowerupCollected();
        int GetHighScoreForLevel(int level);
    }
}
