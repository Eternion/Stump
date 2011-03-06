using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stump.Server.DataProvider.Core
{
    public enum LoadingType
    {
        /// <summary>
        /// Load the entire file and store it into memory (Memory--/Speed++)
        /// </summary>
        PreLoading,
        /// <summary>
        /// Store into memory during a defined time objects that are requested once (Memory+-/Speed+-)
        /// </summary>
        CacheLoading,
    }
}
