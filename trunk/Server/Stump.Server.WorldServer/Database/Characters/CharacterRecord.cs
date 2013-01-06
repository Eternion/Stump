using System;
using System.Collections.Generic;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Game.Maps;

namespace Stump.Server.WorldServer.Database.Characters
{
    public class CharacterRelator
    {
        public static string FetchQuery =
            "SELECT * FROM characters";

        /// <summary>
        /// Use string.Format(ToCSV(","))
        /// </summary>
        public static string FetchByMultipleId =
            "SELECT * FROM characters WHERE Id IN ({0})";
    }

    [TableName("characters")]
    public class CharacterRecord : IAutoGeneratedRecord
    {
        private EntityLook m_entityLook;

        #region Record Properties

        private string m_customEntityLookString;
        private string m_entityLookString;
        private byte[] m_knownZaapsBin;
        private int? m_spawnMapId;

        public CharacterRecord()
        {
            TitleParam = string.Empty;
        }

        // Primitive properties

        public int Id
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get;
            set;
        }

        public DateTime? LastUsage
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public PlayableBreedEnum Breed
        {
            get;
            set;
        }

        public SexTypeEnum Sex
        {
            get;
            set;
        }

        public int Head
        {
            get;
            set;
        }

        [NullString]
        public string EntityLookString
        {
            get { return m_entityLookString; }
            set
            {
                m_entityLookString = value;
                m_entityLook = !string.IsNullOrEmpty(EntityLookString) ? m_entityLookString.ToEntityLook() : null;
            }
        }

        [NullString]
        public string CustomEntityLookString
        {
            get { return m_customEntityLookString; }
            set
            {
                m_customEntityLookString = value;
                m_customEntityLook = !string.IsNullOrEmpty(CustomEntityLookString)
                                         ? CustomEntityLookString.ToEntityLook()
                                         : null;
            }
        }

        public bool CustomLookActivated
        {
            get;
            set;
        }

        public long TitleId
        {
            get;
            set;
        }

        public string TitleParam
        {
            get;
            set;
        }

        public bool HasRecolor
        {
            get;
            set;
        }

        public bool HasRename
        {
            get;
            set;
        }

        public bool CantBeAggressed
        {
            get;
            set;
        }

        public bool CantBeChallenged
        {
            get;
            set;
        }

        public bool CantTrade
        {
            get;
            set;
        }

        public bool CantBeAttackedByMutant
        {
            get;
            set;
        }

        public bool CantRun
        {
            get;
            set;
        }

        public bool ForceSlowWalk
        {
            get;
            set;
        }

        public bool CantMinimize
        {
            get;
            set;
        }

        public bool CantMove
        {
            get;
            set;
        }

        public bool CantAggress
        {
            get;
            set;
        }

        public bool CantChallenge
        {
            get;
            set;
        }

        public bool CantExchange
        {
            get;
            set;
        }

        public bool CantAttack
        {
            get;
            set;
        }

        public bool CantChat
        {
            get;
            set;
        }

        public bool CantBeMerchant
        {
            get;
            set;
        }

        public bool CantUseObject
        {
            get;
            set;
        }

        public bool CantUseTaxCollector
        {
            get;
            set;
        }

        public bool CantUseInteractive
        {
            get;
            set;
        }

        public bool CantSpeakToNpc
        {
            get;
            set;
        }

        public bool CantChangeZone
        {
            get;
            set;
        }

        public bool CantAttackMonster
        {
            get;
            set;
        }

        public bool CantWalk8Directions
        {
            get;
            set;
        }

        public int MapId
        {
            get;
            set;
        }

        public short CellId
        {
            get;
            set;
        }

        public DirectionsEnum Direction
        {
            get;
            set;
        }

        public int BaseHealth
        {
            get;
            set;
        }

        public int DamageTaken
        {
            get;
            set;
        }

        public int AP
        {
            get;
            set;
        }

        public int MP
        {
            get;
            set;
        }

        public int Prospection
        {
            get;
            set;
        }

        public short Strength
        {
            get;
            set;
        }

        public short Chance
        {
            get;
            set;
        }

        public short Vitality
        {
            get;
            set;
        }

        public short Wisdom
        {
            get;
            set;
        }

        public short Intelligence
        {
            get;
            set;
        }

        public short Agility
        {
            get;
            set;
        }

        public short PermanentAddedStrength
        {
            get;
            set;
        }

        public short PermanentAddedChance
        {
            get;
            set;
        }

        public short PermanentAddedVitality
        {
            get;
            set;
        }

        public short PermanentAddedWisdom
        {
            get;
            set;
        }

        public short PermanentAddedIntelligence
        {
            get;
            set;
        }

