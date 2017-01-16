using System;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Commands.Commands.Patterns;
using Stump.Server.WorldServer.Commands.Trigger;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.BaseServer.Commands;

namespace GameplayPlugin.Commands
{
    public class GiveCommand : SubCommandContainer
    {
        public GiveCommand()
        {
            Aliases = new[] { "give" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Give Commands";
        }
    }

    public class MapGiveCommand : InGameSubCommand
    {
        public MapGiveCommand()
        {
            Aliases = new[] { "map" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Give item event(20837) to the whole map";
            ParentCommandType = typeof(GiveCommand);
            AddParameter<int>("amount");
        }

        public override void Execute(GameTrigger trigger)
        {
            var character = trigger.Character;
            var players = character.Map.GetAllCharacters().ToArray();
            var itemToken = ItemManager.Instance.TryGetTemplate(20837);
            var amount = trigger.Get<int>("amount");

            var item = character.Inventory.TryGetItem(itemToken);

            if (item.Stack < amount)
            {
                trigger.ReplyError("You doesn't have enough items !!");
                return;
            }

            character.Inventory.RemoveItem(item, amount);

            var amountPerPlayer = (int)Math.Ceiling((double)amount/players.Count());

            foreach (var player in players)
            {
                player.Inventory.AddItem(itemToken, amountPerPlayer);
            }

            trigger.Reply("Successfully add {0} items to {1} players !", amount, players.Count());
        }
    }

    public class PlayerGiveCommand : TargetSubCommand
    {
        public PlayerGiveCommand()
        {
            Aliases = new[] { "player" };
            RequiredRole = RoleEnum.Moderator;
            Description = "Give item event(20837) to specified player";
            ParentCommandType = typeof(GiveCommand);
            AddTargetParameter(true, "Character who will receive the item");
            AddParameter<int>("amount");
        }

        public override void Execute(TriggerBase trigger)
        {
            var character = (trigger as GameTrigger).Character;
            var target = GetTargets(trigger).FirstOrDefault(x => x != character);

            var itemToken = ItemManager.Instance.TryGetTemplate(20837);
            var amount = trigger.Get<int>("amount");
            var item = character.Inventory.TryGetItem(itemToken);

            if (target == null)
            {
                trigger.ReplyError("Please specify target");
                return;
            }

            if (item.Stack < amount)
            {
                trigger.ReplyError("You doesn't have enough items !");
                return;
            }

            character.Inventory.RemoveItem(item, amount);
            target.Inventory.AddItem(itemToken, amount);

            trigger.Reply("Successfully add {0} items to {1} player !", amount, target);
        }
    }
}
