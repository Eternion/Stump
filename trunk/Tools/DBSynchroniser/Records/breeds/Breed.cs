 


// Generated on 10/06/2013 14:21:58
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("Breeds")]
    [D2OClass("Breed")]
    public class BreedRecord : ID2ORecord
    {
        private const String MODULE = "Breeds";
        public int id;
        public uint shortNameId;
        public uint longNameId;
        public uint descriptionId;
        public uint gameplayDescriptionId;
        public String maleLook;
        public String femaleLook;
        public uint creatureBonesId;
        public int maleArtwork;
        public int femaleArtwork;
        public List<List<uint>> statsPointsForStrength;
        public List<List<uint>> statsPointsForIntelligence;
        public List<List<uint>> statsPointsForChance;
        public List<List<uint>> statsPointsForAgility;
        public List<List<uint>> statsPointsForVitality;
        public List<List<uint>> statsPointsForWisdom;
        public List<uint> breedSpellsId;
        public List<uint> maleColors;
        public List<uint> femaleColors;

        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint ShortNameId
        {
            get { return shortNameId; }
            set { shortNameId = value; }
        }

        public uint LongNameId
        {
            get { return longNameId; }
            set { longNameId = value; }
        }

        public uint DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }

        public uint GameplayDescriptionId
        {
            get { return gameplayDescriptionId; }
            set { gameplayDescriptionId = value; }
        }

        [NullString]
        public String MaleLook
        {
            get { return maleLook; }
            set { maleLook = value; }
        }

        [NullString]
        public String FemaleLook
        {
            get { return femaleLook; }
            set { femaleLook = value; }
        }

        public uint CreatureBonesId
        {
            get { return creatureBonesId; }
            set { creatureBonesId = value; }
        }

        public int MaleArtwork
        {
            get { return maleArtwork; }
            set { maleArtwork = value; }
        }

        public int FemaleArtwork
        {
            get { return femaleArtwork; }
            set { femaleArtwork = value; }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForStrength
        {
            get { return statsPointsForStrength; }
            set
            {
                statsPointsForStrength = value;
                m_statsPointsForStrengthBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForStrengthBin;
        public byte[] StatsPointsForStrengthBin
        {
            get { return m_statsPointsForStrengthBin; }
            set
            {
                m_statsPointsForStrengthBin = value;
                statsPointsForStrength = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForIntelligence
        {
            get { return statsPointsForIntelligence; }
            set
            {
                statsPointsForIntelligence = value;
                m_statsPointsForIntelligenceBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForIntelligenceBin;
        public byte[] StatsPointsForIntelligenceBin
        {
            get { return m_statsPointsForIntelligenceBin; }
            set
            {
                m_statsPointsForIntelligenceBin = value;
                statsPointsForIntelligence = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForChance
        {
            get { return statsPointsForChance; }
            set
            {
                statsPointsForChance = value;
                m_statsPointsForChanceBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForChanceBin;
        public byte[] StatsPointsForChanceBin
        {
            get { return m_statsPointsForChanceBin; }
            set
            {
                m_statsPointsForChanceBin = value;
                statsPointsForChance = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForAgility
        {
            get { return statsPointsForAgility; }
            set
            {
                statsPointsForAgility = value;
                m_statsPointsForAgilityBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForAgilityBin;
        public byte[] StatsPointsForAgilityBin
        {
            get { return m_statsPointsForAgilityBin; }
            set
            {
                m_statsPointsForAgilityBin = value;
                statsPointsForAgility = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForVitality
        {
            get { return statsPointsForVitality; }
            set
            {
                statsPointsForVitality = value;
                m_statsPointsForVitalityBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForVitalityBin;
        public byte[] StatsPointsForVitalityBin
        {
            get { return m_statsPointsForVitalityBin; }
            set
            {
                m_statsPointsForVitalityBin = value;
                statsPointsForVitality = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<List<uint>> StatsPointsForWisdom
        {
            get { return statsPointsForWisdom; }
            set
            {
                statsPointsForWisdom = value;
                m_statsPointsForWisdomBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statsPointsForWisdomBin;
        public byte[] StatsPointsForWisdomBin
        {
            get { return m_statsPointsForWisdomBin; }
            set
            {
                m_statsPointsForWisdomBin = value;
                statsPointsForWisdom = value == null ? null : value.ToObject<List<List<uint>>>();
            }
        }

        [Ignore]
        public List<uint> BreedSpellsId
        {
            get { return breedSpellsId; }
            set
            {
                breedSpellsId = value;
                m_breedSpellsIdBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_breedSpellsIdBin;
        public byte[] BreedSpellsIdBin
        {
            get { return m_breedSpellsIdBin; }
            set
            {
                m_breedSpellsIdBin = value;
                breedSpellsId = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [Ignore]
        public List<uint> MaleColors
        {
            get { return maleColors; }
            set
            {
                maleColors = value;
                m_maleColorsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_maleColorsBin;
        public byte[] MaleColorsBin
        {
            get { return m_maleColorsBin; }
            set
            {
                m_maleColorsBin = value;
                maleColors = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        [Ignore]
        public List<uint> FemaleColors
        {
            get { return femaleColors; }
            set
            {
                femaleColors = value;
                m_femaleColorsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_femaleColorsBin;
        public byte[] FemaleColorsBin
        {
            get { return m_femaleColorsBin; }
            set
            {
                m_femaleColorsBin = value;
                femaleColors = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (Breed)obj;
            
            Id = castedObj.id;
            ShortNameId = castedObj.shortNameId;
            LongNameId = castedObj.longNameId;
            DescriptionId = castedObj.descriptionId;
            GameplayDescriptionId = castedObj.gameplayDescriptionId;
            MaleLook = castedObj.maleLook;
            FemaleLook = castedObj.femaleLook;
            CreatureBonesId = castedObj.creatureBonesId;
            MaleArtwork = castedObj.maleArtwork;
            FemaleArtwork = castedObj.femaleArtwork;
            StatsPointsForStrength = castedObj.statsPointsForStrength;
            StatsPointsForIntelligence = castedObj.statsPointsForIntelligence;
            StatsPointsForChance = castedObj.statsPointsForChance;
            StatsPointsForAgility = castedObj.statsPointsForAgility;
            StatsPointsForVitality = castedObj.statsPointsForVitality;
            StatsPointsForWisdom = castedObj.statsPointsForWisdom;
            BreedSpellsId = castedObj.breedSpellsId;
            MaleColors = castedObj.maleColors;
            FemaleColors = castedObj.femaleColors;
        }
        
        public virtual object CreateObject()
        {
            
            var obj = new Breed();
            obj.id = Id;
            obj.shortNameId = ShortNameId;
            obj.longNameId = LongNameId;
            obj.descriptionId = DescriptionId;
            obj.gameplayDescriptionId = GameplayDescriptionId;
            obj.maleLook = MaleLook;
            obj.femaleLook = FemaleLook;
            obj.creatureBonesId = CreatureBonesId;
            obj.maleArtwork = MaleArtwork;
            obj.femaleArtwork = FemaleArtwork;
            obj.statsPointsForStrength = StatsPointsForStrength;
            obj.statsPointsForIntelligence = StatsPointsForIntelligence;
            obj.statsPointsForChance = StatsPointsForChance;
            obj.statsPointsForAgility = StatsPointsForAgility;
            obj.statsPointsForVitality = StatsPointsForVitality;
            obj.statsPointsForWisdom = StatsPointsForWisdom;
            obj.breedSpellsId = BreedSpellsId;
            obj.maleColors = MaleColors;
            obj.femaleColors = FemaleColors;
            return obj;
        
        }
    }
}