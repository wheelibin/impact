using System;
using CocosSharp;
using Impact.Game.Entities;

namespace Impact.Game.Factories
{
    public class SwitchFactory
    {
        //Singleton
        private static readonly Lazy<SwitchFactory> SelfInstance = new Lazy<SwitchFactory>(() => new SwitchFactory());
        public static SwitchFactory Instance => SelfInstance.Value;

        public event Action<Switch> SwitchCreated;

        public Switch CreateNew(string spriteImageFilename, CCPoint position, string objectName)
        {
            Switch newSwitch = new Switch(spriteImageFilename, position, objectName);
            SwitchCreated?.Invoke(newSwitch);
            return newSwitch;
        }
    }
}
