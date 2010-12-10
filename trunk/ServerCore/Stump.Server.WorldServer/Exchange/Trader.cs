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
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;

namespace Stump.Server.WorldServer.Exchange
{
    public class Trader : IDialoger
    {
        public Trader(Entity trader)
        {
            Entity = trader;
            Items = new List<Item>();
        }

        public uint Kamas
        {
            get;
            set;
        }

        public List<Item> Items
        {
            get;
            set;
        }

        public bool Ready
        {
            get;
            set;
        }

        public Trade Trade
        {
            get;
            set;
        }

        #region IDialoger Members

        /// <summary>
        ///   Current entity
        /// </summary>
        /// <remarks>
        ///   Entity can be a player or a npc
        /// </remarks>
        public Entity Entity
        {
            get;
            set;
        }

        #endregion
    }
}