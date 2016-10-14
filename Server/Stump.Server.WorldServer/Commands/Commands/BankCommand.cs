using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Bank;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Commands.Commands
{
    public class BankCommands : SubCommandContainer
    {
        public BankCommands()
        {
            Aliases = new[] { "bank" };
            Description = "Gives commands to manage bank";
            RequiredRole = RoleEnum.GameMaster;
        }
    }

    public class BankOpenCommand : TargetSubCommand
    {
        public BankOpenCommand()
        {
            Aliases = new[] { "open" };
            Description = "Open target bank";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof(BankCommands);
            AddTargetParameter();
        }
        
        public override void Execute(TriggerBase trigger)
        {
            var target = GetTarget(trigger);
            var source = (trigger as GameTrigger).Character;

            if (target != source)
            {
                InventoryHandler.SendExchangeStartedMessage(source.Client, ExchangeTypeEnum.STORAGE);
                InventoryHandler.SendStorageInventoryContentMessage(source.Client, target.Bank);

            }
            else
            {
                var dialog = new BankDialog(target);
                dialog.Open();
            }
        }
    }
}