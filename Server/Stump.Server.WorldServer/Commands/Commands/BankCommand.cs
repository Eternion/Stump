using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Commands;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Handlers.Inventory;

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
            Description = "Open target bank";
            RequiredRole = RoleEnum.GameMaster;
            ParentCommandType = typeof (BankCommands);
            AddParameter("target", "t", "Bank Owner nam",
                    converter: ParametersConverter.CharacterConverter, isOptional: true);
        }

        public override void Execute(GameTrigger trigger)
        {
            if (trigger.IsArgumentDefined("target"))
            {
                var target = trigger.Get<Character>("target");
                var source = trigger.Character.Client;

                if (!target.Bank.IsLoaded)
                {
                    WorldServer.Instance.IOTaskPool.AddMessage(() =>
                    {
                        target.Bank.LoadRecord();

                        InventoryHandler.SendExchangeStartedMessage(source, ExchangeTypeEnum.STORAGE);
                        InventoryHandler.SendStorageInventoryContentMessage(source, target.Bank);
                    });
                }
                else
                {
                    InventoryHandler.SendExchangeStartedMessage(source, ExchangeTypeEnum.STORAGE);
                    InventoryHandler.SendStorageInventoryContentMessage(source, target.Bank);
                }
            }
            else
            {
                trigger.ReplyError("Please define a target");
            }
        }
    }
}