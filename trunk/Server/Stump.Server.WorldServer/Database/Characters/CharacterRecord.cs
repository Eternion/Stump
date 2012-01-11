using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Items;
using Shortcut = Stump.Server.WorldServer.Database.Shortcuts.Shortcut;

namespace Stump.Server.WorldServer.Database.Characters
{
    [ActiveRecord("characters", Access=PropertyAccess.Property)]
    public class CharacterRecord : WorldBaseRecord<CharacterRecord>
    {
        private EntityLook m_entityLook;
        private string m_lookAsString;

        public CharacterRecord()
        {
            TitleParam = string.Empty; // why doesn't it work with Attribute ? dunno :x
        }

        public CharacterRecord(Breed breed)
            : this()
        {
            Breed = (PlayableBreedEnum) breed.Id;

            BaseHealth = breed.StartHealthPoint;
            AP = breed.StartActionPoints;
            MP = breed.StartMovementPoints;
            Prospection = breed.StartProspection;
            SpellsPoints = breed.StartSpellsPoints;
            StatsPoints = breed.StartStatsPoints;
            Strength = breed.StartStrength;
            Vitality = breed.StartVitality;
            Wisdom = breed.StartWisdom;
            Chance = breed.StartChance;
            Intelligence = breed.StartIntelligence;
            Agility = breed.StartAgility;

            MapId = breed.StartMap;
            CellId = breed.StartCell;
            Direction = breed.StartDirection;
        }

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property("Name", Length = 24, NotNull = true)]
        public string Name
        {
            get;
            set;
        }

        [Property("Breed", NotNull = true)]
        public PlayableBreedEnum Breed
        {
            get;
            set;
        }

        [Property("Sex", NotNull = true)]
        public SexTypeEnum Sex
        {
            get;
            set;
        }

        [Property("EntityLook", ColumnType = "StringClob", SqlType = "Text")]
        private string LookAsString
        {
            get
            {
                if (EntityLook == null)
                    return string.Empty;

                if (string.IsNullOrEmpty(m_lookAsString))
                    m_lookAsString = EntityLook.ConvertToString();

                return m_lookAsString;
            }
            set
            {
                m_lookAsString = value;

                if (value != null)
                    m_entityLook = m_lookAsString.ToEntityLook();
            }
        }

        public EntityLook EntityLook
        {
            get { return m_entityLook; }
            set
            {
                m_entityLook = value;

                if (value != null)
                    m_lookAsString = value.ConvertToString();
            }
        }

        [Property("TitleId", NotNull = true, Default = "0")]
        public uint TitleId
        {
            get;
            set;
        }

        [Property("TitleParam", NotNull = true, Default = "", ColumnType = "StringClob", SqlType = "Text")]
        public string TitleParam
        {
            get;
            set;
        }

        [Property("HasRecolor", NotNull = true, Default = "0")]
        public bool Recolor
        {
            get;
            set;
        }

        [Property("HasRename", NotNull = true, Default = "0")]
        public bool Rename
        {
            get;
            set;
        }

        #region Restrictions
        [Property]
        public bool CantBeAggressed
        {
            get;
            set;
        }

        [Property]
        public bool CantBeChallenged
        {
            get;
            set;
        }

        [Property]
        public bool CantTrade
        {
            get;
            set;
        }

        [Property]
        public bool CantBeAttackedByMutant
        {
            get;
            set;
        }

        [Property]
        public bool CantRun
        {
            get;
            set;
        }

        [Property]
        public bool ForceSlowWalk
        {
            get;
            set;
        }

        [Property]
        public bool CantMinimize
        {
            get;
            set;
        }

        [Property]
        public bool CantMove
        {
            get;
            set;
        }

        [Property]
        public bool CantAggress
        {
            get;
            set;
        }

        [Property]
        public bool CantChallenge
        {
            get;
            set;
        }

        [Property]
        public bool CantExchange
        {
            get;
            set;
        }

