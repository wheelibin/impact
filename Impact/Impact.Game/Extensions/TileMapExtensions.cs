using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace Impact.Game.Extensions
{
    public static class TileMapExtensions
    {

        public static T GetPropertyValue<T>(this PropertyDict properties, string name, Func<string, T> conversionFunc)
        {
            if (properties.ContainsKey(name))
            {
                return conversionFunc(properties[name]);
            }

            return default(T);
        }

    }
}
