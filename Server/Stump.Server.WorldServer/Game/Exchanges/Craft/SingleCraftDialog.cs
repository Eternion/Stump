using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class SingleCraftDialog : CraftDialog
    {
        public SingleCraftDialog(Character character, InteractiveObject interactive, Skill skill)
             : base(interactive, skill, character.Jobs[skill.SkillTemplate.ParentJobId])
        {
            Crafter = new Crafter(this, character);
            Receiver = Crafter;
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

        public void Open()
        {
            InventoryHandler.SendExchangeStartOkCraftWithInformationMessage(Character.Client, Skill);

            Character.SetDialoger(Crafter);
            Crafter.ItemMoved += OnItemMoved;
            Crafter.ReadyStatusChanged += OnReady;
        }

        private void OnReady(Trader trader, bool isready)
        {
            if (isready)
            {
                Craft();

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

        protected override IEnumerable<PlayerTradeItem> GetIngredients()
        {
            return Crafter.Items.OfType<PlayerTradeItem>();
        }
    }
}