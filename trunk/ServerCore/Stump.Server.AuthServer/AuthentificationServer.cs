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
using System.Net.Sockets;
using System.Reflection;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Stump.Server.AuthServer.Commands;
using Stump.Server.AuthServer.IPC;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;

namespace Stump.Server.AuthServer
{
    public class AuthentificationServer : ServerBase<AuthentificationServer>
    {
        /// <summary>
        ///   Bag containing client currently under authentification process.
        /// </summary>
        //private ConcurrentBag<AuthClient> m_AuthClients = new ConcurrentBag<AuthClient>();
        public AuthentificationServer() :
            base(Definitions.ConfigFilePath, Definitions.SchemaFilePath)
        {
        }

        public override void Initialize()
        {
            try
            {
                base.Initialize();
                ConsoleInterface = new AuthConsole();
                ConsoleBase.SetTitle("#Stump Authentification Server");


                logger.Info("Initializing Database...");
                DatabaseAccessor.Initialize(
                    Assembly.GetExecutingAssembly(),
                    Definitions.DatabaseRevision,
                    DatabaseType.MySQL,
                    DatabaseService.AuthServer);

                logger.Info("Opening Database...");
                DatabaseAccessor.OpenDatabase();

                logger.Info("Initializing WorldServers Manager");
                WorldServerManager.Initialize();

                logger.Info("Register Messages...");
                MessageReceiver.Initialize();
                ProtocolTypeManager.Initialize();

                logger.Info("Register Packets Handlers...");
                HandlerManager.RegisterAll(typeof (AuthentificationServer).Assembly);

                logger.Info("Register Commands...");
                CommandManager.RegisterAll<AuthCommand, AuthSubCommand>(typeof(AuthCommand).Assembly);
            }
            catch (Exception ex)
            {
                logger.Fatal("Cannot initialize Server : " + ex);
                Shutdown();
            }
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll<AuthCommand, AuthSubCommand>(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Start World Servers Manager");
            WorldServerManager.Start();

            logger.Info("Starting IPC Server..");
            IpcServer.Instance.Initialize();
        }

        public override void OnShutdown()
        {
        }

        public override BaseClient CreateClient(Socket s)
        {
            return new AuthClient(s);
        }

        public bool DisconnectClientsUsingAccount(AccountRecord account)
        {
            var clients = GetClientsUsingAccount(account).ToArray();

            for (int i = 0; i < clients.Length; i++)
            {
                clients[i].Disconnect();
            }

            if (IpcServer.Instance.GetIpcClients().Any(ipcclient => ipcclient.DisconnectConnectedAccount(account)))
            {
                return true;
            }

            return clients.Count() > 0;
        }

        public IEnumerable<AuthClient> GetClientsUsingAccount(AccountRecord account)
        {
            return (from AuthClient entry in MessageListener.ClientList
                    where entry.Account != null && entry.Account.Id == account.Id
                    select entry);
        }

        public IEnumerable<AuthClient> GetClientsLookingOfServers()
        {
            return (from AuthClient entry in MessageListener.ClientList
                    where entry.LookingOfServers
                    select entry);
        }
    }
}