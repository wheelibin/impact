using System;
using CocosSharp;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    public class SwitchableElementFactory
    {
        //Singleton
        private static readonly Lazy<SwitchableElementFactory> SelfInstance = new Lazy<SwitchableElementFactory>(() => new SwitchableElementFactory());
        public static SwitchableElementFactory Instance => SelfInstance.Value;

        public event Action<SwitchableElement> SwitchableElementCreated;


        public SwitchableElement CreateNew(string spriteImageFilename, CCPoint position, CCSize size, string toggleSwitchName, string objectName)
        {
            SwitchableElement elem = new SwitchableElement(spriteImageFilename, position, size, toggleSwitchName, objectName);
            SwitchableElementCreated?.Invoke(elem);
            return elem;
        }
    }

}


