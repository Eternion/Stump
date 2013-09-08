using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
namespace Stump.Server.WorldServer.Commands.Commands
{
    class GuildCommand : SubCommandContainer
    {
        public GuildCommand()
        {
            Aliases = new[] { "guild" };
            RequiredRole = RoleEnum.GameMaster;
            Description = "Provides many commands to manage guilds";
        }
    }

    public class GuildCreateCommand : SubCommand
    {
        public GuildCreateCommand()
        {
            Aliases = new[] { "create" };
            RequiredRole = RoleEnum.GameMaster;
            ParentCommand = typeof(GuildCommand);
        }

        public override void Execute(TriggerBase trigger)
        {
            trigger.GetSource().Send(new GuildCreationStartedMessage());
        }
    }
}
