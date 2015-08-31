using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;

namespace FakeClients
{
    public class FakeClientCommands : SubCommandContainer
    {
        public FakeClientCommands()
        {
            Aliases = new[] {"fakeclient"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Simulate clients on the server";
        }
    }

    public class ConnectClientsCommand : SubCommand
    {
        public ConnectClientsCommand()
        {
            Aliases = new[] {"connect"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Connect some clients on the server";
            ParentCommandType = typeof (FakeClientCommands);
            AddParameter("count", "c", "Number of clients to connect", 1);
        }

        public override void Execute(TriggerBase trigger)
        {
            var count = trigger.Get<int>("count");

            foreach (var client in FakeClientManager.Instance.AddAndConnectClients(count))
                client.CreatorTrigger = trigger;

            trigger.Reply("{0} fake clients connecting ...", count);
        }
    }
}