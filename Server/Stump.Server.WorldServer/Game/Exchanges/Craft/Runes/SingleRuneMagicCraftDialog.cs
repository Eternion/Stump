using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class SingleRuneMagicCraftDialog : RuneMagicCraftDialog
    {
        public SingleRuneMagicCraftDialog(Character character, InteractiveObject interactive, Skill skill)
            : base(interactive, skill, character.Jobs[skill.SkillTemplate.ParentJobId])
        {
            Crafter = new RuneCrafter(this, character);
            Clients = new WorldClientCollection(character.Client);
        }

        private Character Character => Crafter.Character;
        public override Trader FirstTrader => Crafter;
        public override Trader SecondTrader => Crafter;

        public override void Close()
        {
            base.Close();

            foreach (var item in Crafter.Items.OfType<PlayerTradeItem>().ToArray())
            {
                Crafter.MoveItemToInventory(item, 0);
            }

            Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
        }

        public override void Open()
        {
            base.Open();

            InventoryHandler.SendExchangeStartOkCraftWithInformationMessage(Character.Client, Skill);

            Character.SetDialoger(Crafter);
            Crafter.ReadyStatusChanged += OnReady;
        }

        private void OnReady(Trader trader, bool isready)
        {
            if (isready)
            {
                ApplyAllRunes();

                trader.ToggleReady(false);
            }
        }

        protected override void OnRuneApplied(CraftResultEnum result, MagicPoolStatus poolStatus)
        {
            InventoryHandler.SendExchangeCraftResultMagicWithObjectDescMessage(Character.Client, result, ItemToImprove.PlayerItem, ItemEffects, poolStatus);

            InventoryHandler.SendExchangeCraftInformationObjectMessage(Character.Client, ItemToImprove.PlayerItem, ItemToImprove.Owner, (ExchangeCraftResultEnum)result);
            ItemToImprove.Owner.Inventory.RefreshItem(ItemToImprove.PlayerItem);
        }

        protected override void OnAutoCraftStopped(ExchangeReplayStopReasonEnum reason)
        {
            InventoryHandler.SendExchangeItemAutoCraftStopedMessage(Character.Client, reason);
        }

        protected override void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            base.OnItemMoved(trader, item, modified, difference);

            if (!modified && item.Stack > 0)
                InventoryHandler.SendExchangeObjectAddedMessage(Clients, false, item);

            else if (item.Stack <= 0)
                InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);

            else
                InventoryHandler.SendExchangeObjectModifiedMessage(Character.Client, false, item);
        }
    }
}