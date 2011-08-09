using Stump.Core.Cache;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Worlds.Actors
{
    public abstract class ContextActor
    {
        protected ContextActor()
        {
// ReSharper disable DoNotCallOverridableMethodsInConstructor
            InitializeValidators();
// ReSharper restore DoNotCallOverridableMethodsInConstructor
        }

        private int m_id;

        public virtual int Id
        {
            get { return m_id; }
            protected set
            {
                m_id = value;
                m_gameContextActorInformations.Invalidate();
            }
        }

        private EntityLook m_look;

        public virtual EntityLook Look
        {
            get { return m_look; }
            protected set
            {
                m_look = value;
                m_gameContextActorInformations.Invalidate();
            }
        }

        private ObjectPosition m_position;

        public virtual ObjectPosition Position
        {
            get { return m_position; }
            protected set
            {
                m_position = value;
                m_position.PositionChanged += OnPositionChanged;
                m_gameContextActorInformations.Invalidate();
            }
        }

        #region Network

        protected virtual void InitializeValidators()
        {
            m_entityDispositionInformations = new ObjectValidator<EntityDispositionInformations>(BuildEntityDispositionInformations);
            m_gameContextActorInformations = new ObjectValidator<GameContextActorInformations>(BuildGameContextActorInformations);
        }

        #region EntityDispositionInformations

        protected ObjectValidator<EntityDispositionInformations> m_entityDispositionInformations;

        protected virtual void OnPositionChanged(ObjectPosition position)
        {
            m_entityDispositionInformations.Invalidate();
        }

        protected virtual EntityDispositionInformations BuildEntityDispositionInformations()
        {
            return new EntityDispositionInformations(Position.Cell.Id, (byte) Position.Direction);
        }

        public EntityDispositionInformations GetEntityDispositionInformations()
        {
            return m_entityDispositionInformations;
        }

        #endregion

        #region GameContextActorInformations

        protected ObjectValidator<GameContextActorInformations> m_gameContextActorInformations;

        protected virtual GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameContextActorInformations(
                Id,
                Look,
                GetEntityDispositionInformations());
        }

        public GameContextActorInformations GetGameContextActorInformations()
        {
            return m_gameContextActorInformations;
        }


        public IdentifiedEntityDispositionInformations GetIdentifiedEntityDispositionInformations()
        {
            return new IdentifiedEntityDispositionInformations(Position.Cell.Id, (byte) Position.Direction, Id);
        }

        #endregion

        #endregion
    }
}