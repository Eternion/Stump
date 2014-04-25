using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Exchanges.Bank;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class BankCommands : SubCommandContainer
    {
        public BankCommands()
        {
            Aliases = new[] {"bank"};
            Description = "Gives commands to manage bank";
            RequiredRole = RoleEnum.GameMaster;
        }
    }

    public class BankOpenCommand : InGameSubCommand
    {
        public BankOpenCommand()
        {
            Aliases = new[] {"open"};
            Description = "Open his own bank";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof (BankCommands);
        }

        public override void Execute(GameTrigger trigger)
        {
            var dialog = new BankDialog(trigger.Character);
            dialog.Open();
        }
    }
}