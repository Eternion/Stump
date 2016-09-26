using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.Enums;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Breeds
{
    public class BreedRelator
    {
        public static string FetchQuery = "SELECT * FROM breeds " +
                                          "LEFT JOIN breeds_items ON breeds_items.BreedId = breeds.id " +
                                          "LEFT JOIN breeds_spells ON breeds_spells.BreedId = breeds.id";

        private Breed m_current;

        public Breed Map(Breed breed, BreedItem item, BreedSpell spell)
        {
            if (breed == null)
                return m_current;

            if (m_current != null && m_current.Id == breed.Id)
            {
                if (item.Id != 0 && !m_current.Items.Exists(x => x.Id == item.Id))
                    m_current.Items.Add(item);
                if (spell.Id != 0 && !m_current.Spells.Exists(x => x.Id == spell.Id))
                    m_current.Spells.Add(spell);
                return null;
            }

            var previous = m_current;

            m_current = breed;
            if (item.Id != 0 && !m_current.Items.Exists(x => x.Id == item.Id))
                m_current.Items.Add(item);
            if (spell.Id != 0 && !m_current.Spells.Exists(x => x.Id == spell.Id))
                m_current.Spells.Add(spell);

            return previous;
        }
    }

    [TableName("breeds")]
    [D2OClass("Breed", "com.ankamagames.dofus.datacenter.breeds")]
    public class Breed : IAssignedByD2O, ISaveIntercepter, IAutoGeneratedRecord
    {
        #region Record Properties

        private string m_breedSpellsIdCSV;
        private string m_femaleColorsCSV;
        private string m_femaleLookString;
        private string m_maleColorsCSV;
        private string m_maleLookString;
        private string m_statsPointsForAgilityCSV;
        private string m_statsPointsForChanceCSV;
        private string m_statsPointsForIntelligenceCSV;
        private string m_statsPointsForStrengthCSV;
        private string m_statsPointsForVitalityCSV;
        private string m_statsPointsForWisdomCSV;

        public Breed()
        {
            Items = new List<BreedItem>();
            Spells = new List<BreedSpell>();
        }

        // Primitive properties

        [PrimaryKey("Id", false)]
        public int Id
        {
            get;
            set;
        }

        public int GameplayDescriptionId
        {
            get;
            set;
        }

        public int ShortNameId
        {
            get;
            set;
        }

        public int LongNameId
        {
            get;
            set;
        }

        public int DescriptionId
        {
            get;
            set;
        }

        public string MaleLookString
        {
            get { return m_maleLookString; }
            set
            {
                m_maleLookString = value;
                m_maleLook = ActorLook.Parse(MaleLookString);
            }
        }

        public string FemaleLookString
        {
            get { return m_femaleLookString; }
            set
            {
                m_femaleLookString = value;
                m_femaleLook = ActorLook.Parse(FemaleLookString);
            }
        }

        public short CreatureBonesId
        {
            get;
            set;
        }

        public short TombBonesId
        {
            get;
            set;
        }

        public short MaleGhostBonesId
        {
            get;
            set;
        }

        public short FemaleGhostBonesId
        {
            get;
            set;
        }

        public int MaleArtwork
        {
            get;
            set;
        }

        public int FemaleArtwork
        {
            get;
            set;
        }

        public string StatsPointsForStrengthCSV
        {
            get { return m_statsPointsForStrengthCSV; }
            set
            {
                m_statsPointsForStrengthCSV = value;
                m_statsPointsForStrength = StatsPointsForStrengthCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string StatsPointsForIntelligenceCSV
        {
            get { return m_statsPointsForIntelligenceCSV; }
            set
            {
                m_statsPointsForIntelligenceCSV = value;
                m_statsPointsForIntelligence = StatsPointsForIntelligenceCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string StatsPointsForChanceCSV
        {
            get { return m_statsPointsForChanceCSV; }
            set
            {
                m_statsPointsForChanceCSV = value;
                m_statsPointsForChance = StatsPointsForChanceCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string StatsPointsForAgilityCSV
        {
            get { return m_statsPointsForAgilityCSV; }
            set
            {
                m_statsPointsForAgilityCSV = value;
                m_statsPointsForAgility = StatsPointsForAgilityCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string MaleColorsCSV
        {
            get { return m_maleColorsCSV; }
            set
            {
                m_maleColorsCSV = value;
                m_maleColors = MaleColorsCSV.FromCSV<uint>(",");
            }
        }

        public string StatsPointsForVitalityCSV
        {
            get { return m_statsPointsForVitalityCSV; }
            set
            {
                m_statsPointsForVitalityCSV = value;
                m_statsPointsForVitality = StatsPointsForVitalityCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string StatsPointsForWisdomCSV
        {
            get { return m_statsPointsForWisdomCSV; }
            set
            {
                m_statsPointsForWisdomCSV = value;
                m_statsPointsForWisdom = StatsPointsForWisdomCSV.FromCSV("|", x => x.FromCSV<uint>(","));
            }
        }

        public string BreedSpellsIdCSV
        {
            get { return m_breedSpellsIdCSV; }
            set
            {
                m_breedSpellsIdCSV = value;
                m_breedSpellsId = BreedSpellsIdCSV.FromCSV<uint>(",");
            }
        }

        public string FemaleColorsCSV
        {
            get { return m_femaleColorsCSV; }
            set
            {
                m_femaleColorsCSV = value;
                m_femaleColors = FemaleColorsCSV.FromCSV<uint>(",");
            }
        }

        public int StartMap
        {
            get;
            set;
        }

        public short StartCell
        {
            get;
            set;
        }

        public DirectionsEnum StartDirection
        {
            get;
            set;
        }

        public int StartActionPoints
        {
            get;
            set;
        }

        public int StartMovementPoints
        {
            get;
            set;
        }

        public int StartHealthPoint
        {
            get;
            set;
        }

        public int StartProspection
        {
            get;
            set;
        }

        public ushort StartStatsPoints
        {
            get;
            set;
        }

        public ushort StartSpellsPoints
        {
            get;
            set;
        }

        public short StartStrength
        {
            get;
            set;
        }

        public short StartVitality
        {
            get;
            set;
        }

        public short StartWisdom
        {
            get;
            set;
        }

        public short StartIntelligence
        {
            get;
            set;
        }

        public short StartChance
        {
            get;
            set;
        }

        public short StartAgility
        {
            get;
            set;
        }

        public short StartLevel
        {
            get;
            set;
        }

        public int StartKamas
        {
            get;
            set;
        }

        [Ignore]
        public List<BreedItem> Items
        {
            get;
            set;
        }

        [Ignore]
        public List<BreedSpell> Spells
        {
            get;
            set;
        }

        #endregion

        private uint[] m_breedSpellsId;
        private uint[] m_femaleColors;
        private ActorLook m_femaleLook;
        private string m_longName;
        private uint[] m_maleColors;
        private ActorLook m_maleLook;

        private string m_shortName;
        private ObjectPosition m_startPosition;
        private uint[][] m_statsPointsForAgility;
        private uint[][] m_statsPointsForChance;
        private uint[][] m_statsPointsForIntelligence;
        private uint[][] m_statsPointsForStrength;
        private uint[][] m_statsPointsForVitality;
        private uint[][] m_statsPointsForWisdom;


        [Ignore]
        public string ShortName
        {
            get { return m_shortName ?? (m_shortName = TextManager.Instance.GetText(ShortNameId)); }
        }

        [Ignore]
        public string LongName
        {
            get { return m_longName ?? (m_longName = TextManager.Instance.GetText(LongNameId)); }
        }

        [Ignore]
        public ActorLook MaleLook
        {
            get { return m_maleLook ?? (m_maleLook = ActorLook.Parse(MaleLookString)); }
            set
            {
                m_maleLook = value;
                MaleLookString = m_maleLook.ToString();
            }
        }

        [Ignore]
        public ActorLook FemaleLook
        {
            get
            {
                return m_femaleLook ?? ( m_femaleLook = ActorLook.Parse(FemaleLookString) );
            }
            set
            {
                m_femaleLook = value;
                FemaleLookString = m_femaleLook.ToString();
            }
        }

        [Ignore]
        public uint[][] StatsPointsForStrength
        {
            get
            {
                return m_statsPointsForStrength ??
                       (m_statsPointsForStrength = StatsPointsForStrengthCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForStrength = value;
                StatsPointsForStrengthCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[][] StatsPointsForIntelligence
        {
            get
            {
                return m_statsPointsForIntelligence ??
                       (m_statsPointsForIntelligence =
                        StatsPointsForIntelligenceCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForIntelligence = value;
                StatsPointsForIntelligenceCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[][] StatsPointsForChance
        {
            get
            {
                return m_statsPointsForChance ??
                       (m_statsPointsForChance = StatsPointsForChanceCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForChance = value;
                StatsPointsForChanceCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[][] StatsPointsForWisdom
        {
            get
            {
                return m_statsPointsForWisdom ??
                       (m_statsPointsForWisdom = StatsPointsForWisdomCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForWisdom = value;
                StatsPointsForWisdomCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[][] StatsPointsForVitality
        {
            get
            {
                return m_statsPointsForVitality ??
                       (m_statsPointsForVitality = StatsPointsForVitalityCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForVitality = value;
                StatsPointsForVitalityCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[][] StatsPointsForAgility
        {
            get
            {
                return m_statsPointsForAgility ??
                       (m_statsPointsForAgility = StatsPointsForAgilityCSV.FromCSV("|", x => x.FromCSV<uint>(",")));
            }
            set
            {
                m_statsPointsForAgility = value;
                StatsPointsForAgilityCSV = value.ToCSV("|", x => x.ToCSV(","));
            }
        }

        [Ignore]
        public uint[] MaleColors
        {
            get { return m_maleColors ?? (m_maleColors = MaleColorsCSV.FromCSV<uint>(",")); }
            set
            {
                m_maleColors = value;
                MaleColorsCSV = value.ToCSV(",");
            }
        }

        [Ignore]
        public uint[] FemaleColors
        {
            get { return m_femaleColors ?? (m_femaleColors = FemaleColorsCSV.FromCSV<uint>(",")); }
            set
            {
                m_femaleColors = value;
                FemaleColorsCSV = value.ToCSV(",");
            }
        }


        [Ignore]
        public uint[] BreedSpellsId
        {
            get { return m_breedSpellsId ?? (m_breedSpellsId = BreedSpellsIdCSV.FromCSV<uint>(",")); }
            set
            {
                m_breedSpellsId = value;
                BreedSpellsIdCSV = value.ToCSV(",");
            }
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var breed = (DofusProtocol.D2oClasses.Breed) d2oObject;
            Id = breed.id;
            GameplayDescriptionId = (int) breed.gameplayDescriptionId;
            ShortNameId = (int) breed.shortNameId;
            LongNameId = (int) breed.longNameId;
            DescriptionId = (int) breed.descriptionId;
            MaleLookString = breed.maleLook;
            FemaleLookString = breed.femaleLook;
            CreatureBonesId = (short)breed.creatureBonesId;
            MaleArtwork = breed.maleArtwork;
            FemaleArtwork = breed.femaleArtwork;
            StatsPointsForStrength = breed.statsPointsForStrength.Select(x => x.ToArray()).ToArray();
            StatsPointsForIntelligence = breed.statsPointsForIntelligence.Select(x => x.ToArray()).ToArray();
            StatsPointsForChance = breed.statsPointsForChance.Select(x => x.ToArray()).ToArray();
            StatsPointsForWisdom = breed.statsPointsForWisdom.Select(x => x.ToArray()).ToArray();
            StatsPointsForVitality = breed.statsPointsForVitality.Select(x => x.ToArray()).ToArray();
            StatsPointsForAgility = breed.statsPointsForAgility.Select(x => x.ToArray()).ToArray();
            MaleColors = breed.maleColors.ToArray();
            BreedSpellsId = breed.breedSpellsId.ToArray();
            FemaleColors = breed.femaleColors.ToArray();
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(bool insert)
        {
            MaleLookString = m_maleLook.ToString();
            FemaleLookString = m_femaleLook.ToString();

            StatsPointsForStrengthCSV = m_statsPointsForStrength.ToCSV("|", x => x.ToCSV(","));
            StatsPointsForIntelligenceCSV = m_statsPointsForIntelligence.ToCSV("|", x => x.ToCSV(","));
            StatsPointsForChanceCSV = m_statsPointsForChance.ToCSV("|", x => x.ToCSV(","));
            StatsPointsForWisdomCSV = m_statsPointsForWisdom.ToCSV("|", x => x.ToCSV(","));
            StatsPointsForVitalityCSV = m_statsPointsForVitality.ToCSV("|", x => x.ToCSV(","));
            StatsPointsForAgilityCSV = m_statsPointsForAgility.ToCSV("|", x => x.ToCSV(","));

            MaleColorsCSV = m_maleColors.ToCSV(",");
            FemaleColorsCSV = m_femaleColors.ToCSV(",");

            BreedSpellsIdCSV = m_breedSpellsId.ToCSV(",");
        }

        #endregion

        public ObjectPosition GetStartPosition()
        {
            return m_startPosition ??
                   (m_startPosition =
                    new ObjectPosition(Game.World.Instance.GetMap(StartMap), StartCell, StartDirection));
        }

        public uint[][] GetThresholds(StatsBoostTypeEnum statsid)
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

        public uint[] GetThreshold(short actualpoints, StatsBoostTypeEnum statsid)
        {
            uint[][] thresholds = GetThresholds(statsid);
            return thresholds[GetThresholdIndex(actualpoints, thresholds)];
        }

        public int GetThresholdIndex(int actualpoints, uint[][] thresholds)
        {
            for (int i = 0; i < thresholds.Length - 1; i++)
            {
                if (thresholds[i][0] <= actualpoints &&
                    thresholds[i + 1][0] > actualpoints)
                    return i;
            }

            return thresholds.Length - 1;
        }

        public ActorLook GetLook(SexTypeEnum sex, bool clone = false)
        {
            if (clone)
                return sex == SexTypeEnum.SEX_FEMALE ? FemaleLook.Clone() : MaleLook.Clone();

            return sex == SexTypeEnum.SEX_FEMALE ? FemaleLook : MaleLook;
        }

        public uint[] GetColors(SexTypeEnum sex)
        {
            return sex == SexTypeEnum.SEX_FEMALE ? FemaleColors : MaleColors;
        }
    }
}