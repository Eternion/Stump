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
using ProtoBuf;
using Stump.DofusProtocol.Classes;

namespace Stump.Server.DataProvider.Data.Mount
{
    [Serializable,ProtoContract]
    public class MountTemplate
    {
        [ProtoMember(1)]
        public uint MountId { get; set; }

        [ProtoMember(2)]
        public string LookStr { get; set; }

        public EntityLook Look { get; set; }

        [ProtoMember(3)]
        public string PodFormula { get; set; }

        [ProtoMember(4)]
        public string EnergyFormula { get; set; }

        [ProtoMember(5)]
        public uint MaxMaturity { get; set; }

        [ProtoMember(6)]
        public string GestationDuration { get; set; }

        [ProtoMember(7)]
        public string LearningMalus { get; set; }

        public MountTemplate()
        {
        }
    }
}