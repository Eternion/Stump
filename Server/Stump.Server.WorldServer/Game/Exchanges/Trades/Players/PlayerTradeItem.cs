#region License GNU GPL

// PlayerTradeItem.cs
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
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Exchanges.Trades.Players
{
    public class PlayerTradeItem : TradeItem
    {
        private readonly BasePlayerItem m_item;
        private uint m_stack;

        public PlayerTradeItem(Exchanger trader, BasePlayerItem item, uint stack)
        {
            Trader = trader;
            m_item = item;
            m_stack = stack;
        }

        public Exchanger Trader
        {
            get;
        }

        public Character Owner => m_item.Owner;

        public BasePlayerItem PlayerItem => m_item;

        public override int Guid
        {
            get { return m_item.Guid; }
        }

        public override ItemTemplate Template
        {
            get { return m_item.Template; }
        }

        public override uint Stack
        {
            get { return m_stack; }
            set { m_stack = value; }
        }

        public override List<EffectBase> Effects
        {
            get { return m_item.Effects; }
        }

        public override CharacterInventoryPositionEnum Position
        {
            get { return CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED; }
        }
    }
}