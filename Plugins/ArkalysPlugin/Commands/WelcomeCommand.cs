using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;

namespace ArkalysPlugin.Commands
{
    public class WelcomeCommand : InGameCommand
    {
        public WelcomeCommand()
        {
            Aliases = new [] {"welcome"};
            RequiredRole = RoleEnum.Player;
            Description = "Affiche le message de bienvenue";
        }

        public override void Execute(GameTrigger trigger)
        {
            WelcomeMessage.ShowWelcomeMessage(trigger.Character);
        }
    }
}