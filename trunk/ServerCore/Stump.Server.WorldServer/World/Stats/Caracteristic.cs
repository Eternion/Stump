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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Chat;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.Pathfinding;
using Stump.Server.WorldServer.Handlers;

namespace Stump.Server.WorldServer.Entities
{
    public class Caracteristic
    {

        public Caracteristic(Stats stats, CaracteristicEnum name, uint baseValue)
        {
            m_stats = stats;
            Name = name;
            BaseValue = baseValue;
        }

        private readonly Stats m_stats;
        public readonly CaracteristicEnum Name;

        public uint BaseValue
        {
            get;
            set;
        }



    }
}