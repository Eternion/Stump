using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid
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

        protected override void OnPositionChanged(ObjectPosition position)
        {
            m_record.MapId = position.Map.Id;
            m_record.CellId = position.Cell.Id;
            m_record.Direction = position.Direction;

            base.OnPositionChanged(position);
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

        public PlayableBreedEnum Breed
        {
            get { return m_record.Breed; }
            private set { m_record.Breed = value; }
        }

        #endregion

        #region Stats

        public byte Level
        {
            get;
            private set;
        }

        #endregion

        #region Alignment

        private byte m_alignmentGrade;
        private AlignmentSideEnum m_alignmentSide;

        private byte m_alignmentValue;
        private int m_characterPower;
        private ushort m_dishonor;

        public AlignmentSideEnum AlignmentSide
        {
            get { return m_alignmentSide; }
            private set
            {
                m_alignmentSide = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        public byte AlignmentValue
        {
            get { return m_alignmentValue; }
            private set
            {
                m_alignmentValue = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        public byte AlignmentGrade
        {
            get { return m_alignmentGrade; }
            private set
            {
                m_alignmentGrade = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        public ushort Dishonor
        {
            get { return m_dishonor; }
            private set
            {
                m_dishonor = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        public int CharacterPower
        {
            get { return m_characterPower; }
            private set
            {
                m_characterPower = value;
                m_actorAlignmentInformations.Invalidate();
            }
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

            Position.Map.Enter(this);
            World.Instance.Enter(this);

            InWorld = true;
        }

        public void LogOut()
        {
        }

        #region Save & Load

        public void SaveLater()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(SaveNow);
        }

        public void SaveNow()
        {
        }

        private void LoadRecord()
        {
            Map map = World.Instance.GetMap(m_record.MapId);
            Position = new ObjectPosition(
                map,
                map.Cells[m_record.CellId],
                m_record.Direction);

            Level = ExperienceManager.Instance.GetCharacterLevel(m_record.Experience);
        }

        #endregion

        #region Network

        protected override void InitializeValidators()
        {
            base.InitializeValidators();

            m_actorAlignmentInformations =
                new ObjectValidator<ActorAlignmentInformations>(BuildActorAlignmentInformations);
            m_actorAlignmentInformations.ObjectInvalidated += OnAlignementInvalidation;
        }

        protected override GameContextActorInformations BuildGameContextActorInformations()
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

        private ObjectValidator<ActorAlignmentInformations> m_actorAlignmentInformations;

        private void OnAlignementInvalidation(ObjectValidator<ActorAlignmentInformations> validator)
        {
            m_gameContextActorInformations.Invalidate();
        }

        private ActorAlignmentInformations BuildActorAlignmentInformations()
        {
            return new ActorAlignmentInformations(
                (byte) AlignmentSide,
                AlignmentValue,
                AlignmentGrade,
                Dishonor,
                CharacterPower);
        }

        public ActorAlignmentInformations GetActorAlignmentInformations()
        {
            return m_actorAlignmentInformations;
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
                (byte) Breed,
                Sex == SexTypeEnum.SEX_FEMALE);
        }

        #endregion

        #endregion
    }
}