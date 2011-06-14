
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Npcs.StartActions
{
    public class ShopAction : NpcStartAction
    {
        public List<ItemToSellInNpcShop> ItemsToSell;
        public uint TokenId;
        private List<ObjectItemToSellInNpcShop> m_itemsToSell;

        private ShopAction()
        {
        }

        public ShopAction(int npcId, uint tokenId, List<ItemToSellInNpcShop> itemsToSell)
        {
            NpcId = npcId;
            TokenId = tokenId;
            ItemsToSell = itemsToSell;
        }

        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_BUY_SELL; }
        }

        public override void Execute(NpcSpawn npc, Character executer)
        {
            if (m_itemsToSell == null || m_itemsToSell.Count <= 0)
                m_itemsToSell =
                    ItemsToSell.Select(
                        entry =>
                        entry.ToNetworkObjectItem()).ToList();

            var dialog = new NpcShopDialog(npc, executer, ItemsToSell);

            executer.Dialoger = dialog.Dialoger;

            InventoryHandler.SendExchangeStartOkNpcShopMessage(executer.Client, (int) npc.Id, TokenId, m_itemsToSell);
        }
    }
}