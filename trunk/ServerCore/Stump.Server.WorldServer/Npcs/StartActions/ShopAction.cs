// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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
            if (m_itemsToSell.Count <= 0)
                m_itemsToSell =
                    ItemsToSell.Select(
                        entry =>
                        entry.ToNetworkObjectItem()).ToList();

            var dialog = new NpcShopDialog(npc, executer, ItemsToSell);

            executer.Dialoger = dialog.Dialoger;

            InventoryHandler.SendExchangeStartOkNpcShopMessage(executer.Client, NpcId, TokenId, m_itemsToSell);
        }
    }
}