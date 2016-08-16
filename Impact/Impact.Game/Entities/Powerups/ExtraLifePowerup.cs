using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities.Powerups
{
    /// <summary>
    /// A powerup that grants the player an extra life
    /// </summary>
    public class ExtraLifePowerup : Powerup
    {
        public ExtraLifePowerup(string imageFilename, CCPoint initialPosition)
            : base(initialPosition, imageFilename)
        {
            
        }

        /// <summary>
        /// Add an extra life
        /// </summary>
        public override void Activate()
        {
            GameStateManager.Instance.AddLife();
        }

        /// <summary>
        /// Not applicable in this powerup
        /// </summary>
        public override void Deactivate()
        {
            return;
        }
    }
}
