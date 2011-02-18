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
using Stump.BaseCore.Framework.Extensions;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Effects;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Handlers;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.World.Storages
{

    public class SellBag
    {

        public SellBag(SellBagRecord record, Character owner)
        {
            Record =record;
            Owner = owner;
            Capactity = record.Capacity;
            m_items = record.PricedItems.Select(i => new Item(i)).ToDictionary(entry => entry.Guid);  
        }
        
        public readonly SellBagRecord Record;

        public Character Owner
        {
            get;
            private set;
        }

        private Dictionary<long, PricedItem> m_items = new Dictionary<long, PricedItem>();
        public IEnumerable<PricedItem> Items
        {
            get { return m_items.Values; }
        }

        public PricedItem this[long guid]
        {
            get
            {
                Item item;
                m_items.TryGetValue(guid, out item);
                return item;
            }
        }

        public uint Capactity
        {
            get;
            set;
        }

    }
}