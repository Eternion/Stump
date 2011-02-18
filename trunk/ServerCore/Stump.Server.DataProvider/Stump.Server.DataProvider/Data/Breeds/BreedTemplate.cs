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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Breeds
{
    [ProtoContract]
    public class BreedTemplate
    {
        [ProtoMember(1)]
        public PlayableBreedEnum Id { get; set; }

        public EntityLook MaleLook { get; set; }

        public List<uint> MaleColors { get; set; }

        public EntityLook FemaleLook { get; set; }

        public List<uint> FemaleColors { get; set; }

        public List<List<uint>> StatsPointsForStrength { get; set; }

        public List<List<uint>> StatsPointsForIntelligence { get; set; }

        public List<List<uint>> StatsPointsForChance { get; set; }

        public List<List<uint>> StatsPointsForAgility { get; set; }

        public List<List<uint>> StatsPointsForVitality { get; set; }

        public List<List<uint>> StatsPointsForWisdom { get; set; }

        [ProtoMember(2)]
        public List<LearnableSpell> LearnableSpells { get; set; }

        [ProtoMember(3)]
        public uint StartHealthPoint { get; set; }

        [ProtoMember(4)]
        public int StartMap { get; set; }

        [ProtoMember(5)]
        public uint StartCell { get; set; }

        [ProtoMember(6)]
        public DirectionsEnum StartDirection { get; set; }

    }
}