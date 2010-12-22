// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;

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
        public static List<string> BotChannels = new List<string> {"#stump"};

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
        public static string[] AllowedUserNicks = new[] { "bouh2" };

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
        public static string DofusPath = @"C:\Program Files\Dofus 2\";

        /// <summary>
        /// Path to Dofus source file (DofusInvoker.swf)
        /// </summary>
        [Variable]
        public static string DofusSourcePath = @"C:\Program Files\Dofus 2\app\DofusInvoker";


        private readonly Dictionary<string, Assembly> m_loadedAssemblies;

        public Bot()
        {
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);

            ConfigFile = new XmlConfigFile(ConfigPath, SchemaPath);
            ConfigFile.DefinesVariables(ref m_loadedAssemblies);

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

        public XmlConfigFile ConfigFile
        {
            get;
            private set;
        }
    }
}