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
        [Variable]
        public static float RegenRate = 1;

        [Variable]
        public static float XpRate = 1;
    }
}