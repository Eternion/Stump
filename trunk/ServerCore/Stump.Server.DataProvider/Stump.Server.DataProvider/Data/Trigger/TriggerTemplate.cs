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
using ProtoBuf;

namespace Stump.Server.DataProvider.Data.Trigger
{
    [ProtoContract]
    public class TriggerTemplate
    {
        [ProtoMember(1)]
        public uint MapId { get; set; }     

        [ProtoMember(2)]
        public ushort CellId { get; set; }

        [ProtoMember(3)]
        public ushort Action { get; set; }

        [ProtoMember(4)]
        public ushort Condition { get; set; }
    }
}