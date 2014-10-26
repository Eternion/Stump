using Stump.DofusProtocol.Enums;
using Stump.Server.AuthServer.Managers;
using Stump.Server.BaseServer.Commands;

namespace Stump.Server.AuthServer.Commands
{
    public class ServerCommands : SubCommandContainer
    {
        public ServerCommands()
        {
            Aliases = new[] {"server", "serv"};
            RequiredRole = RoleEnum.Administrator;
            Description = "Provides many commands to manage servers";
        }
    }

    public class ServerStatusCommand : SubCommand
    {
        public ServerStatusCommand()
        {
            Aliases = new[] {"status", "state"};
            ParentCommandType = typeof (ServerCommands);
            RequiredRole = RoleEnum.Administrator;
            Description = "Change status of a server.";

            AddParameter<int>("serverid", "server", "Id of the server");
            AddParameter<int>("status", "state", "StatusId to set");
        }

        public override void Execute(TriggerBase trigger)
        {
            var serverId = trigger.Get<int>("serverid");
            var stateId = trigger.Get<int>("status");

            var world = WorldServerManager.Instance.GetServerById(serverId);
            if (world == null)
                return;

            WorldServerManager.Instance.ChangeWorldState(world, (ServerStatusEnum) stateId, false);
        }
    }
}