        public short PermanentAddedAgility
        {
            get;
            set;
        }

        public int Kamas
        {
            get;
            set;
        }

        public bool CanRestat
        {
            get;
            set;
        }

        public long Experience
        {
            get;
            set;
        }

        public short EnergyMax
        {
            get;
            set;
        }

        public short Energy
        {
            get;
            set;
        }

        public ushort StatsPoints
        {
            get;
            set;
        }

        public ushort SpellsPoints
        {
            get;
            set;
        }

        public AlignmentSideEnum AlignmentSide
        {
            get;
            set;
        }

        public sbyte AlignmentValue
        {
            get;
            set;
        }

        public ushort Honor
        {
            get;
            set;
        }

        public ushort Dishonor
        {
            get;
            set;
        }

        public bool PvPEnabled
        {
            get;
            set;
        }

        public byte[] KnownZaapsBin
        {
            get { return m_knownZaapsBin; }
            set
            {
                m_knownZaapsBin = value;
                m_knownZaaps = UnSerializeZaaps(KnownZaapsBin);
            }
        }

        public int? SpawnMapId
        {
            get { return m_spawnMapId; }
            set
            {
                m_spawnMapId = value;
                m_spawnMap = null;
            }
        }

        public bool WarnOnConnection
        {
            get;
            set;
        }

        public bool WarnOnLevel
        {
            get;
            set;
        }

        public DateTime? MuteUntil
        {
            get;
            set;
        }

        public bool Rename
        {
            get;
            set;
        }

        public bool Recolor
        {
            get;
            set;
        }

        public bool Relook
        {
            get;
            set;
        }

        #endregion

        public CharacterRecord(Breed breed)
            : this()
        {
            Breed = (PlayableBreedEnum) breed.Id;

            BaseHealth = (ushort) (breed.StartHealthPoint + breed.StartLevel*5);
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
            Direction = (DirectionsEnum) breed.StartDirection;

            SpellsPoints = breed.StartLevel;
            StatsPoints = (ushort) (breed.StartLevel*5);
            Kamas = breed.StartKamas;

            CanRestat = true;

            if (breed.StartLevel > 100)
                AP++;
        }


        [Ignore]
        public EntityLook EntityLook
        {
            get { return m_entityLook; }
            set
            {
                m_entityLook = value;
                EntityLookString = value != null ? value.ConvertToString() : string.Empty;
            }
        }

        [Ignore]
        public EntityLook CustomEntityLook
        {
            get { return m_customEntityLook; }
            set
            {
                m_customEntityLook = value;
                CustomEntityLookString = value != null ? value.ConvertToString() : string.Empty;
            }
        }
        #region Zaaps

        private EntityLook m_customEntityLook;
        private string m_customLookAsString;
        private List<Map> m_knownZaaps = new List<Map>();
        private Map m_spawnMap;

        [Ignore]
        public List<Map> KnownZaaps
        {
            get { return m_knownZaaps; }
            set
            {
                m_knownZaaps = value;
                KnownZaapsBin = SerializeZaaps(m_knownZaaps);
            }
        }

        [Ignore]
        public Map SpawnMap
        {
            get
            {
                if (!SpawnMapId.HasValue)
                    return null;

                return m_spawnMap ?? (m_spawnMap = Game.World.Instance.GetMap(SpawnMapId.Value));
            }
            set
            {
                m_spawnMap = value;

                if (value == null)
                    SpawnMapId = null;
                else
                    SpawnMapId = value.Id;
            }
        }


        private byte[] SerializeZaaps(List<Map> knownZaaps)
        {
            var result = new byte[knownZaaps.Count*4];

            for (int i = 0; i < knownZaaps.Count; i++)
            {
                result[i*4] = (byte) (knownZaaps[i].Id >> 24);
                result[i*4 + 1] = (byte) ((knownZaaps[i].Id >> 16) & 0xFF);
                result[i*4 + 2] = (byte) ((knownZaaps[i].Id >> 8) & 0xFF);
                result[i*4 + 3] = (byte) (knownZaaps[i].Id & 0xFF);
            }

            return result;
        }

        private List<Map> UnSerializeZaaps(byte[] serialized)
        {
            var result = new List<Map>();

            for (int i = 0; i < serialized.Length; i += 4)
            {
                int id = serialized[i] << 24 | serialized[i + 1] << 16 | serialized[i + 2] << 8 | serialized[i + 3];

                Map map = Game.World.Instance.GetMap(id);

                if (map == null)
                    throw new Exception("Map " + id + " not found");

                result.Add(map);
            }

            return result;
        }

        #endregion
    }
}