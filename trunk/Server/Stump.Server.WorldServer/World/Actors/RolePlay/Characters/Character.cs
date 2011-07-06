using Stump.Core.Cache;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.World.Actors.RolePlay.Characters
{
    public sealed class Character : Humanoid
    {
        private readonly CharacterRecord m_record;

        public Character(CharacterRecord record, WorldClient client)
        {
            m_record = record;
            Client = client;
        }

        public WorldClient Client
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return m_record.Id; }
            protected set { m_record.Id = value; }
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

        #region Stats
        public byte Level
        {
            get;
            set;
        }

        #endregion

        #region Alignment

        private AlignmentSideEnum m_alignmentSide;

        public AlignmentSideEnum AlignmentSide
        {
            get { return m_alignmentSide; }
            private set
            {
                m_alignmentSide = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        private byte m_alignmentValue;

        public byte AlignmentValue
        {
            get { return m_alignmentValue; }
            private set
            {
                m_alignmentValue = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        private byte m_alignmentGrade;

        public byte AlignmentGrade
        {
            get { return m_alignmentGrade; }
            private set
            {
                m_alignmentGrade = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        private ushort m_dishonor;

        public ushort Dishonor
        {
            get { return m_dishonor; }
            private set
            {
                m_dishonor = value;
                m_actorAlignmentInformations.Invalidate();
            }
        }

        private int m_characterPower;

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

        #region Save

        public void SaveLater()
        {
            WorldServer.Instance.IOTaskPool.EnqueueTask(SaveNow);
        }

        public void SaveNow()
        {
        }

        #endregion

        #region Network

        protected override void InitializeValidators()
        {
            base.InitializeValidators();

            m_actorAlignmentInformations = new ObjectValidator<ActorAlignmentInformations>(BuildActorAlignmentInformations);
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