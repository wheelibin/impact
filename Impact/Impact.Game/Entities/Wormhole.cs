using CocosSharp;
using Impact.Game.Enums;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    /// <summary>
    /// Represents a wormhole, a portal the ball can travel through
    /// </summary>
    public class Wormhole : CCNode
    {
        public WormholeType WormholeType { get; set; }
        public string ObjectName { get; set; }
        public string ExitName { get; set; }
        public WormholeExitDirection ExitDirection { get; }
        public bool InUse { get; set; }

        public Wormhole(string spriteImageFilename, CCPoint position, WormholeType wormholeType, string objectName, string exitName, WormholeExitDirection exitDirection)
        {
            WormholeType = wormholeType;
            ObjectName = objectName;
            ExitName = exitName;
            ExitDirection = exitDirection;

            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            AddChild(sprite);

            ContentSize = sprite.ContentSize;
            AnchorPoint = CCPoint.AnchorLowerLeft;
            Position = position;
            
        }

    }
}
