#region License GNU GPL
// NpcTradeItem.cs
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
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Exchanges.Items
{
    public class NpcTradeItem : TradeItem
    {
        private readonly int m_guid;
        private readonly ItemTemplate m_template;
        private uint m_stack;

        public NpcTradeItem(int guid, ItemTemplate template, uint stack)
        {
            m_guid = guid;
            m_template = template;
            m_stack = stack;
        }

        public override int Guid
        {
            get { return m_guid; }
        }

        public override ItemTemplate Template
        {
            get { return m_template; }
        }

        public override uint Stack
        {
            get { return m_stack; }
            set { m_stack = value; }
        }

        public override List<EffectBase> Effects
        {
            get { return m_template.Effects; }
        }

        public override CharacterInventoryPositionEnum Position
        {
            get { return CharacterInventoryPositionEnum.INVENTORY_POSITION_NOT_EQUIPED; }
        }
    }
}