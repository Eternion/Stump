using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database
{
    public class BreedConfiguration : EntityTypeConfiguration<Breed>
    {
        public BreedConfiguration()
        {
            ToTable("breeds");
            HasMany(x => x.Items);
            HasMany(x => x.Spells);

            Ignore(x => x.MaleLook);
            Ignore(x => x.FemaleLook);
            Ignore(x => x.AlternativeFemaleSkin);
            Ignore(x => x.AlternativeMaleSkin);
            Ignore(x => x.StatsPointsForStrength);
            Ignore(x => x.StatsPointsForIntelligence);
            Ignore(x => x.StatsPointsForChance);
            Ignore(x => x.StatsPointsForWisdom);
            Ignore(x => x.StatsPointsForVitality);
            Ignore(x => x.StatsPointsForAgility);
            Ignore(x => x.MaleColors);
            Ignore(x => x.FemaleColors);
            Ignore(x => x.BreedSpellsId);
        }
    }

    [D2OClass("Breed", "com.ankamagames.dofus.datacenter.breeds")]
    public partial class Breed : IAssignedByD2O, ISaveIntercepter
    {
        #region Record Properties

        private byte[] m_alternativeFemaleSkinBin;
        private byte[] m_alternativeMaleSkinBin;
        private byte[] m_breedSpellsIdBin;
        private byte[] m_femaleColorsBin;
        private string m_femaleLookString;
        private byte[] m_maleColorsBin;
        private string m_maleLookString;
        private byte[] m_statsPointsForAgilityBin;
        private byte[] m_statsPointsForChanceBin;
        private byte[] m_statsPointsForIntelligenceBin;
        private byte[] m_statsPointsForStrengthBin;
        private byte[] m_statsPointsForVitalityBin;
        private byte[] m_statsPointsForWisdomBin;

        public Breed()
        {
            Items = new HashSet<BreedItem>();
            Spells = new HashSet<BreedSpell>();
        }

        // Primitive properties

        public int Id
        {
            get;
            set;
        }

        public byte[] AlternativeMaleSkinBin
        {
            get { return m_alternativeMaleSkinBin; }
            set
            {
                m_alternativeMaleSkinBin = value;
                m_alternativeMaleSkin = AlternativeMaleSkinBin.ToObject<List<uint>>();
            }
        }

        public byte[] AlternativeFemaleSkinBin
        {
            get { return m_alternativeFemaleSkinBin; }
            set
            {
                m_alternativeFemaleSkinBin = value;
                m_alternativeFemaleSkin = AlternativeFemaleSkinBin.ToObject<List<uint>>();
            }
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
                m_maleLook = MaleLookString.ToEntityLook();
            }
        }

        public string FemaleLookString
        {
            get { return m_femaleLookString; }
            set
            {
                m_femaleLookString = value;
                m_femaleLook = FemaleLookString.ToEntityLook();
            }
        }

        public long CreatureBonesId
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

        public byte[] StatsPointsForStrengthBin
        {
            get { return m_statsPointsForStrengthBin; }
            set
            {
                m_statsPointsForStrengthBin = value;
                m_statsPointsForStrength = StatsPointsForStrengthBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] StatsPointsForIntelligenceBin
        {
            get { return m_statsPointsForIntelligenceBin; }
            set
            {
                m_statsPointsForIntelligenceBin = value;
                m_statsPointsForIntelligence = StatsPointsForIntelligenceBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] StatsPointsForChanceBin
        {
            get { return m_statsPointsForChanceBin; }
            set
            {
                m_statsPointsForChanceBin = value;
                m_statsPointsForChance = StatsPointsForChanceBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] StatsPointsForAgilityBin
        {
            get { return m_statsPointsForAgilityBin; }
            set
            {
                m_statsPointsForAgilityBin = value;
                m_statsPointsForAgility = StatsPointsForAgilityBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] MaleColorsBin
        {
            get { return m_maleColorsBin; }
            set
            {
                m_maleColorsBin = value;
                m_maleColors = MaleColorsBin.ToObject<List<uint>>();
            }
        }

        public byte[] StatsPointsForVitalityBin
        {
            get { return m_statsPointsForVitalityBin; }
            set
            {
                m_statsPointsForVitalityBin = value;
                m_statsPointsForVitality = StatsPointsForVitalityBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] StatsPointsForWisdomBin
        {
            get { return m_statsPointsForWisdomBin; }
            set
            {
                m_statsPointsForWisdomBin = value;
                m_statsPointsForWisdom = StatsPointsForWisdomBin.ToObject<List<List<uint>>>();
            }
        }

        public byte[] BreedSpellsIdBin
        {
            get { return m_breedSpellsIdBin; }
            set
            {
                m_breedSpellsIdBin = value;
                m_breedSpellsId = BreedSpellsIdBin.ToObject<List<uint>>();
            }
        }

        public byte[] FemaleColorsBin
        {
            get { return m_femaleColorsBin; }
            set
            {
                m_femaleColorsBin = value;
                m_femaleColors = FemaleColorsBin.ToObject<List<uint>>();
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

        public int StartDirection
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

        public int StartStatsPoints
        {
            get;
            set;
        }

        public int StartSpellsPoints
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

        public byte StartLevel
        {
            get;
            set;
        }

        public int StartKamas
        {
            get;
            set;
        }

        // Navigation properties

        public virtual ICollection<BreedItem> Items
        {
            get;
            set;
        }

        public virtual ICollection<BreedSpell> Spells
        {
            get;
            set;
        }

        #endregion

        private List<uint> m_alternativeFemaleSkin;
        private List<uint> m_alternativeMaleSkin;
        private List<uint> m_breedSpellsId;
        private List<uint> m_femaleColors;
        private EntityLook m_femaleLook;
        private string m_longName;
        private List<uint> m_maleColors;
        private EntityLook m_maleLook;

        private string m_shortName;
        private ObjectPosition m_startPosition;
        private List<List<uint>> m_statsPointsForAgility;
        private List<List<uint>> m_statsPointsForChance;
        private List<List<uint>> m_statsPointsForIntelligence;
        private List<List<uint>> m_statsPointsForStrength;
        private List<List<uint>> m_statsPointsForVitality;
        private List<List<uint>> m_statsPointsForWisdom;

        public List<uint> AlternativeFemaleSkin
        {
            get
            {
                return m_alternativeFemaleSkin ??
                       (m_alternativeFemaleSkin = AlternativeFemaleSkinBin.ToObject<List<uint>>());
            }
            set
            {
                m_alternativeFemaleSkin = value;
                AlternativeFemaleSkinBin = value.ToBinary();
            }
        }

        public List<uint> AlternativeMaleSkin
        {
            get { return m_alternativeMaleSkin ?? (m_alternativeMaleSkin = AlternativeMaleSkinBin.ToObject<List<uint>>()); }
            set
            {
                m_alternativeMaleSkin = value;
                AlternativeMaleSkinBin = value.ToBinary();
            }
        }


        public string ShortName
        {
            get { return m_shortName ?? (m_shortName = TextManager.Instance.GetText(ShortNameId)); }
        }

        public string LongName
        {
            get { return m_longName ?? (m_longName = TextManager.Instance.GetText(LongNameId)); }
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

        public EntityLook FemaleLook
        {
            get { return m_femaleLook ?? (m_femaleLook = FemaleLookString.ToEntityLook()); }
            set
            {
                m_femaleLook = value;
                FemaleLookString = m_femaleLook.ConvertToString();
            }
        }

        public List<List<uint>> StatsPointsForStrength
        {
            get
            {
                return m_statsPointsForStrength ??
                       (m_statsPointsForStrength = StatsPointsForStrengthBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForStrength = value;
                StatsPointsForStrengthBin = value.ToBinary();
            }
        }

        public List<List<uint>> StatsPointsForIntelligence
        {
            get
            {
                return m_statsPointsForIntelligence ??
                       (m_statsPointsForIntelligence = StatsPointsForIntelligenceBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForIntelligence = value;
                StatsPointsForIntelligenceBin = value.ToBinary();
            }
        }

        public List<List<uint>> StatsPointsForChance
        {
            get
            {
                return m_statsPointsForChance ??
                       (m_statsPointsForChance = StatsPointsForChanceBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForChance = value;
                StatsPointsForChanceBin = value.ToBinary();
            }
        }

        public List<List<uint>> StatsPointsForWisdom
        {
            get
            {
                return m_statsPointsForWisdom ??
                       (m_statsPointsForWisdom = StatsPointsForWisdomBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForWisdom = value;
                StatsPointsForWisdomBin = value.ToBinary();
            }
        }

        public List<List<uint>> StatsPointsForVitality
        {
            get
            {
                return m_statsPointsForVitality ??
                       (m_statsPointsForVitality = StatsPointsForVitalityBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForVitality = value;
                StatsPointsForVitalityBin = value.ToBinary();
            }
        }

        public List<List<uint>> StatsPointsForAgility
        {
            get
            {
                return m_statsPointsForAgility ??
                       (m_statsPointsForAgility = StatsPointsForAgilityBin.ToObject<List<List<uint>>>());
            }
            set
            {
                m_statsPointsForAgility = value;
                StatsPointsForAgilityBin = value.ToBinary();
            }
        }

        public List<uint> MaleColors
        {
            get { return m_maleColors ?? (m_maleColors = MaleColorsBin.ToObject<List<uint>>()); }
            set
            {
                m_maleColors = value;
                MaleColorsBin = value.ToBinary();
            }
        }

        public List<uint> FemaleColors
        {
            get { return m_femaleColors ?? (m_femaleColors = FemaleColorsBin.ToObject<List<uint>>()); }
            set
            {
                m_femaleColors = value;
                FemaleColorsBin = value.ToBinary();
            }
        }


        public List<uint> BreedSpellsId
        {
            get { return m_breedSpellsId ?? (m_breedSpellsId = BreedSpellsIdBin.ToObject<List<uint>>()); }
            set
            {
                m_breedSpellsId = value;
                BreedSpellsIdBin = value.ToBinary();
            }
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var breed = (DofusProtocol.D2oClasses.Breed) d2oObject;

            AlternativeFemaleSkin = breed.alternativeFemaleSkin;
            AlternativeMaleSkin = breed.alternativeMaleSkin;
            GameplayDescriptionId = (int) breed.gameplayDescriptionId;
            ShortNameId = (int) breed.shortNameId;
            LongNameId = (int) breed.longNameId;
            DescriptionId = (int) breed.descriptionId;
            MaleLookString = breed.maleLook;
            FemaleLookString = breed.femaleLook;
            CreatureBonesId = breed.creatureBonesId;
            MaleArtwork = breed.maleArtwork;
            FemaleArtwork = breed.femaleArtwork;
            StatsPointsForStrength = breed.statsPointsForStrength;
            StatsPointsForIntelligence = breed.statsPointsForIntelligence;
            StatsPointsForChance = breed.statsPointsForChance;
            StatsPointsForWisdom = breed.statsPointsForWisdom;
            StatsPointsForVitality = breed.statsPointsForVitality;
            StatsPointsForAgility = breed.statsPointsForAgility;
            MaleColors = breed.maleColors;
            BreedSpellsId = breed.breedSpellsId;
            FemaleColors = breed.femaleColors;
        }

        #endregion

        public ObjectPosition GetStartPosition()
        {
            return m_startPosition ??
                   (m_startPosition =
                    new ObjectPosition(Game.World.Instance.GetMap(StartMap), StartCell, (DirectionsEnum) StartDirection));
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
            List<List<uint>> thresholds = GetThresholds(statsid);
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

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            AlternativeFemaleSkinBin = m_alternativeFemaleSkin.ToBinary();
            AlternativeMaleSkinBin = m_alternativeMaleSkin.ToBinary();

            MaleLookString = m_maleLook.ConvertToString();
            FemaleLookString = m_femaleLook.ConvertToString();

            StatsPointsForStrengthBin = m_statsPointsForStrength.ToBinary();
            StatsPointsForIntelligenceBin = m_statsPointsForIntelligence.ToBinary();
            StatsPointsForChanceBin = m_statsPointsForChance.ToBinary();
            StatsPointsForWisdomBin = m_statsPointsForWisdom.ToBinary();
            StatsPointsForVitalityBin = m_statsPointsForVitality.ToBinary();
            StatsPointsForAgilityBin = m_statsPointsForAgility.ToBinary();

            MaleColorsBin = m_maleColors.ToBinary();
            FemaleColorsBin = m_femaleColors.ToBinary();

            BreedSpellsIdBin = m_breedSpellsId.ToBinary();
        }
    }
}