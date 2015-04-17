using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Trigger;

namespace Stump.Plugins.DebugPlugin.Commands
{
    public class StartCommand : CommandBase
    {
        public StartCommand()
        {
            Aliases = new[] { "start" };
            RequiredRole = RoleEnum.Player;
            Description = "Teleport to the start map";
        }

        public override void Execute(TriggerBase trigger)
        {
            if (!(trigger is GameTrigger))
                return;

            var character = ( (GameTrigger) trigger ).Character;

            character.Teleport(character.Breed.GetStartPosition());

            trigger.Reply("Teleported to the start map");
        }
    }
}