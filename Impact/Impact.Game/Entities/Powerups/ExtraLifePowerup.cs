using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that grants the player and extra life
    /// </summary>
    public class ExtraLifePowerup : Powerup
    {
        public ExtraLifePowerup(string imageFilename, CCPoint initialPosition)
            : base(initialPosition, imageFilename)
        {
            
        }

        public override void Activate()
        {
            GameStateManager.Instance.AddLife();
        }

        public override void Deactivate()
        {
            return;
        }
    }
}
