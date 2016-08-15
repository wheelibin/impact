using System;

namespace Impact.Game.Extensions
{
    public static class NumberExtensions
    {

        /// <summary>
        /// Applies a percentage variation to the supplied number
        /// </summary>
        public static float ApplyVariation(this float input, int variationPercentage = 5)
        {
            float variationAmount = (input / 100) * variationPercentage;

            var rnd = new Random();

            if (input < 0)
            {
                return rnd.Next((int) (input + variationAmount), (int) (input - variationAmount));
            }
            return rnd.Next((int) (input - variationAmount), (int) (input + variationAmount));
        }

    }
}
