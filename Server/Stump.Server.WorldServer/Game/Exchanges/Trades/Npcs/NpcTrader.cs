#region License GNU GPL
// NpcTrader.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Npcs
{
    public class NpcTrader : Trader
    {
        private readonly UniqueIdProvider m_idProvider = new UniqueIdProvider();

        public NpcTrader(Npc npc, ITrade trade)
            : base(trade)
        {
            Npc = npc;
        }

        public Npc Npc
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return Npc.Id; }
        }

        public void Clear()
        {
            foreach (var item in Items)
            {
                OnItemMoved(item, true, 0);
            }

            ClearItems();
        }

        public override bool MoveItem(int id, int quantity)
        {
            var template = ItemManager.Instance.TryGetTemplate(id);

            if (template == null || quantity <= 0)
                return false;

            AddItem(template, (uint)quantity);

            return true;
        }

        public void AddItem(ItemTemplate template, uint amount)
        {
            var item = Items.FirstOrDefault(x => x.Template == template);

            if (item != null)
            {
                item.Stack += amount;
                OnItemMoved(item, true, (int)amount);
            }
            else
            {
                item = new NpcTradeItem(m_idProvider.Pop(), template, amount);
                AddItem(item);
                OnItemMoved(item, false, (int)amount);
            }
        }
        
        public void AddItem(ItemTemplate template, uint amount, List<EffectBase> effects)
        {
            var item = Items.FirstOrDefault(x => x.Template == template);

            if (item != null)
            {
                item.Stack += amount;
                OnItemMoved(item, true, (int)amount);
            }
            else
            {
                item = new NpcTradeItem(m_idProvider.Pop(), template, amount, effects);
                AddItem(item);
                OnItemMoved(item, false, (int)amount);
            }
        }

        public bool RemoveItem(ItemTemplate template, uint amount)
        {
            var item = Items.FirstOrDefault(x => x.Template == template);

            if (item == null)
                return false;

            var amountRemoved = amount;
            if (item.Stack - amount <= 0)
            {
                RemoveItem(item);
                amountRemoved = item.Stack;
            }
            else
            {
                item.Stack -= amount;
            }

            OnItemMoved(item, item.Stack > 0, -(int)amountRemoved);
            return true;
        }


    }
}