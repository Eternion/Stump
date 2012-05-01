
using System.Drawing;
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

        [Variable(true)]
        public static string HtmlMOTDColor = ColorTranslator.ToHtml(Color.OrangeRed);

        public static Color MOTDColor
        {
            get { return ColorTranslator.FromHtml(HtmlMOTDColor); }
            set { HtmlMOTDColor = ColorTranslator.ToHtml(value); }
        }
    }
}