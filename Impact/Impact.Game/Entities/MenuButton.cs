using System;
using CocosSharp;
using Impact.Game;
using Impact.Game.Managers;

namespace Impact.Entities
{
    public class MenuButton : CCNode
    {
        private readonly Action _clickCallback;
        public int Id { get; private set; }

        public MenuButton(int id, string textureFilename, string text, Action clickCallback)
        {
            _clickCallback = clickCallback;
            Id = id;
            var frame = GameManager.Instance.SpriteSheet.Frames.Find(item => item.TextureFilename == textureFilename);
            var button = new CCSprite(frame)
            {
                AnchorPoint = CCPoint.AnchorLowerLeft
            };
            AddChild(button);
            ContentSize = button.ContentSize;

            var label = new CCLabel(text, "visitor1.ttf", 48, CCLabelFormat.SystemFont)
            {
                Position = ContentSize.Center,
                AnchorPoint = CCPoint.AnchorMiddle
            };
            AddChild(label);

            //var touchListener = new CCEventListenerTouchAllAtOnce { OnTouchesEnded = OnTouchesEnded };
            //AddEventListener(touchListener, this);
        }

        public void Click()
        {
            _clickCallback();
        }

        //private void OnTouchesEnded(List<CCTouch> touches, CCEvent touchEvent)
        //{
        //    if (touches.Count == 1)
        //    {
        //        ButtonClicked?.Invoke(this);
        //        touchEvent.StopPropogation();
        //    }
        //}
    }
}
