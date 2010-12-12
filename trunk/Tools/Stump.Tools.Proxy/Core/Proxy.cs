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
using System.Net;
using System.Reflection;
using NLog;
using Stump.BaseCore.Framework.XmlUtils;
using Stump.Tools.Proxy.Messages;

namespace Stump.Tools.Proxy
{
    internal static class Proxy
    {
        private static Dictionary<string, Assembly> m_loadedAssemblies;
        private static Logger logger;
        private static XmlConfigFile m_configFile;
        public static HandlerManager HandlerManager = new HandlerManager();
        public static List<WorldDerivedConnexion> clientList = new List<WorldDerivedConnexion>();
        public static ClientListener authClientListener;
        public static ClientListener worldClientListener;


        public static void Initialize()
        {
            /* Initialize Config File */
            m_loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToDictionary(entry => entry.GetName().Name);
            m_configFile = new XmlConfigFile("proxy_config.xml", "proxy_config.xsd");
            // m_configFile.DefinesVariables(ref m_loadedAssemblies);

            /*Initialize HandlerManager */
            HandlerManager.RegisterAll(typeof (IdentificationSuccessMessageHandler).Assembly);

            /* Create Auth et World Client Listener */
            authClientListener = new ClientListener("127.0.0.1", 5555, 2000, 2000);
            worldClientListener = new ClientListener("127.0.0.1", 5556, 2000, 2000);

            /* Initialize AuthClient Listener */
            authClientListener.Init();

            /* Handle AuthClient Connexion */
            authClientListener.onClientConnexion += onNewAuthClient;

            /* Initialize WorldClient Listener */
            worldClientListener.Init();

            /* Handle WorldClient Connexion */
            worldClientListener.onClientConnexion += onNewWorldClient;

            /* Start AuthClient Listener */
            authClientListener.Start();

            /* Start WorldClient Listener */
            worldClientListener.Start();
        }


        public static void onNewAuthClient(Client client)
        {
//"213.248.126.180"
            Console.WriteLine("New Auth Client <{0}>", client.IP);
            DerivedConnexion derivedConnexion =
                new AuthDerivedConnexion(new IPEndPoint(IPAddress.Parse("193.238.148.207"), 5555), client);
        }

        public static void onNewWorldClient(Client client)
        {
            Console.WriteLine("New World Client <{0}>", client.IP);
            var derivedConnexion = new WorldDerivedConnexion(client);
            clientList.Add(derivedConnexion);
        }
    }
}