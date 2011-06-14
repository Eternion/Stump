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
using System;
using System.Collections.Generic;
using Stump.Server.WorldServer.Dialog;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Characters;

namespace Stump.Server.WorldServer.Exchange
{
    public class Trader : Dialoger
    {
        public Trader(Character trader, PlayerTrade trade)
            : base(trader, trade)
        {
            Items = new List<Item>();
        }

        public PlayerTrade PlayerTrade
        {
            get { return Dialog as PlayerTrade; }
        }

        public List<Item> Items
        {
            get;
            set;
        }

        public uint Kamas
        {
            get;
            set;
        }

        public bool Ready
        {
            get;
            set;
        }
    }
}