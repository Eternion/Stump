using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Items;

namespace ArkalysPlugin.Commands
{
    public class GiveMapCommand : InGameCommand
    {
        public GiveMapCommand()
        {
            Aliases = new[] { "givemap" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Give an item to the whole map";
            AddParameter<int>("amount");
        }

        public override void Execute(GameTrigger trigger)
        {
            var character = trigger.Character;
            var players = character.Map.GetAllCharacters().ToArray();
            var itemToken = ItemManager.Instance.TryGetTemplate(20867);
            var amount = trigger.Get<int>("amount");

            var item = character.Inventory.TryGetItem(itemToken);

            if (item.Stack < amount)
            {
                trigger.ReplyError("You doesn't have enough items !!");
                return;
            }

            character.Inventory.RemoveItem(item, amount);

            var amountPerPlayer = (int)Math.Ceiling((double)amount/players.Count());

            foreach (var player in players.Where(player => player != character))
            {
                player.Inventory.AddItem(itemToken, amountPerPlayer);
            }

            trigger.Reply("Successfully add {0} items to {1} players !", amount, players.Count());
        }
    }
}
