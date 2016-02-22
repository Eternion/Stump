using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Context;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class MultiCraftDialog : CraftDialog
    {
        public MultiCraftDialog(Character crafter, Character customer, InteractiveObject interactive, Skill skill)
            : base(interactive, skill, crafter.Jobs[skill.SkillTemplate.ParentJobId])
        {
            Crafter = new Crafter(this, crafter);
            Receiver = new CraftCustomer(this, customer);
            Clients = new WorldClientCollection(new []{crafter.Client, customer.Client});
        }

        public override void Close()
        {
            Crafter.Character.ResetDialog();
            Receiver.Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Clients, DialogType, false);
        }
        
        public void Open()
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

                FirstTrader.ToggleReady(false);
                SecondTrader.ToggleReady(false);

                return;
            }

            if (Crafter.ReadyToApply && Receiver.ReadyToApply)
            {
                Craft();
                
                Crafter.Character.Inventory.AddKamas(-Receiver.Character.Inventory.SubKamas((int)Receiver.Kamas));
                Receiver.SetKamas(0);

                ContextRoleplayHandler.SendJobExperienceOtherPlayerUpdateMessage(Receiver.Character.Client, Crafter.Character, Job);

                FirstTrader.ToggleReady(false);
                SecondTrader.ToggleReady(false);
            }
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
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


            FirstTrader.ToggleReady(false);
            SecondTrader.ToggleReady(false);
        }

        public override Trader FirstTrader => Crafter;
        public override Trader SecondTrader => Receiver;


    }
}