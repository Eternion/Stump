
using Stump.BaseCore.Framework.Attributes;

namespace Stump.Server.WorldServer
{
    /// <summary>
    ///   Global settings defined by the config file
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// Path of content folder
        /// </summary>
        [Variable]
        public static string ContentPath = "./../../content/";

        /// <summary>
        /// Path of static folder
        /// </summary>
        [Variable]
        public static string StaticPath = "./../../static/";
    }
}