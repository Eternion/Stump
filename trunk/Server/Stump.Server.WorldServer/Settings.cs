
using Stump.Core.Attributes;

namespace Stump.Server.WorldServer
{
    /// <summary>
    ///   Global settings defined by the config file
    /// </summary>
    public class Settings
    {
        [Variable(true)]
        public static string MOTD = "Bienvenue sur le serveur test de <b>Stump v. pre-alpha by bouh2</b>";
    }
}