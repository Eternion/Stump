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
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using Stump.BaseCore.Framework.Attributes;
using Stump.Database;
using Stump.Database.AuthServer;
using Stump.Database.Types;
using Stump.DofusProtocol;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.BaseServer.IPC;
using Stump.Server.BaseServer.Network;
using Stump.Server.BaseServer.Plugins;
using Stump.Server.WorldServer.Actions;
using Stump.Server.WorldServer.Commands;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer
{
    public class WorldServer : ServerBase<WorldServer>
    {
        [Variable]
        public static WorldServerInformation ServerInformation = new WorldServerInformation
        {
            Id = 1,
            Name = "Jiva",
            Address = "localhost",
            Port = 3467
        };

        [Variable]
        public static DatabaseConfiguration WorldDatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            Name = "stump_world",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/world/",
        };

        [Variable]
        public static DatabaseConfiguration DataDatabaseConfiguration = new DatabaseConfiguration
        {
            Host = "localhost",
            Name = "stump_data",
            User = "root",
            Password = "",
            UpdateFileDir = "./sql_update/data/",
        };

        public DatabaseAccessor WorldDbAccessor
        {
            get;
            private set;
        }

        public DatabaseAccessor DataDbAccessor
        {
            get;
            private set;
        }

        public WorldServer()
            : base(Definitions.ConfigFilePath, Definitions.SchemaFilePath)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            ConsoleInterface = new WorldConsole();
            ConsoleBase.SetTitle("#Stump World Server : " + ServerInformation.Name);


            logger.Info("Initializing World Database...");
            WorldDbAccessor = new DatabaseAccessor(WorldDatabaseConfiguration, Definitions.DatabaseRevision, typeof(WorldBaseRecord<>));

            logger.Info("Initializing Data Database...");
            DataDbAccessor = new DatabaseAccessor(DataDatabaseConfiguration, Definitions.DataDatabaseRevision, typeof(D2OBaseRecord<>));

            logger.Info("Start Database Engine...");
            DatabaseAccessor.StartEngine();

            logger.Info("Opening World Database...");
            WorldDbAccessor.OpenDatabase();

            logger.Info("Opening Data Database...");
            DataDbAccessor.OpenDatabase();

            logger.Info("Register Messages...");
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();

            logger.Info("Register Packet Handlers...");
            HandlerManager.RegisterAll(typeof (WorldServer).Assembly);

            logger.Info("Register Commands...");
            CommandManager.RegisterAll<WorldCommand, WorldSubCommand>(typeof(WorldCommand).Assembly);

            logger.Info("Start Parallel Initialization Procedure...");
            StageManager.Initialize(GetType().Assembly);

            logger.Info("Build World...");
            World.Instance.Initialize();

            logger.Info("Initializing IPC Client..");
            IpcAccessor.Instance.Initialize();
        }

        protected override void OnPluginAdded(PluginContext plugincontext)
        {
            CommandManager.RegisterAll<WorldCommand, WorldSubCommand>(plugincontext.PluginAssembly);

            base.OnPluginAdded(plugincontext);
        }

        public override void Start()
        {
            base.Start();

            logger.Info("Starting Console Handler Interface...");
            ConsoleInterface.Start();

            logger.Info("Starting Authenfication Server Communication on " + IpcAccessor.IpcAuthPort + "/" +
                        IpcAccessor.IpcWorldPort + "...");
            IpcAccessor.Instance.Start();
        }

        public override void Update()
        {
            base.Update();
        }

        public override BaseClient CreateClient(Socket s)
        {
            return new WorldClient(s);
        }

        public override void OnShutdown()
        {
        }

        public IEnumerable<WorldClient> GetClients()
        {
            return MessageListener.ClientList.OfType<WorldClient>();
        }

        public IEnumerable<WorldClient> GetClientsUsingAccount(AccountRecord account)
        {
            IEnumerable<WorldClient> clients = (from WorldClient entry in MessageListener.ClientList
                                                where entry.Account.Id == account.Id
                                                select entry);

            return clients;
        }

        public IEnumerable<WorldClient> GetClientsWithActiveCharacter()
        {
            return GetClients().Where(c => c.ActiveCharacter != null);
        }

    }
}