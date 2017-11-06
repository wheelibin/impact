using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public class Switch : CCNode
    {
        public string ObjectName { get; set; }

        public Switch(string spriteImageFilename, CCPoint position, string objectName)
        {
            ObjectName = objectName;

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
