using System;
using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.Interfaces;
using Stump.Server.WorldServer.Worlds.Actors.Stats;
using Stump.Server.WorldServer.Worlds.Breeds;
using Stump.Server.WorldServer.Worlds.Items;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid,
        IStatsOwner, IInventoryOwner
    {
        private readonly CharacterRecord m_record;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;

            LoadRecord();
        }

        #region Properties

        public WorldClient Client
        {
            get;
            private set;
        }

        public bool InWorld
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return m_record.Id; }
            protected set
            {
                m_record.Id = value;
                base.Id = value;
            }
        }

        public Inventory Inventory
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return m_record.Name; }
            protected set
            {
                m_record.Name = value;
                base.Name = value;
            }
        }

        #region Position

        public Map Map
        {
            get { return Position.Map; }
            set { Position.Map = value; }
        }

        public Cell Cell
        {
            get { return Position.Cell; }
            set { Position.Cell = value; }
        }

        public DirectionsEnum Direction
        {
            get { return Position.Direction; }
            set { Position.Direction = value; }
        }

        #endregion

        #region Apparence

        public override EntityLook Look
        {
            get { return m_record.EntityLook; }
            protected set
            {
                m_record.EntityLook = value;
                base.Look = value;
            }
        }

        public SexTypeEnum Sex
        {
            get { return m_record.Sex; }
            private set { m_record.Sex = value; }
        }

        public PlayableBreedEnum BreedId
        {
            get { return m_record.Breed; }
            private set
            {
                m_record.Breed = value; 
                Breed = BreedManager.Instance.GetBreed(value);
            }
        }

        public Breed Breed
        {
            get;
            private set;
        }

        #endregion

        #region Stats

        public delegate void LevelChangedHandler(Character character, byte currentLevel, int difference);
        public event LevelChangedHandler LevelChanged;

        public void NotifyLevelChanged(byte currentlevel, int difference)
        {
            LevelChangedHandler handler = LevelChanged;
            if (handler != null)
                handler(this, currentlevel, difference);
        }

        public byte Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return m_record.Experience; }
            set
            {
                m_record.Experience = value;
                if (value >= UpperBoundExperience)
                {
                    var lastLevel = Level;

                    Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);

                    LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
                    UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

                    var difference = Level - lastLevel;

                    NotifyLevelChanged(Level, difference);
                }
            }
        }

        public long LowerBoundExperience
        {
            get;
            private set;
        }

        public long UpperBoundExperience
        {
            get;
            private set;
        }

        public ushort StatsPoints
        {
            get { return m_record.StatsPoints; }
            set { m_record.StatsPoints = value; }
        }

        public ushort SpellsPoints
        {
            get { return m_record.SpellsPoints; }
            set { m_record.SpellsPoints = value; }
        }

        public short EnergyMax
        {
            get { return m_record.EnergyMax; }
            set { m_record.EnergyMax = value; }
        }

        public short Energy
        {
            get { return m_record.Energy; }
            set { m_record.Energy = value; }
        }

        public StatsFields Stats
        {
            get;
            private set;
        }

        #endregion

        #region Alignment

        public AlignmentSideEnum AlignmentSide
        {
            get;
            private set;
        }

        public byte AlignmentValue
        {
            get;
            private set;
        }

        public byte AlignmentGrade
        {
            get;
            private set;
        }

        public ushort Dishonor
        {
            get;
            private set;
        }

        public int CharacterPower
        {
            get { return Id + Level; }
        }

        #endregion

        #endregion

        /// <summary>
        ///   Spawn the character on the map. It can be called once.
        /// </summary>
        public void LogIn()
        {
            if (InWorld)
                return;

            Map.Enter(this);
            World.Instance.Enter(this);

            // todo: send MOTD

            InWorld = true;
        }

        public void LogOut()
        {
            if (InWorld)
            {
                if (Map != null)
                    Map.Leave(this);

                World.Instance.Leave(this);
            }

            SaveLater();
        }

        #region Save & Load

        public void SaveLater()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(SaveNow);
        }

        public void SaveNow()
        {
            m_record.MapId = Map.Id;
            m_record.CellId = Cell.Id;
            m_record.Direction = Direction;

            m_record.AP = (ushort) Stats[CaracteristicsEnum.AP].Base;
            m_record.MP = (ushort) Stats[CaracteristicsEnum.MP].Base;
            m_record.Strength = Stats[CaracteristicsEnum.Strength].Base;
            m_record.Agility = Stats[CaracteristicsEnum.Agility].Base;
            m_record.Chance = Stats[CaracteristicsEnum.Chance].Base;
            m_record.Intelligence = Stats[CaracteristicsEnum.Intelligence].Base;
            m_record.Wisdom = Stats[CaracteristicsEnum.Wisdom].Base;
            m_record.BaseHealth = (ushort) (Stats[CaracteristicsEnum.Health] as StatsHealth).Base;
            m_record.DamageTaken = (ushort) ( Stats[CaracteristicsEnum.Health] as StatsHealth ).DamageTaken;


            m_record.Save();
        }

        private void LoadRecord()
        {
            Breed = BreedManager.Instance.GetBreed(BreedId);

            Map map = World.Instance.GetMap(m_record.MapId);
            Position = new ObjectPosition(
                map,
                map.Cells[m_record.CellId],
                m_record.Direction);

            Stats = new StatsFields(this, m_record);
            Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetCharacterLevelExperience(Level);
            UpperBoundExperience = ExperienceManager.Instance.GetCharacterNextLevelExperience(Level);

            Inventory = new Inventory(this, m_record.Inventory);
        }

        #endregion

        #region Network

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayCharacterInformations(
                Id,
                Look,
                GetEntityDispositionInformations(),
                Name,
                GetHumanInformations(),
                GetActorAlignmentInformations());
        }

        #region ActorAlignmentInformations

        public ActorAlignmentInformations GetActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(
                (byte)AlignmentSide,
                AlignmentValue,
                AlignmentGrade,
                Dishonor,
                CharacterPower);
        }

        #endregion

        #region ActorExtendedAlignmentInformations

        public ActorExtendedAlignmentInformations GetActorAlignmentExtendInformations()
        {
            return new ActorExtendedAlignmentInformations(
                            (byte)AlignmentSide,
                            AlignmentValue,
                            AlignmentGrade,
                            Dishonor,
                            CharacterPower,
                            0,
                            0,
                            0,
                            false
                            );
        }

        #endregion

        #region CharacterBaseInformations

        public CharacterBaseInformations GetCharacterBaseInformations()
        {
            return new CharacterBaseInformations(
                Id,
                Level,
                Name,
                Look,
                (byte) BreedId,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        #endregion

        #endregion
    }
}