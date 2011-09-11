using System;
using Stump.Core.Attributes;

namespace Stump.Server.WorldServer
{
    [Serializable]
    public static class Rates
    {
        /// <summary>
        /// Life regen rate (default 1 => 20 points per minutes)
        /// </summary>
        [Variable]
        public static float RegenRate = 1;
    }
}