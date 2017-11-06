using CocosSharp;
using Impact.Game.Managers;

namespace Impact.Game.Entities
{
    public class SwitchableElement : CCNode
    {
        public string TogglwSwitchName { get; set; }
        public bool ElementVisible { get; set; }
        public string ObjectName { get; set; }
      
        public SwitchableElement(string spriteImageFilename, CCPoint position, CCSize size, string toggleSwitchName, string objectName)
        {
            ObjectName = objectName;
            ElementVisible = true;
            TogglwSwitchName = toggleSwitchName;

            var frame = GameStateManager.Instance.GameEntitiesSpriteSheet.Frames.Find(item => item.TextureFilename == spriteImageFilename);
            var sprite = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft,
                ContentSize = size
            };

            AddChild(sprite);

            ContentSize = sprite.ContentSize;
            AnchorPoint = CCPoint.AnchorLowerLeft;
            Position = position;
        }
    }
}
