
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.Core.Attributes;
using Stump.Core.Xml;
using Stump.Core.Xml.Config;

namespace Stump.Tools.UtilityBot
{
    public class Bot
    {
        private const string ConfigPath = "./utilitybot_config.xml";
        private const string SchemaPath = "./utilitybot_config.xsd";

        /// <summary>
        /// IRC server adress
        /// </summary>
        [Variable]
        public static string IrcServer = "irc.epiknet.org";

        /// <summary>
        /// IRC server port
        /// </summary>
        [Variable]
        public static int IrcPort = 6667;

        /// <summary>
        /// IRC bot channels
        /// </summary>
        [Variable]
        public static List<string> BotChannels = new List<string> {"#stump", "#pmg"};

        /// <summary>
        /// IRC commands prefix
        /// </summary>
        [Variable]
        public static string CommandPrefix = "!";

        /// <summary>
        /// IRC bots usernames
        /// </summary>
        [Variable]
        public static string[] BotNicknames = new[] {"StumpBot", "StumpBot-2", "StumpBot-3"};

        /// <summary>
        /// Usernames allowed on the IRC
        /// </summary>
        [Variable]
        public static string[] AllowedUserNicks = new[] { "bouh2", "nath2" };

        /// <summary>
        /// IRC username
        /// </summary>
        [Variable]
        public static string IrcUserName = "UtilityBot";

        /// <summary>
        /// IRC user informations
        /// </summary>
        [Variable]
        public static string IrcUserInfo = "Bot";

        /// <summary>
        /// Path to Dofus 2 folder
        /// </summary>
        [Variable]
        public static string DofusPath = @"C:\Program Files (x86)\Dofus 2\";

        /// <summary>
        /// Path to Dofus source file (DofusInvoker.swf)
        /// </summary>
        [Variable]
        public static string DofusSourcePath = @"C:\Program Files\Dofus 2\app\DofusInvoker";


        private readonly Dictionary<string, Assembly> m_loadedAssemblies;

        public Bot()
        {
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);

            Config = new XmlConfig(ConfigPath, SchemaPath);

            Connection = new IrcConnection(BotChannels, CommandPrefix)
                {
                    Nicks = BotNicknames,
                    UserName = IrcUserName,
                    Info = IrcUserInfo
                };

            Connection.BeginConnect(IrcServer, IrcPort);
        }

        public IrcConnection Connection
        {
            get;
            private set;
        }

        public XmlConfig Config
        {
            get;
            private set;
        }
    }
}