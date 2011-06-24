
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Database.Interfaces;
using Stump.Database.Types;
using Stump.Database.WorldServer.Alignment;
using Stump.Database.WorldServer.Guild;
using Stump.Database.WorldServer.Job;
using Stump.Database.WorldServer.Quest;
using Stump.Database.WorldServer.Storage;
using Stump.Database.WorldServer.Zaap;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;

namespace Stump.Database.WorldServer.Character
{
    [ActiveRecord("characters"), JoinedBase]
    public class CharacterRecord : WorldBaseRecord<CharacterRecord>, IRestrictable
    {
        private IList<QuestRecord> m_activeQuests;
        private IList<QuestRecord> m_finishedQuests;
        private IList<JobRecord> m_jobs;
        private IList<CharacterSpellRecord> m_spells;
        private IList<ZaapRecord> m_zaaps;

        #region Base

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("Name", Length = 18, NotNull = true)]
        public string Name
        {
            get;
            set;
        }

        [Property("Restrictions", NotNull = true, Default = "0")]
        public int Restrictions
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

        [Property("BaseHealth", NotNull = true)]
        public uint BaseHealth
        {
            get;
            set;
        }

        [Property("DamageTaken", NotNull = true)]
        public uint DamageTaken
        {
            get;
            set;
        }

        [Property("LastConnection")]
        public DateTime LastConnection
        {
            get;
            set;
        }

        #endregion

        #region Look

        [Property("Sex", NotNull = true)]
        public SexTypeEnum Sex
        {
            get;
            set;
        }

        [Property("BonesId", NotNull = true, Default = "1")]
        private uint BonesId
        {
            get;
            set;
        }

        [Property("Skins", NotNull = true)]
        private string Skins
        {
            get;
            set;
        }

        [Property("Color1", NotNull = true)]
        private int Color1
        {
            get;
            set;
        }

        [Property("Color2", NotNull = true)]
        private int Color2
        {
            get;
            set;
        }

        [Property("Color3", NotNull = true)]
        private int Color3
        {
            get;
            set;
        }

        [Property("Color4", NotNull = true)]
        private int Color4
        {
            get;
            set;
        }

        [Property("Color5", NotNull = true)]
        private int Color5
        {
            get;
            set;
        }

        [Property("Height", NotNull = true)]
        private int Height
        {
            get;
            set;
        }

        [Property("Width", NotNull = true)]
        private int Width
        {
            get;
            set;
        }

        public EntityLook BaseLook
        {
            get
            {
                List<uint> skins = Skins.Split(',').ToList().ConvertAll(s => uint.Parse(s));
                return new EntityLook(BonesId, skins, new List<int>(5) {Color1, Color2, Color3, Color4, Color5},
                                      new List<int>(2) {Height, Width}, new List<SubEntity>());
            }
            set
            {
                BonesId = value.bonesId;
                Skins = string.Join(",", value.skins);
                Color1 = value.indexedColors[0];
                Color2 = value.indexedColors[1];
                Color3 = value.indexedColors[2];
                Color4 = value.indexedColors[3];
                Color5 = value.indexedColors[4];
                if (value.scales.Count == 2)
                {
                    Height = value.scales[0];
                    Width = value.scales[1];
                }
                else
                {
                    Height = value.scales[0];
                    Width = value.scales[0];
                }
            }
        }

        [Property("TitleId", NotNull = true, Default = "0")]
        public uint TitleId
        {
            get;
            set;
        }

        [Property("TitleParam", NotNull = true, Default = "")]
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

        #endregion

        #region Position

        [Property("MapId", NotNull = true)]
        public int MapId
        {
            get;
            set;
        }

        [Property("CellId", NotNull = true)]
        public ushort CellId
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

        [Property("Strength", NotNull = true)]
        public int Strength
        {
            get;
            set;
        }

        [Property("Chance", NotNull = true)]
        public int Chance
        {
            get;
            set;
        }

        [Property("Vitality", NotNull = true)]
        public int Vitality
        {
            get;
            set;
        }

        [Property("Wisdom", NotNull = true)]
        public int Wisdom
        {
            get;
            set;
        }

        [Property("Intelligence", NotNull = true)]
        public int Intelligence
        {
            get;
            set;
        }

        [Property("Agility", NotNull = true)]
        public int Agility
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
        public uint EnergyMax
        {
            get;
            set;
        }

        [Property("Energy", NotNull = true, Default = "10000")]
        public uint Energy
        {
            get;
            set;
        }

        [Property("StatsPoints", NotNull = true, Default = "0")]
        public int StatsPoints
        {
            get;
            set;
        }

        [Property("SpellsPoints", NotNull = true, Default = "0")]
        public int SpellsPoints
        {
            get;
            set;
        }

        #endregion

        #region Inventory

        [BelongsTo("InventoryId", NotNull = true, Cascade = CascadeEnum.Delete)]
        public InventoryRecord Inventory
        {
            get;
            set;
        }

        #endregion

        #region Spell

        [HasMany(typeof (CharacterSpellRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<CharacterSpellRecord> Spells
        {
            get { return m_spells ?? new List<CharacterSpellRecord>(); }
            set { m_spells = value; }
        }

        public IDictionary<uint, CharacterSpellRecord> SpellsDictionnary
        {
            get { return Spells.ToDictionary(s => s.SpellId); }
            set { Spells = value.Select(k => k.Value).ToList(); }
        }

        #endregion

        #region Zaaps

        [HasMany(typeof (ZaapRecord))]
        public IList<ZaapRecord> Zaaps
        {
            get { return m_zaaps ?? new List<ZaapRecord>(); }
            set { m_zaaps = value; }
        }

        #endregion

        #region Quests

        [HasMany(typeof (QuestRecord))]
        public IList<QuestRecord> ActiveQuests
        {
            get { return m_activeQuests ?? new List<QuestRecord>(); }
            set { m_activeQuests = value; }
        }

        // [HasMany(typeof(typeof(int)), ColumnKey="", Table="")]
        public IList<QuestRecord> FinishedQuests
        {
            get { return m_finishedQuests ?? new List<QuestRecord>(); }
            set { m_finishedQuests = value; }
        }

        #endregion

        #region Jobs

        [HasMany(typeof (JobRecord))]
        public IList<JobRecord> Jobs
        {
            get { return m_jobs ?? new List<JobRecord>(); }
            set { m_jobs = value; }
        }

        #endregion

        #region Alignment

        [OneToOne(Cascade = CascadeEnum.Delete)]
        public AlignmentRecord Alignment
        {
            get;
            set;
        }

        #endregion

        #region Merchant

        [OneToOne]
        public SellBagRecord SellBag
        {
            get;
            set;
        }

        #endregion

        #region Guild

        [OneToOne]
        public GuildMemberRecord GuildMember
        {
            get;
            set;
        }

        #endregion

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

        public static CharacterRecord FindById(int characterId)
        {
            return FindByPrimaryKey(characterId);
        }

        public static CharacterRecord FindByName(string characterName)
        {
            return FindOne(NHibernate.Criterion.Restrictions.Eq("Name", characterName));
        }

        public static bool IsNameExists(string name)
        {
            return Exists(NHibernate.Criterion.Restrictions.Eq("Name", name));
        }

        public static int GetCount()
        {
            return Count();
        }
    }
}