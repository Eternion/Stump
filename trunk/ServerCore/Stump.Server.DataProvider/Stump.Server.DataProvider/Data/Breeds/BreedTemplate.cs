
using System;
using System.Collections.Generic;
using ProtoBuf;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.DataProvider.Data.Breeds
{
    [Serializable,ProtoContract]
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
        public ushort StartCell { get; set; }

        [ProtoMember(6)]
        public DirectionsEnum StartDirection { get; set; }

        [ProtoMember(7)]
        public int StartStatsPoints { get; set; }

        [ProtoMember(8)]
        public ushort StartSpellsPoints { get; set; }

        [ProtoMember(9)]
        public ushort StartStrength { get; set; }

        [ProtoMember(10)]
        public ushort StartVitality { get; set; }

        [ProtoMember(11)]
        public ushort StartWisdom { get; set; }

        [ProtoMember(12)]
        public ushort StartIntelligence { get; set; }

        [ProtoMember(13)]
        public ushort StartChance { get; set; }

        [ProtoMember(14)]
        public ushort StartAgility { get; set; }

        [ProtoMember(15)]
        public uint StartLevel { get; set; }
    }
}