        [Property]
        public bool CantAttack
        {
            get;
            set;
        }

        [Property]
        public bool CantChat
        {
            get;
            set;
        }

        [Property]
        public bool CantBeMerchant
        {
            get;
            set;
        }

        [Property]
        public bool CantUseObject
        {
            get;
            set;
        }

        [Property]
        public bool CantUseTaxCollector
        {
            get;
            set;
        }

        [Property]
        public bool CantUseInteractive
        {
            get;
            set;
        }

        [Property]
        public bool CantSpeakToNpc
        {
            get;
            set;
        }

        [Property]
        public bool CantChangeZone
        {
            get;
            set;
        }

        [Property]
        public bool CantAttackMonster
        {
            get;
            set;
        }

        [Property]
        public bool CantWalk8Directions
        {
            get;
            set;
        }
        #endregion

        #region Position

        [Property("MapId", NotNull = true)]
        public int MapId
        {
            get;
            set;
        }

        [Property("CellId", NotNull = true)]
        public short CellId
        {
            get;
            set;
        }

        [Property("Direction", NotNull = true)]
        public DirectionsEnum Direction
        {
            get;
            set;
        }

        #endregion

        #region Stats

        [Property("BaseHealth", NotNull = true)]
        public ushort BaseHealth
        {
            get;
            set;
        }

        [Property("DamageTaken", NotNull = true)]
        public ushort DamageTaken
        {
            get;
            set;
        }

        [Property("AP", NotNull = true)]
        public ushort AP
        {
            get;
            set;
        }

        [Property("MP", NotNull = true)]
        public ushort MP
        {
            get;
            set;
        }

        [Property("Prospection", NotNull = true)]
        public ushort Prospection
        {
            get;
            set;
        }

        [Property("Strength", NotNull = true)]
        public short Strength
        {
            get;
            set;
        }

        [Property("Chance", NotNull = true)]
        public short Chance
        {
            get;
            set;
        }

        [Property("Vitality", NotNull = true)]
        public short Vitality
        {
            get;
            set;
        }

        [Property("Wisdom", NotNull = true)]
        public short Wisdom
        {
            get;
            set;
        }

        [Property("Intelligence", NotNull = true)]
        public short Intelligence
        {
            get;
            set;
        }

        [Property("Agility", NotNull = true)]
        public short Agility
        {
            get;
            set;
        }

        [Property("Kamas", NotNull = true, Default = "0")]
        public int Kamas
        {
            get;
            set;
        }
        #endregion

        #region Points

        [Property("Experience", NotNull = true, Default = "0")]
        public long Experience
        {
            get;
            set;
        }

        [Property("EnergyMax", NotNull = true, Default = "10000")]
        public short EnergyMax
        {
            get;
            set;
        }

        [Property("Energy", NotNull = true, Default = "10000")]
        public short Energy
        {
            get;
            set;
        }

        [Property("StatsPoints", NotNull = true, Default = "0")]
        public ushort StatsPoints
        {
            get;
            set;
        }

        [Property("SpellsPoints", NotNull = true, Default = "0")]
        public ushort SpellsPoints
        {
            get;
            set;
        }

        #endregion

        protected override void OnDelete()
        {
            ItemRecord.DeleteAll("OwnerId = " + Id);
            CharacterSpellRecord.DeleteAll("OwnerId = " + Id);
            Shortcut.DeleteAll("OwnerId = " + Id);


            base.OnDelete();
        }

        public static CharacterRecord FindById(int characterId)
        {
            return FindByPrimaryKey(characterId);
        }

        public static CharacterRecord FindByName(string characterName)
        {
            return FindOne(NHibernate.Criterion.Restrictions.Eq("Name", characterName));
        }

        public static bool DoesNameExists(string name)
        {
            return Exists(NHibernate.Criterion.Restrictions.Eq("Name", name));
        }

        public static int GetCount()
        {
            return Count();
        }
    }
}