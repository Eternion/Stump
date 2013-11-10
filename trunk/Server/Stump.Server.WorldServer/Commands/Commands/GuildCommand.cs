using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;

namespace Stump.Server.WorldServer.Commands.Commands
{
    internal class GuildCommand : SubCommandContainer
    {
        public GuildCommand()
        {
            Aliases = new[] {"guild"};
            RequiredRole = RoleEnum.GameMaster;
            Description = "Provides many commands to manage guilds";
        }
    }

    public class GuildCreateCommand : SubCommand
    {
        public GuildCreateCommand()
        {
            Aliases = new[] {"create"};
            RequiredRole = RoleEnum.GameMaster;
            ParentCommand = typeof (GuildCommand);
        }

        public override void Execute(TriggerBase trigger)
        {
            if (trigger is GameTrigger)
            {
                var panel = new GuildCreationPanel((trigger as GameTrigger).Character);
                panel.Open();
            }
            else
                trigger.ReplyError("Only in game");
        }
    }
}