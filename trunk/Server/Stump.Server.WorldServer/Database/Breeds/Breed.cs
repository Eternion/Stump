using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Maps.Cells;

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

        private string m_shortName;

        public string ShortName
        {
            get
            {
                return m_shortName ?? ( m_shortName = TextManager.Instance.GetText(ShortNameId) );
            }
        }

        [D2OField("longNameId")]
        [Property("LongNameId")]
        public uint LongNameId
        {
            get;
            set;
        }

        private string m_longName;

        public string LongName
        {
            get
            {
                return m_longName ?? ( m_longName = TextManager.Instance.GetText(LongNameId) );
            }
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
        [Property("MaleLook", ColumnType = "StringClob", SqlType = "Text")]
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
        [Property("FemaleLook", ColumnType = "StringClob", SqlType = "Text")]
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

        [HasMany(typeof(LearnableSpell), Table = "breeds_spells", ColumnKey = "Breed", Cascade = ManyRelationCascadeEnum.All)]
        public IList<LearnableSpell> LearnableSpells
        {
            get;
            set;
        }

        [HasMany(typeof(StartItem), Table = "breeds_items", ColumnKey = "Breed", Cascade = ManyRelationCascadeEnum.All)]
        public IList<StartItem> StartItems
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

        private ObjectPosition m_startPosition;

        public ObjectPosition GetStartPosition()
        {
            return m_startPosition ?? ( m_startPosition = new ObjectPosition(Game.World.Instance.GetMap(StartMap), StartCell, StartDirection) );
        }

        [Property("StartActionPoints")]
        public ushort StartActionPoints
        {
            get;
            set;
        }

        [Property("StartMovementPoints")]
        public ushort StartMovementPoints
        {
            get;
            set;
        }

        [Property("StartHealthPoint")]
        public ushort StartHealthPoint
        {
            get;
            set;
        }

        [Property("StartProspection")]
        public ushort StartProspection
        {
            get;
            set;
        }

        [Property("StartStatsPoints")]
        public ushort StartStatsPoints
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
        public short StartStrength
        {
            get;
            set;
        }

        [Property("StartVitality")]
        public short StartVitality
        {
            get;
            set;
        }

        [Property("StartWisdom")]
        public short StartWisdom
        {
            get;
            set;
        }

        [Property("StartIntelligence")]
        public short StartIntelligence
        {
            get;
            set;
        }

        [Property("StartChance")]
        public short StartChance
        {
            get;
            set;
        }

        [Property("StartAgility")]
        public short StartAgility
        {
            get;
            set;
        }

        [Property("StartLevel")]
        public byte StartLevel
        {
            get;
            set;
        }

        [Property("StartKamas")]
        public int StartKamas
        {
            get;
            set;
        }

        public List<List<uint>> GetThresholds(StatsBoostTypeEnum statsid)
        {        
            switch (statsid)
            {
                case StatsBoostTypeEnum.Agility:
                    return StatsPointsForAgility;
                case StatsBoostTypeEnum.Chance:
                    return StatsPointsForChance;
                case StatsBoostTypeEnum.Intelligence:
                    return StatsPointsForIntelligence;
                case StatsBoostTypeEnum.Strength:
                    return StatsPointsForStrength;
                case StatsBoostTypeEnum.Wisdom:
                    return StatsPointsForWisdom;
                case StatsBoostTypeEnum.Vitality:
                    return StatsPointsForVitality;
                default:
                    throw new ArgumentException("statsid");
            }

        }

        public List<uint> GetThreshold(short actualpoints, StatsBoostTypeEnum statsid)
        {
            var thresholds = GetThresholds(statsid);
            return thresholds[GetThresholdIndex(actualpoints, thresholds)];
        }

        public int GetThresholdIndex(int actualpoints, List<List<uint>> thresholds)
        {
            for (int i = 0; i < thresholds.Count - 1; i++)
            {
                if (thresholds[i][0] <= actualpoints &&
                    thresholds[i + 1][0] > actualpoints)
                    return i;
            }

            return thresholds.Count - 1;
        }

        public EntityLook GetLook(SexTypeEnum sex)
        {
            return sex == SexTypeEnum.SEX_FEMALE ? FemaleLook : MaleLook;
        }
    }

}