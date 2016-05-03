using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class SingleRuneMagicCraftDialog : RuneMagicCraftDialog
    {
        public SingleRuneMagicCraftDialog(Character character, InteractiveObject interactive, Skill skill)
            : base(interactive, skill, character.Jobs[skill.SkillTemplate.ParentJobId])
        {
            Crafter = new Crafter(this, character);
            Clients = new WorldClientCollection(character.Client);
        }

        private Character Character => Crafter.Character;
        public override Trader FirstTrader => Crafter;
        public override Trader SecondTrader => Crafter;

        public override void Close()
        {
            Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
        }

        public override void Open()
        {
            base.Open();

            InventoryHandler.SendExchangeStartOkCraftWithInformationMessage(Character.Client, Skill);

            Character.SetDialoger(Crafter);
            Crafter.ItemMoved += OnItemMoved;
            Crafter.ReadyStatusChanged += OnReady;
        }

        private void OnReady(Trader trader, bool isready)
        {
            if (isready)
            {
                foreach (var result in ApplyRune())
                {
                    InventoryHandler.SendExchangeCraftResultMagicWithObjectDescMessage(Character.Client, result.First, ItemToImprove.PlayerItem, result.Second);
                }

                trader.ToggleReady(false);
            }
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            if (!modified && item.Stack > 0)
                InventoryHandler.SendExchangeObjectAddedMessage(Clients, false, item);

            else if (item.Stack <= 0)
                InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);

            else
                InventoryHandler.SendExchangeObjectModifiedMessage(Character.Client, false, item);
        }
    }
}