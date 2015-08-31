using System;
using System.Collections.Generic;
using System.Linq;
using CSScriptLibrary;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Benchmark;
using Stump.Server.BaseServer.Network;
using System.Net.Sockets;
using Stump.DofusProtocol.Messages.Custom;
using Stump.Core.IO;
using System.Threading;
using NLog;
using Stump.DofusProtocol.Messages;

namespace Stump.Server.BaseServer.Commands.Commands
{
    public class BenchmarkCommands : SubCommandContainer
    {
        public BenchmarkCommands()
        {
            Aliases = new[] { "benchmark", "bench" };
            RequiredRole = RoleEnum.Administrator;
        }
    }

    public class BenchmarkSummaryCommand : SubCommand
    {
        public BenchmarkSummaryCommand()
        {
            Aliases = new[] { "summary", "sum" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<string>("type", "t", "Entry type", isOptional:true);
            AddParameter<string>("msg", "m", "Specific message", isOptional:true);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger.IsArgumentDefined("type"))
            {
                var type = trigger.Get<string>("type");

                var entries = BenchmarkManager.Instance.Entries.Where(x => x.AdditionalProperties.ContainsKey("type") 
                    && x.AdditionalProperties["type"].Equals(type));

                trigger.Reply(BenchmarkManager.Instance.GenerateReport(entries).HtmlEntities());
            }
            else if (trigger.IsArgumentDefined("msg"))
            {
                var msg = trigger.Get<string>("msg");

                var entries = BenchmarkManager.Instance.Entries.Where(x => x.MessageType.Equals(msg, StringComparison.InvariantCultureIgnoreCase));

                foreach (var entry in entries)
                {
                    trigger.Reply(entry.ToString());
                }
            }
            else
                trigger.Reply(BenchmarkManager.Instance.GenerateReport().HtmlEntities());
        }
    }

    public class BenchmarkEnableCommand : SubCommand
    {
        public BenchmarkEnableCommand()
        {
            Aliases = new[] { "enable", "on" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.Enable = true;
        }
    }

    public class BenchmarkDisableCommand : SubCommand
    {
        public BenchmarkDisableCommand()
        {
            Aliases = new[] { "disable", "off" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.Enable = false;
        }
    }
    
    public class BenchmarkLimitCommand : SubCommand
    {
        public BenchmarkLimitCommand()
        {
            Aliases = new[] { "limit" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<int>("limit", "l", "Entries limit");
        }

        public override void Execute(TriggerBase trigger)
        {
            BenchmarkManager.EntriesLimit = trigger.Get<int>("limit");
        }
    }

    public class BenchmarkIOInfoCommand : SubCommand
    {
         public BenchmarkIOInfoCommand()
        {
            Aliases = new[] { "ioinfo" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.Reply(ServerBase.InstanceAsBase.IOTaskPool.GetDebugInformations());
        }
    }

    public class BenchmarkPingIOCommand : SubCommand
    {
        public BenchmarkPingIOCommand()
        {
            Aliases = new[] { "pingio" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<int>("times", "t", "Pings count", 20);
        }

        public override void Execute(TriggerBase trigger)
        {
            int i = 0;
            int count = trigger.Get<int>("times");
            PingIO(trigger, 0, trigger.Get<int>("times"), new List<DateTime>() { DateTime.Now});
        }

        private void PingIO(TriggerBase trigger, int i, int count, List<DateTime> dates)
        {
            if (i >= count)
            {
                double sum = 0;
                for (int j = 1; j < dates.Count; j++)
                {
                    sum += (dates[j] - dates[j - 1]).TotalMilliseconds;
                    trigger.Reply("{0:F1} ms", (dates[j] - dates[j - 1]).TotalMilliseconds);
                }

                trigger.Reply("Average : {0} ms", sum/(dates.Count - 1));
            }
            else
            {
                ServerBase.InstanceAsBase.IOTaskPool.AddMessage(() =>
                {
                    dates.Add(DateTime.Now);
                    PingIO(trigger, i + 1, count, dates);
                });
            }
        }
    }

    public class BenchmarkStressNetworkCommand : SubCommand
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static ClientManager server;

        public BenchmarkStressNetworkCommand()
        {
            Aliases = new[] { "stressnet" };
            RequiredRole = RoleEnum.Administrator;
            ParentCommandType = typeof(BenchmarkCommands);
            AddParameter<int>("clients", "c", "Clients count", 1000);
            AddParameter<int>("sends", "s", "Sends count", 100);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (server == null)
            {
                server = new ClientManager();
                server.Initialize(CreateClient);
                server.Start("127.0.0.1", 555);
            }

            try
            {
                int count = trigger.Get<int>("clients");
                var clients = new List<FakeClientTest>();
                for (int i = 0; i < count; i++)
                {
                    var client = new FakeClientTest();
                    client.Connect();
                    clients.Add(client);
                }

                int count2 = trigger.Get<int>("sends");
                for (int i = 0; i < count2; i++)
                {
                    foreach (var client in clients)
                    {
                        client.SendRandomData();
                    }
                }
            }
            finally
            {
                //server.Close();
            }
        }

        private BaseClient CreateClient(Socket socket)
        {
            return new ServerClientTest(socket);
        }

        public class ServerClientTest : BaseClient
        {
            public ServerClientTest(Socket socket)
                : base(socket)
            {
                CanReceive = true;
            }

            protected override void OnMessageReceived(Message message)
            {
                var msg = message as RawDataMessageFixed;
                Console.WriteLine("RECV {0} (x{1})", msg.content[0], msg.content.Length);
            }
        }

        public class FakeClientTest
        {
            public static Random random = new Random();

            private Socket m_socket;

            public FakeClientTest()
            {
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            public void Connect()
            {
                m_socket.Connect("127.0.0.1", 555);
            }

            public void SendRandomData()
            {
                int size = random.Next(100, 10000);
                var value = (byte)random.Next(0, 256);
                var bytes = Enumerable.Repeat(value, size).ToArray();

                var msg = new RawDataMessageFixed(bytes);
                var writer = new BigEndianWriter();
                msg.Pack(writer);
                var e = new SocketAsyncEventArgs();
                var buffer = writer.Data;
                e.SetBuffer(buffer, 0, buffer.Length);
                m_socket.SendAsync(e);

                Console.WriteLine("SEND {0} (x{1})", value, size);
            }
        }
    }
}