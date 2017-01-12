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

            client1.Connect("31.220.41.13", 5555);
        }
    }
}
