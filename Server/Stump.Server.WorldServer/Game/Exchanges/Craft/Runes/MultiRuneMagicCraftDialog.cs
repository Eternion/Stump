using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class MultiRuneMagicCraftDialog : RuneMagicCraftDialog
    {
        public MultiRuneMagicCraftDialog(Character crafter, Character customer, InteractiveObject interactive, Skill skill)
            : base(interactive, skill, crafter.Jobs[skill.SkillTemplate.ParentJobId])
        {
            Crafter = new MultiRuneCrafter(this, crafter);
            Receiver = new CraftCustomer(this, customer);
            Clients = new WorldClientCollection(new[] { crafter.Client, customer.Client });
            Amount = 1;
        }
        
        public override Trader FirstTrader => Crafter;
        public override Trader SecondTrader => Receiver;

        public override void Close()
        {
            Crafter.Character.ResetDialog();
            Receiver.Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Clients, DialogType, false);
        }

        public override void Open()
        {
            InventoryHandler.SendExchangeStartOkMulticraftCrafterMessage(Crafter.Character.Client, Skill.SkillTemplate);
            InventoryHandler.SendExchangeStartOkMulticraftCustomerMessage(Receiver.Character.Client, Skill.SkillTemplate, Job);

            ContextRoleplayHandler.SendJobExperienceOtherPlayerUpdateMessage(Receiver.Character.Client, Crafter.Character, Job);

            Crafter.Character.SetDialoger(Crafter);
            Receiver.Character.SetDialoger(Receiver);

            Crafter.ItemMoved += OnItemMoved;
            Receiver.ItemMoved += OnItemMoved;

            Crafter.ReadyStatusChanged += OnReady;
            Receiver.ReadyStatusChanged += OnReady;

            Receiver.KamasChanged += OnKamasChanged;
        }

        public void MoveItemFromBag(int id, int quantity)
        {
            if (!Receiver.ReadyToApply)
                return;

            var item = Receiver.Character.Inventory[id];

            if (item != null)
            {
                Crafter.MoveItemFromBag(item, Receiver, quantity);
                Receiver.MoveItem(id, -quantity);
            }
        }

        private void OnKamasChanged(Trader trader, uint kamasamount)
        {
            InventoryHandler.SendExchangeCraftPaymentModifiedMessage(Crafter.Character.Client, (int)kamasamount);
            InventoryHandler.SendExchangeCraftPaymentModifiedMessage(Receiver.Character.Client, (int)kamasamount);
        }

        private void OnReady(Trader trader, bool isready)
        {
            InventoryHandler.SendExchangeIsReadyMessage(Crafter.Character.Client,
                                                        trader, isready);
            InventoryHandler.SendExchangeIsReadyMessage(Receiver.Character.Client,
                                                        trader, isready);

            if (Receiver.Kamas > Receiver.Character.Inventory.Kamas)
            {
                InventoryHandler.SendExchangeCraftResultMessage(Clients, ExchangeCraftResultEnum.CRAFT_FAILED);
                return;
            }

            if (Crafter.ReadyToApply && Receiver.ReadyToApply)
            {
                ApplyAllRunes();

                if (Receiver.Kamas > 0)
                {
                    Crafter.Character.Inventory.AddKamas(-Receiver.Character.Inventory.SubKamas((int) Receiver.Kamas));
                    Receiver.SetKamas(0);
                }

                ContextRoleplayHandler.SendJobExperienceOtherPlayerUpdateMessage(Receiver.Character.Client, Crafter.Character, Job);
            }

            else if (trader == Receiver && !isready) // stop the trade
            {
                if (Rune != null && Rune.Trader == Receiver)
                    Rune.Trader.MoveItem(Rune.Guid, (int) -Rune.Stack);

                var itemToImprove = ItemToImprove;
                Crafter.MoveItem(itemToImprove.Guid, (int) -itemToImprove.Stack);
                Receiver.MoveItem(itemToImprove.Guid, (int) itemToImprove.Stack);
            }
        }

        protected override void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            if (trader == Crafter)
            {
                base.OnItemMoved(trader, item, modified, difference);

                if (!modified && item.Stack > 0)
                {
                    InventoryHandler.SendExchangeObjectAddedMessage(Crafter.Character.Client, Crafter != trader, item);
                    InventoryHandler.SendExchangeObjectAddedMessage(Receiver.Character.Client, Receiver != trader, item);
                }
                else if (item.Stack <= 0)
                {
                    InventoryHandler.SendExchangeObjectRemovedMessage(Crafter.Character.Client, Crafter != trader, item.Guid);
                    InventoryHandler.SendExchangeObjectRemovedMessage(Receiver.Character.Client, Receiver != trader, item.Guid);
                }
                else
                {
                    InventoryHandler.SendExchangeObjectModifiedMessage(Crafter.Character.Client, Crafter != trader, item);
                    InventoryHandler.SendExchangeObjectModifiedMessage(Receiver.Character.Client, Receiver != trader, item);
                }
            }
            else
            {
                if (!modified && item.Stack > 0)
                {
                    InventoryHandler.SendExchangeObjectPutInBagMessage(Crafter.Character.Client, Crafter != trader, item);
                    InventoryHandler.SendExchangeObjectPutInBagMessage(Receiver.Character.Client, Receiver != trader, item);
                }
                else if (item.Stack <= 0)
                {
                    InventoryHandler.SendExchangeObjectRemovedFromBagMessage(Crafter.Character.Client, Crafter != trader, item.Guid);
                    InventoryHandler.SendExchangeObjectRemovedFromBagMessage(Receiver.Character.Client, Receiver != trader, item.Guid);
                }
                else
                {
                    InventoryHandler.SendExchangeObjectModifiedInBagMessage(Crafter.Character.Client, Crafter != trader, item);
                    InventoryHandler.SendExchangeObjectModifiedInBagMessage(Receiver.Character.Client, Receiver != trader, item);
                }
            }
        }

        protected override void OnRuneApplied(CraftResultEnum result, MagicPoolStatus poolStatus)
        {
            InventoryHandler.SendExchangeCraftResultMagicWithObjectDescMessage(Crafter.Character.Client, result, ItemToImprove.PlayerItem, poolStatus);
            InventoryHandler.SendExchangeCraftResultMagicWithObjectDescMessage(Receiver.Character.Client, result, ItemToImprove.PlayerItem, poolStatus);

            InventoryHandler.SendExchangeCraftInformationObjectMessage(Crafter.Character.Client, ItemToImprove.PlayerItem, ItemToImprove.Owner, (ExchangeCraftResultEnum)result);
            InventoryHandler.SendExchangeCraftInformationObjectMessage(Receiver.Character.Client, ItemToImprove.PlayerItem, ItemToImprove.Owner, (ExchangeCraftResultEnum)result);

            ItemToImprove.Owner.Inventory.RefreshItem(ItemToImprove.PlayerItem);
        }

        protected override void OnAutoCraftStopped(ExchangeReplayStopReasonEnum reason)
        {
            InventoryHandler.SendExchangeItemAutoCraftStopedMessage(Receiver.Character.Client, reason);
            InventoryHandler.SendExchangeItemAutoCraftStopedMessage(Crafter.Character.Client, reason);
        }

    }
}