
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.BaseServer
{
    /// <summary>
    ///   Global settings defined by the config file
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Enable/Disable perfomances tracing
        /// </summary>
        [Variable]
        public static bool EnableBenchmarking;

        /// <summary>
        ///   Disconnect Client after specified time(in s) or NULL for desactivate
        /// </summary>
        [Variable]
        public static int? InactivityDisconnectionTime = 900;
    }
}