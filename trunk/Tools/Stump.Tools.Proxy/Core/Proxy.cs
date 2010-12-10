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
using System.Text;
using System.Reflection;
using System.Net;
using System.Runtime;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.Server.BaseServer;
using Stump.Tools.Proxy.Messages;

namespace Stump.Tools.Proxy
{
    public static class Proxy
    {
        private static Dictionary<string, Assembly> m_loadedAssemblies;
        private static Logger logger;
        private static XmlConfigFile m_configFile;
        public static HandlerManager HandlerManager= new HandlerManager();

        private static ClientListener m_authClientListener;
        private static ClientListener m_worldClientListener;


        public static void Initialize()
        {
            /* Initialize Config File */
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            m_configFile = new XmlConfigFile("proxy_config.xml", "proxy_config.xsd");
            m_configFile.DefinesVariables(ref m_loadedAssemblies);

            /*Initialize HandlerManager */
            HandlerManager.RegisterAll(typeof(IdentificationSuccessMessageHandler).Assembly);

            /* Initialize AuthClient Listener */
            m_authClientListener.Init();

            /* Handle AuthClient Connexion */
            m_authClientListener.onClientConnexion += onNewAuthClient;

            /* Initialize WorldClient Listener */
            m_worldClientListener.Init();

            /* Handle WorldClient Connexion */
            m_worldClientListener.onClientConnexion += onNewWorldClient;

            /* Start AuthClient Listener */
            m_authClientListener.Start();

            /* Start WorldClient Listener */
            m_worldClientListener.Start();
        }


        public static void onNewAuthClient(Client client)
        {
            DerivedConnexion derivedConnexion = new AuthDerivedConnexion(new IPEndPoint(IPAddress.Parse(AuthHost),AuthPort), client);
        }

        public static void onNewWorldClient(Client client)
        {
            DerivedConnexion derivedConnexion = new WorldDerivedConnexion(client);
        }

    }
}
