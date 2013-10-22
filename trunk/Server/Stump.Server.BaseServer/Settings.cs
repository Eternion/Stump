
using Stump.Core.Attributes;
using Stump.Core.I18N;

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
        public static readonly bool EnableBenchmarking;

        /// <summary>
        ///   Disconnect Client after specified time(in s) or NULL for desactivate
        /// </summary>
        [Variable]
        public static readonly int? InactivityDisconnectionTime = 900;

        [Variable]
        public static readonly Languages Language = Languages.English;
    }
}