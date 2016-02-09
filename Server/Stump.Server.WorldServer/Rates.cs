using System;
using Stump.Core.Attributes;

namespace Stump.Server.WorldServer
{
    [Serializable]
    public static class Rates
    {
        /// <summary>
        /// Life regen rate (default 2 => 2hp/seconds.)
        /// </summary>
        [Variable(true)]
        public static float RegenRate = 2;

        [Variable(true)]
        public static float XpRate = 1;

        [Variable(true)]
        public static float KamasRate = 1;

        [Variable(true)]
        public static float DropsRate = 1;

        [Variable(true)]
        public static float JobXpRate = 1;
    }
}