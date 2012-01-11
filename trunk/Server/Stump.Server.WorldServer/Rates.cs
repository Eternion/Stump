using System;
using Stump.Core.Attributes;

namespace Stump.Server.WorldServer
{
    [Serializable]
    public static class Rates
    {
        /// <summary>
        /// Life regen rate (default 1 => 1hp/2seconds. Max = 20)
        /// </summary>
        [Variable(true)]
        public static float RegenRate = 1;

        [Variable(true)]
        public static float XpRate = 1;

        [Variable(true)]
        public static float KamasRate = 1;

        [Variable(true)]
        public static float DropsRate = 1;

    }
}