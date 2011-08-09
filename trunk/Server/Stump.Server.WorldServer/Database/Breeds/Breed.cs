using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;

namespace Stump.Server.WorldServer.Database.Breeds
{
    [ActiveRecord("breeds")]
    [D2OClass("Breed", "com.ankamagames.dofus.datacenter.breeds")]
    public partial class Breed : WorldBaseRecord<Breed>
    {
        private EntityLook m_femaleLook;
        private EntityLook m_maleLook;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [D2OField("alternativeMaleSkin")]
        [Property("AlternativeMaleSkin", ColumnType = "Serializable")]
        public List<uint> AlternativeMaleSkin
        {
            get;
            set;
        }

        [D2OField("alternativeFemaleSkin")]
        [Property("AlternativeFemaleSkin", ColumnType = "Serializable")]
        public List<uint> AlternativeFemaleSkin
        {
            get;
            set;
        }

        [D2OField("gameplayDescriptionId")]
        [Property("GameplayDescriptionId")]
        public uint GameplayDescriptionId
        {
            get;
            set;
        }

        [D2OField("shortNameId")]
        [Property("ShortNameId")]
        public uint ShortNameId
        {
            get;
            set;
        }

        [D2OField("longNameId")]
        [Property("LongNameId")]
        public uint LongNameId
        {
            get;
            set;
        }

        [D2OField("descriptionId")]
        [Property("DescriptionId")]
        public uint DescriptionId
        {
            get;
            set;
        }

        private string m_maleLookString;

        [D2OField("maleLook")]
        [Property("MaleLook")]
        private String MaleLookString
        {
            get
            {
                return m_maleLookString;
            }
            set
            {
                m_maleLookString = value;
                m_maleLook = null; // update the entitylook
            }
        }

        public EntityLook MaleLook
        {
            get { return m_maleLook ?? (m_maleLook = MaleLookString.ToEntityLook()); }
            set
            {
                m_maleLook = value;
                MaleLookString = m_maleLook.ConvertToString();
            }
        }

        private string m_femaleLookString;

        [D2OField("femaleLook")]
        [Property("FemaleLook")]
        private String FemaleLookString
        {
            get
            {
                return m_femaleLookString;
            }
            set
            {
                m_femaleLookString = value;
                m_femaleLook = null;
            }
        }

        public EntityLook FemaleLook
        {
            get { return m_femaleLook ?? (m_femaleLook = FemaleLookString.ToEntityLook()); }
            set
            {
                m_femaleLook = value;
                FemaleLookString = m_femaleLook.ConvertToString();
            }
        }

        [D2OField("creatureBonesId")]
        [Property("CreatureBonesId")]
        public uint CreatureBonesId
        {
            get;
            set;
        }

        [D2OField("maleArtwork")]
        [Property("MaleArtwork")]
        public int MaleArtwork
        {
            get;
            set;
        }

        [D2OField("femaleArtwork")]
        [Property("FemaleArtwork")]
        public int FemaleArtwork
        {
            get;
            set;
        }

        [D2OField("statsPointsForStrength")]
        [Property("StatsPointsForStrength", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForStrength
        {
            get;
            set;
        }

        [D2OField("statsPointsForIntelligence")]
        [Property("StatsPointsForIntelligence", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForIntelligence
        {
            get;
            set;
        }

        [D2OField("statsPointsForChance")]
        [Property("StatsPointsForChance", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForChance
        {
            get;
            set;
        }

        [D2OField("statsPointsForAgility")]
        [Property("StatsPointsForAgility", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForAgility
        {
            get;
            set;
        }

        [D2OField("maleColors")]
        [Property("MaleColors", ColumnType = "Serializable")]
        public List<uint> MaleColors
        {
            get;
            set;
        }

        [D2OField("statsPointsForVitality")]
        [Property("StatsPointsForVitality", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForVitality
        {
            get;
            set;
        }

        [D2OField("statsPointsForWisdom")]
        [Property("StatsPointsForWisdom", ColumnType = "Serializable")]
        public List<List<uint>> StatsPointsForWisdom
        {
            get;
            set;
        }

        [D2OField("breedSpellsId")]
        [Property("BreedSpellsId", ColumnType = "Serializable")]
        public List<uint> BreedSpellsId
        {
            get;
            set;
        }

        [D2OField("femaleColors")]
        [Property("FemaleColors", ColumnType = "Serializable")]
        public List<uint> FemaleColors
        {
            get;
            set;
        }

        [HasMany(typeof(LearnableSpell), Table = "breeds_spells", ColumnKey = "BreedId", Cascade = ManyRelationCascadeEnum.All)]
        public IList<LearnableSpell> LearnableSpells
        {
            get;
            set;
        }

        [Property("StartMap")]
        public int StartMap
        {
            get;
            set;
        }

        [Property("StartCell")]
        public short StartCell
        {
            get;
            set;
        }

        [Property("StartDirection")]
        public DirectionsEnum StartDirection
        {
            get;
            set;
        }


        [Property("StartActionPoints")]
        public uint StartActionPoints
        {
            get;
            set;
        }

        [Property("StartMovementPoints")]
        public uint StartMovementPoints
        {
            get;
            set;
        }

        [Property("StartHealthPoint")]
        public uint StartHealthPoint
        {
            get;
            set;
        }

        [Property("StartStatsPoints")]
        public int StartStatsPoints
        {
            get;
            set;
        }

        [Property("StartSpellsPoints")]
        public ushort StartSpellsPoints
        {
            get;
            set;
        }

        [Property("StartStrength")]
        public ushort StartStrength
        {
            get;
            set;
        }

        [Property("StartVitality")]
        public ushort StartVitality
        {
            get;
            set;
        }

        [Property("StartWisdom")]
        public ushort StartWisdom
        {
            get;
            set;
        }

        [Property("StartIntelligence")]
        public ushort StartIntelligence
        {
            get;
            set;
        }

        [Property("StartChance")]
        public ushort StartChance
        {
            get;
            set;
        }

        [Property("StartAgility")]
        public ushort StartAgility
        {
            get;
            set;
        }

        [Property("StartLevel")]
        public uint StartLevel
        {
            get;
            set;
        }
    }

}