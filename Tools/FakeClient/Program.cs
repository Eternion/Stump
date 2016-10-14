using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FakeClient.Handlers;
using NLog.Config;
using NLog.Targets;
using Stump.Core.IO;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;

namespace FakeClient
{
    class Program
    {
        public static FakeClientPacketHandler Handler
        {
            get;
            set;
        }

        static void Main(string[] args)
        {
            MessageReceiver.Initialize();
            ProtocolTypeManager.Initialize();
            NLogHelper.DefineLogProfile(false, true);
            NLogHelper.EnableLogging();
            Handler = new FakeClientPacketHandler();
            Handler.RegisterAll(Assembly.GetExecutingAssembly());
            

            var client1 = new FakeClient(1)
            {
                AccountName = "loom",
                AccountPassword = "******"
            };

            var client2 = new FakeClient(2)
            {
                AccountName = "loom",
                AccountPassword = "******"
            };

            client1.Connect("31.220.41.13", 5555);

            int pass = 0;
            while (true)
            {
                Thread.Sleep(10);

                if (pass == 0 && client1.WorldIp != null && !client1.IsConnected && !client2.IsConnected)
                {
                    client2.Connect("31.220.41.13", 5555);
                    pass++;
                }

                if (pass == 1 && client2.WorldIp != null && !client2.IsConnected)
                {
                    client1.Connect(client1.WorldIp, client1.WorldPort);
                    pass++;
                }

                if (pass == 2 && client1.KamasTransferred)
                {
                    client2.Connect(client2.WorldIp, client2.WorldPort);
                    pass++;
                }

                if (pass == 3 && client2.IsInGame)
                {
                    client2.Disconnect();
                    break;
                }
            }
        }
    }
}
