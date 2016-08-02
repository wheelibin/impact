namespace Impact.Managers
{
    public interface IScoreManager
    {
        int Score { get; }
        void BrickDestroyed();
        void PowerupCollected();
    }
}
