using CocosSharp;
using Impact.Enums;
using Impact.Game.Managers;

namespace Impact.Entities
{
    public class Wormhole : CCNode
    {
        public WormholeType WormholeType { get; set; }
        public string ObjectName { get; set; }
        public string ExitName { get; set; }

        public Wormhole(string spriteImageFilename, CCPoint position, WormholeType wormholeType, string objectName, string exitName)
        {
            WormholeType = wormholeType;
            ObjectName = objectName;
            ExitName = exitName;
            //AddChild(new CCLayerColor(CCColor4B.Yellow));
            var frame = GameManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
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
